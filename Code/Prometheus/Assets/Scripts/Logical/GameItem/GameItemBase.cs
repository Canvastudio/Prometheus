using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemBase : MonoBehaviour, ITagable {

    public int itemId = 0;

    public ulong featur = 0;

    public bool isDiscovered = false;

    public bool inViewArea = false;

    /// <summary>
    /// 当前依附的砖块
    /// </summary>
    public Brick standBrick
    {
        get
        {
            return _stanbBrick;
        }
        set
        {
            if (_stanbBrick != null)
            {
                _stanbBrick.item = null;
                _stanbBrick.brickType = BrickType.EMPTY;
            }

            OnSetStandBrick(value);

            _stanbBrick = value;
            _stanbBrick.item = this;
        }
    }

    /// <summary>
    /// 不手动修改，如果写在entity那边 又需要一个字典 没有必要
    /// </summary>
    public HashSet<ETag> Etag
    {
        get;
        set;
    }

    [SerializeField]
    private Brick _stanbBrick;

    protected virtual void OnSetStandBrick(Brick brick)
    {

    }

    public virtual void OnDiscoverd()
    {
        isDiscovered = true;
        CheckViewArea();
    }

    public bool CheckViewArea()
    {
        bool visible = false;

        if (isDiscovered)
        {
            var screen_Pos = RectTransformUtility.WorldToScreenPoint(StageView.Instance.show_camera, transform.position);

            visible = RectTransformUtility.RectangleContainsScreenPoint(
                StageView.Instance.viewArea,
                screen_Pos,
                StageView.Instance.show_camera);

            if (visible != inViewArea)
            {
                inViewArea = visible;
                StageCore.Instance.tagMgr.SetEntityTag(this, ETag.Tag(ST.VISIBLE), inViewArea);

            }
        }

        return visible;
    }

    // 当对象已启用并处于活动状态时调用此函数
    private void Awake()
    {
        Messenger.AddListener(SA.RefreshGameItemPos, RefreshPosistion);
        Messenger.AddListener(SA.MapMoveDown, OnMapMoveDown);
    }

    public void RefreshPosistion()
    {
        if (standBrick != null)
        {
            transform.position = standBrick.transform.position;
        }

        CheckViewArea();
    }

    protected virtual void OnMapMoveDown()
    {
        CheckViewArea();
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(SA.RefreshGameItemPos, RefreshPosistion);
    }
}
