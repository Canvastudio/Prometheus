using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameItemBase : MonoBehaviour, ITagable {

    public int itemId = 0;

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

    protected virtual void OnEnable()
    {

    }

    public virtual IEnumerator OnDiscoverd()
    {
        isDiscovered = true;

        CheckViewArea();

        return null;
    }

    /// <summary>
    /// 检查是否在区域内
    /// </summary>
    /// <returns></returns>
    public bool CheckViewArea()
    {
        bool in_area = false;

        var screen_Pos = RectTransformUtility.WorldToScreenPoint(StageView.Instance.show_camera, transform.position);

        in_area = RectTransformUtility.RectangleContainsScreenPoint(
            StageView.Instance.viewArea,
            screen_Pos,
            StageView.Instance.show_camera);

        if (in_area)
        {
            if (inViewArea == false)
            {
                OnEnterIntoArea();
            }
        }
        else
        {
            if (inViewArea == true)
            {
                OnExitFromArea();
            }
        }

        inViewArea = in_area;

        return in_area;
    }

    protected virtual void OnEnterIntoArea()
    {
        StageCore.Instance.RegisterItem(this);
    }

    protected virtual void OnExitFromArea()
    {
        StageCore.Instance.UnRegisterItem(this);
    }

    // 当对象已启用并处于活动状态时调用此函数
    private void Awake()
    {
        Messenger.AddListener(SA.RefreshGameItemPos, RefreshPosistion);
        Messenger.AddListener(SA.PlayerMoveEnd, PlayerMoveEnd);
    }

    public virtual void Recycle()
    {
        ResetValues();
    }

    public void RefreshPosistion()
    {
        if (standBrick != null)
        {
            transform.position = standBrick.transform.position;
        }

        CheckViewArea();
    }

    protected virtual void PlayerMoveEnd()
    {
        CheckViewArea();
    }


    private void OnDisable()
    {
        Messenger.RemoveListener(SA.RefreshGameItemPos, RefreshPosistion);
        Messenger.RemoveListener(SA.PlayerMoveEnd, PlayerMoveEnd);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(SA.RefreshGameItemPos, RefreshPosistion);
        Messenger.RemoveListener(SA.PlayerMoveEnd, PlayerMoveEnd);
    }

    public virtual void ResetValues()
    {
        isDiscovered = false;
        inViewArea = false;
    }

    public override bool Equals(object other)
    {
        return (other as GameItemBase).itemId == itemId;
    }
}
