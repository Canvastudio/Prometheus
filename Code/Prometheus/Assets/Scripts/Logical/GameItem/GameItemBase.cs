using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemBase : MonoBehaviour {

    public bool isDiscovered = false;

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
            }

            OnSetStandBrick(value);

            _stanbBrick = value;
            _stanbBrick.item = this;
        }
    }

    [SerializeField]
    private Brick _stanbBrick;

    protected virtual void OnSetStandBrick(Brick brick)
    {

    }

    public virtual void OnDiscoverd()
    {

    }

    // 当对象已启用并处于活动状态时调用此函数
    private void Awake()
    {
        Messenger.AddListener(StageAction.RefreshGameItemPos, RefreshPosistion);
    }

    public void RefreshPosistion()
    {
        if (standBrick == null)
        {
            Debug.LogError(string.Format("{0} 没有 standbrick", gameObject.name));
        }
        else
        {
            transform.position = standBrick.transform.position;
        }
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(StageAction.RefreshGameItemPos, RefreshPosistion);
    }
}
