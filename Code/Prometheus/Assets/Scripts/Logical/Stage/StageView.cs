using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

/// <summary>
/// 负责管理关卡界面中的物体的生成和初始化设置等
/// </summary>
public class StageView : MuiSingleBase<StageView>
{
    private int lastRow = 0;
    private int lastColumn = 0;

    [Space(5)]
    [SerializeField]
    Brick _brickPrefab;
    [SerializeField]
    ArtWayPoint wayPoint;

    [SerializeField]
    public Transform lower;
    [SerializeField]
    public Transform uper;

    [Space(5)]
    public Transform liveItemRoot;
    public Transform NonliveItemRoot;
    public Transform brickRoot;

    [Space(5)]
    public SpriteAtlas itemAtlas;
    public SpriteAtlas skillAtals;
    public SpriteAtlas stateAtlas;

    [Space(5)]
    public int viewBrickRow = 9;
    public float brickWidth = 120;
    public int column_per_row = 6;
    public string brickName = "brick";
    public Camera show_camera;

    private string strblockMask = "blockMask";




    [SerializeField]
    Transform blockMask;
    [SerializeField]
    Transform maskRoot;

    public Brick CreateBrick(ulong select_Moduel, ulong select_level, int row = -1, int col = -1)
    {
        if (col == -1)
        {
            if (lastColumn > Predefine.BRICK_VIEW_WIDTH - 1)
            {
                col = 0;
                lastColumn = 1;
                row = ++lastRow;
            }
            else
            {
                col = lastColumn++;
                row = lastRow;
            }
        }

        int uid = 0;

        Brick _brick = ObjPool<Brick>.Instance.GetObjFromPoolWithID(out uid, brickName);

        _brick.transform.SetParent(brickRoot);
        _brick.transform.localScale = Vector3.one;
        _brick.transform.localPosition = Vector3.zero;
        _brick.gameObject.SetActive(true);
        _brick.itemId = uid;
        _brick.moduel_id = select_Moduel;
        _brick.level_id = select_level;
        float offset = (750 - brickWidth * column_per_row) / 2f;
        ((RectTransform)_brick.transform).anchoredPosition = new Vector2(offset + brickWidth * col + brickWidth / 2f, brickWidth * row + brickWidth / 2f);
        _brick.transform.SetAsFirstSibling();

        _brick.Init(row, col);

#if UNITY_EDITOR
        _brick.name = row.ToString() + " : " + col.ToString() + " : " + _brick.realBrickType.ToString();
#endif

        _brick.standBrick = _brick;

        if(_brick.haloComponent == null)
        {
            _brick.haloComponent = _brick.GetOrAddComponet<HaloComponent>();
        }

        _brick.ListenInit();

        _brick.icon.SetBrickIcon(row, col);

        return _brick;
    }

    List<Brick> pathBrick = new List<Brick>(20);

    public void SetNodeAsPath(List<Pathfinding.Node> list)
    {

        wayPoint.gameObject.SetActive(true);
        Vector3[] vectors = new Vector3[list.Count + 1];
        vectors[0] = StageCore.Instance.Player.transform.position;
        for (int i = 0; i < list.Count; ++i)
        {
            vectors[i + 1] = list[i].behavirour.transform.position;
        }
        wayPoint.SetWayPoints(vectors);
    }

    public void CancelPahtNode()
    {
        wayPoint.gameObject.SetActive(false);
    }

    public void MoveDownMap(float distance)
    {
        if (!GameTestData.Instance.NoSroll)
        {
            if (StageCore.Instance.totalTime >= 4)
            {
                GCamera.Instance.MoveDown(distance);
            }
        }
    }

    public int AddBlockMask(Brick brick, ref Transform mask)
    {
        int _id;
        mask = ObjPool<Transform>.Instance.GetObjFromPoolWithID(out _id, strblockMask);
        mask.SetParentAndNormalize(maskRoot);
        mask.gameObject.SetActive(true);
        mask.position = brick.transform.position;
        return _id;
    }

