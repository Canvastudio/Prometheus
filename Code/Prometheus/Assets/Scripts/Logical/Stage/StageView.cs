using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

/// <summary>
/// 负责管理关卡界面中的物体的生成和初始化设置等
/// </summary>
public class StageView : SingleGameObject<StageView> {

    private int lastRow = 0;
    private int lastColumn = 0;
    [SerializeField]
    GameObject _brickPrefab;
    public Transform liveItemRoot;
    public Transform brickRoot;
    public Transform moveRoot;
    [SerializeField]
    public SpriteAtlas brickAtlas;
    public int viewBrickRow = 9;
    public float brickWidth = 120;
    public string brickName = "brick";
    public Camera show_camera;
    public RectTransform viewArea;

    protected override void Init()
    {
        base.Init();

        ObjPool.Instance.InitOrRecyclePool(brickName, _brickPrefab);
    }

    #region Add Brick
    public Brick CreateBrick(ulong select_Moduel, ulong select_level, BrickType type = BrickType.EMPTY, int row = -1, int col = -1)
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

        Brick _brick = (ObjPool.Instance.GetObjFromPoolWithID(out uid, brickName) as GameObject).GetComponent<Brick>();

        _brick.transform.SetParent(brickRoot);
        _brick.transform.localScale = Vector3.one;
        _brick.gameObject.SetActive(true);
        _brick.uid = uid;
        _brick.moduel_id = select_Moduel;
        _brick.level_id = select_level;
        ((RectTransform)_brick.transform).anchoredPosition = new Vector2(brickWidth * col + brickWidth / 2f, brickWidth * row + brickWidth / 2f);
        _brick.transform.SetAsFirstSibling();

        _brick.Init(row, col,  type);

#if UNITY_EDITOR
        _brick.name = row.ToString() + " : " + col.ToString() + " : " + _brick.realBrickType.ToString();
#endif

        return _brick;
    }
    #endregion

    List<Brick> pathBrick = new List<Brick>(20);

    public void SetNodeAsPath(List<Pathfinding.Node> list)
    {
        pathBrick.Clear();

        foreach(var node in list)
        {
            var brick = node.behavirour as Brick;

            brick.SetAsPathNode();

            pathBrick.Add(brick);
        }
    }

    public void CancelPahtNode()
    {
        for (int i = 0; i < pathBrick.Count; ++i)
        {
            pathBrick[i].CancelAsPathNode();
        }

        pathBrick.Clear();
    }

    public void MoveDownMap(float distance)
    {
        Debug.Log("move down: " + distance);
        //return;

        if (StageCore.Instance.turnTime >= 4)
        {
            LeanTween.moveLocalY(
                moveRoot.gameObject,
                moveRoot.transform.localPosition.y - (brickWidth * .5f * distance), 0.3f)
                .setOnComplete(BrickCore.Instance.CheckNeedRecycelBrick);
        }

        BrickCore.Instance.CheckNeedCreawteMoudel();

        Messenger.Invoke(SA.MapMoveDown);
    }

}
