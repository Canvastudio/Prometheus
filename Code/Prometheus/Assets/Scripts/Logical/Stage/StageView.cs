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


    [Space(5)]
    public Transform liveItemRoot;
    public Transform NonliveItemRoot;
    public Transform brickRoot;

    public GameObject stageGo;

    [Space(5)]
    public SpriteAtlas itemAtlas;
    public SpriteAtlas skillAtals;
    public SpriteAtlas stateAtlas;

    [Space(5)]
    public int viewBrickRow = 9;
    public float brickWidth = 120;
    public int column_per_row = 6;
    public string brickName = "brick";
    public string skillListItemName = "SLIN";
    public Camera show_camera;
    public RectTransform viewArea;


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

        return _brick;
    }

    List<Brick> pathBrick = new List<Brick>(20);

    public void SetNodeAsPath(List<Pathfinding.Node> list)
    {
        pathBrick.Clear();

        foreach (var node in list)
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
        if (GameManager.Instance.MapScroll)
        {
            if (StageCore.Instance.totalTime >= 4)
            {
                LeanTween.moveLocalY(
                    show_camera.gameObject,
                   show_camera.transform.localPosition.y + (brickWidth * .5f * distance / 100), distance);
            }

            Messenger.Invoke(SA.MapMoveDown);
        }
    }

    public override IEnumerator Init(object param)
    {
        ObjPool<Brick>.Instance.InitOrRecyclePool(brickName, _brickPrefab);

        return null;
    }

    public override IEnumerator Open(object param)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator Close(object param)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator Hide(object param)
    {
        throw new System.NotImplementedException();
    }
}