    public void RemoveBlockMask(int id)
    {
        ObjPool<Transform>.Instance.RecycleObj(strblockMask, id);
    }

    public override IEnumerator Init(object param)
    {
        int w = Screen.width;
        int h = Screen.height;

        transform.Rt().sizeDelta = new Vector2(750f, h * 750f / w);

        ObjPool<Brick>.Instance.InitOrRecyclePool(brickName, _brickPrefab);
        ObjPool<Transform>.Instance.InitOrRecyclePool(strblockMask, blockMask);

        //生成地图，怪物
        BrickCore.Instance.CreatePrimitiveStage();

        //生成玩家
        BrickCore.Instance.CreatePlayer(1);

        yield return 0;

        //刷新下位置
        Messenger.Invoke(SA.RefreshGameItemPos);

    }

    private void ShowSkillInfo()
    {
        StartCoroutine(MuiCore.Instance.AddOpen(UiName.strSkillInfoView));
    }

    public override IEnumerator Open(object param)
    {
        gameObject.SetActive(true);

        yield return MuiCore.Instance.AddOpen(UiName.strStageUIView);
    }

    public override IEnumerator Close(object param)
    {
        throw new System.NotImplementedException();
    }

    public override void Hide(object param)
    {
        gameObject.SetActive(false);
    }

    Dictionary<string, List<ParticleSystem>> fxData = new Dictionary<string, List<ParticleSystem>>();

    public IEnumerator ShowFx(Brick brick, string fxName)
    {
        Debug.Log("尝试播放关卡特效: " + fxName);
        List<ParticleSystem> fxs;
        ParticleSystem fx;
        float time = 0;

        if (!fxData.TryGetValue(fxName, out fxs))
        {
            string name = SpecialEffectConfig.GetConfigDataByKey<SpecialEffectConfig>(fxName).effectName;
            fx = GameObject.Instantiate<GameObject>(Resources.Load("Fx/" + name) as GameObject).GetComponent<ParticleSystem>();
            fxs = new List<ParticleSystem>();
        }
        else
        {
            if (fxs.Count > 0)
            {
                fx = fxs[0];
            }
            else
            {
                fx = GameObject.Instantiate<GameObject>(Resources.Load("Fx/" + name) as GameObject).GetComponent<ParticleSystem>();
            }
        }

        time = fx.main.startLifetime.constant;
        fx.gameObject.SetActive(true);
        fx.transform.position = brick.transform.position;

        yield return new WaitForSeconds(time + 2f);

        fx.gameObject.SetActive(false);
        fxs.Add(fx);
    }

    public ParticleSystem ShowFxLoop(Brick brick, string fxName)
    {
        Debug.Log("尝试播放循环关卡特效: " + fxName);
        List<ParticleSystem> fxs;
        ParticleSystem fx;

        if (!fxData.TryGetValue(fxName, out fxs))
        {
            string name = SpecialEffectConfig.GetConfigDataByKey<SpecialEffectConfig>(fxName).effectName;
            fx = GameObject.Instantiate<GameObject>(Resources.Load("Fx/" + name) as GameObject).GetComponent<ParticleSystem>();
            fxs = new List<ParticleSystem>();
        }
        else
        {
            if (fxs.Count > 0)
            {
                fx = fxs[0];
            }
            else
            {
                fx = GameObject.Instantiate<GameObject>(Resources.Load("Fx/" + name) as GameObject).GetComponent<ParticleSystem>();
            }
        }

        fx.gameObject.SetActive(true);
        fx.transform.position = brick.transform.position;


        //fx.gameObject.SetActive(false);
        //fxs.Add(fx);

        return fx;
    }

    public void RecycleFx(string fxName, ParticleSystem fx)
    {
        fx.gameObject.SetActive(false);
        List<ParticleSystem> fxs;
        if (!fxData.TryGetValue(fxName, out fxs))
        {
            Debug.LogError("找不到Fx池子！？");
        }

        fxs.Add(fx);
    }


}
