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

    [Space(5)]
    [SerializeField]
    Brick _brickPrefab;
    [SerializeField]
    SkillListItem _skillListItemPrefab;

    [Space(5)]
    public Transform liveItemRoot;
    public Transform NonliveItemRoot;
    public Transform brickRoot;
    public Transform moveRoot;
    public Transform skillListRoot;

    [Space(5)]
    public SpriteAtlas itemAtlas;
    public SpriteAtlas skillAtals;

    [Space(5)]
    public int viewBrickRow = 9;
    public float brickWidth = 120;
    public int column_per_row = 6;
    public string brickName = "brick";
    public string skillListItemName = "SLIN";
    public Camera show_camera;
    public RectTransform viewArea;

    private List<SkillListItem> skillItemList = new List<SkillListItem>(10);

    protected override void Init()
    {
        base.Init();

        ObjPool<Brick>.Instance.InitOrRecyclePool(brickName, _brickPrefab);
        ObjPool<SkillListItem>.Instance.InitOrRecyclePool(skillListItemName, _skillListItemPrefab);
    }

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

        return _brick;
    }

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
        if (GameManager.Instance.MapScroll)
        {
            if (StageCore.Instance.totalTime >= 4)
            {
                LeanTween.moveLocalY(
                    moveRoot.gameObject,
                    moveRoot.transform.localPosition.y - (brickWidth * .5f * distance), distance);
            }

            Messenger.Invoke(SA.MapMoveDown);
        }
    }

    public void AddSkillIntoSkillList(ulong uid)
    {
#if UNITY_EDITOR
        foreach(var item in skillItemList)
        {
            if (item.skill_id == uid)
            {
                Debug.LogError("青鑫：尝试在技能列表里重复的添加技能: id: " + uid.ToString());
            }
        }
#endif

        if (uid > 0 &&  FightComponet.IdToSkillType(uid) == SkillType.Active)
        {
            int _id;
            var list_item = ObjPool<SkillListItem>.Instance.GetObjFromPoolWithID(out _id, skillListItemName);
            list_item.id = _id;
            list_item.SetInfo(uid);
            list_item.SetParentAndNormalize(skillListRoot);
            skillItemList.Add(list_item);
        }
    }

    public void RemoveSkillFromSkillList(ulong uid)
    {
        if (FightComponet.IdToSkillType(uid) == SkillType.Active)
        {
            for (int i = 0; i < skillItemList.Count; ++i)
            {
                if (skillItemList[i].skill_id == uid)
                {
                    ObjPool<SkillListItem>.Instance.RecycleObj(skillListItemName, skillItemList[i].id);
                    skillItemList.RemoveAt(i);
                    return;
                }
            }
        }
    }

    public IEnumerator LightTargetWaitSelect(List<GameItemBase> list)
    {
        yield return 0;
    }

    public IEnumerator ShowEffectAndWaitHit(FightComponet fightComponet, ActiveSkillsConfig config)
    {
        yield return GameGlobalVariable.wait0_3s;

        fightComponet.hitTarget = true;
    }
}
