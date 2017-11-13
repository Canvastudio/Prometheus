using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoView : MonoBehaviour {

    [SerializeField]
    Transform contentRoot;
    [SerializeField]
    SkillDetailItem item;
    [SerializeField]
    Button asButton;
    [SerializeField]
    Button psButton;

    private string pname = "sdip";

    /// <summary>
    /// 0显示主动 1 显示被动
    /// </summary>
    public int show_state = 0; 

    private void Awake()
    {
        ObjPool<SkillDetailItem>.Instance.InitOrRecyclePool(pname, item);
    }

    public void Show()
    {

    }

    private void ShowAcitveList()
    {
        ObjPool<SkillDetailItem>.Instance.RecyclePool(pname);
    }

    private void ShowPassiveList()
    {
        ObjPool<SkillDetailItem>.Instance.RecyclePool(pname);
    }
}
