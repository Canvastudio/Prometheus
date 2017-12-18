using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameItemBase : MonoBehaviour, ITagable {

    public Button brickBtn;

    public ulong itemId = 0;

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
        if (action > 0)
        {
            action -= 1;
        }
            
        if (action == 0)
        {
            StageCore.Instance.action_item -= 1;
        }
    }

    public void ActiveReset()
    {
        if (action != 0)
        {
            action = 0;
            StageCore.Instance.action_item -= 1;
        }
    }

    void Start()
    {
        if (brickBtn != null)
        {
            HudEvent.Get(brickBtn.gameObject).onClick = OnBrickClick;
            HudEvent.Get(brickBtn.gameObject).onLongPress = OnLongPress;
            HudEvent.Get(brickBtn.gameObject).onLongPressRelease = LongPressRelease;
        }
    }

    protected virtual void OnBrickClick()
    {
        standBrick.OnBrickClick();
    }

    protected virtual void OnLongPress()
    {
        standBrick.OnLongPress();
    }

    protected virtual void LongPressRelease()
    {
        standBrick.LongPressRelease();
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
            OnSetStandBrick(value);

            _stanbBrick = value;

            if (_stanbBrick != null)
            {
                _stanbBrick.item = this;
            }
        }
    }

    /// <summary>
    /// 不手动修改，如果写在entity那边 又需要一个字典 没有必要
    /// </summary>
    private HashSet<ETag> _etag = new HashSet<ETag>();
    public HashSet<ETag> Etag
    {
        get
        {
            return _etag;
        }
        set { _etag = value; }
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
        if (!gameObject.activeInHierarchy)
        {
            //Debug.Log("没有激活的物体参与了checkViewArea? : " + gameObject.name);
            return false;
        }

        bool in_area = false;

        var screen_Pos = RectTransformUtility.WorldToScreenPoint(GameManager.Instance.GCamera, transform.position);

        in_area = RectTransformUtility.RectangleContainsScreenPoint(
            StageUIView.Instance.viewArea,
            screen_Pos,
            GameManager.Instance.GCamera);

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

    public virtual void ListenInit()
    {
        Messenger.AddListener(SA.RefreshGameItemPos, RefreshPosistion);
        Messenger.AddListener(SA.CamerMove, PlayerMoveEnd);
    }

    private void RemoveLister()
    {
        Messenger.RemoveListener(SA.RefreshGameItemPos, RefreshPosistion);
        Messenger.RemoveListener(SA.CamerMove, PlayerMoveEnd);
    }

    public void RefreshPosistion()
    {
        if (standBrick != null)
        {
            transform.position = standBrick.transform.position;
        }

        CheckViewArea();
    }

    public void PlayerMoveEnd()
    {
        CheckViewArea();
    }



    public void ResetValues()
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
