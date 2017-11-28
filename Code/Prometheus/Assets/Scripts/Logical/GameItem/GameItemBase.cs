using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GameItemBase : MonoBehaviour, ITagable {

    public int itemId = 0;

    public bool isDiscovered = false;

    public bool inViewArea = false;

    private int action = 0;

    public Image icon;

    public CanvasGroup canvasGroup;

    public void OnActionBegin()
    {
        if (action == 0)
        {
            StageCore.Instance.action_item += 1;
        }

        action += 1;
    }

    public void OnActionEnd()
    {
        action -= 1;
            
        if (action == 0)
        {
            StageCore.Instance.action_item -= 1;
        }
    }

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

    public virtual IEnumerator OnDiscoverd()
    {
        //Debug.Log("发现: " + gameObject.name);

        isDiscovered = true;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;
        }

        CheckViewArea();

        StageCore.Instance.tagMgr.RemoveEntityTag(this, ETag.Tag(ST.UNDISCOVER));
        StageCore.Instance.tagMgr.AddEntity(this, ETag.Tag(ST.DISCOVER));

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

    public virtual void Recycle()
    {
        ResetValues();
    }

    protected virtual void OnEnable()
    {
        Messenger.AddListener(SA.RefreshGameItemPos, RefreshPosistion);
        Messenger.AddListener(SA.PlayerMoveEnd, PlayerMoveEnd);
    }

    private void RemoveLister()
    {
        Messenger.RemoveListener(SA.RefreshGameItemPos, RefreshPosistion);
        Messenger.RemoveListener(SA.PlayerMoveEnd, PlayerMoveEnd);
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



    public virtual void ResetValues()
    {
        isDiscovered = false;
        inViewArea = false;
        RemoveLister();
    }

    public override bool Equals(object other)
    {
        return (other as GameItemBase).itemId == itemId;
    }
}
