using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemBase : MonoBehaviour {

    /// <summary>
    /// 当前依附的砖块
    /// </summary>
    public Brick standBrick;

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
