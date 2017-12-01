using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiName
{
    public static string strChipView = "ChipView";
    public static string strChipUpdateView = "ChipUpdateView";
    public static string strSkillInfoView = "SkillInfoView";
    public static string strStageView = "StageView";
    public static string strChipDetailVew = "ChipDetailView";
    public static string strMonsterInfoView = "MonsterInfoView";
    public static string strRoleInfoView = "RoleInfoView";

    //public static string strStageUIView = "StageUIView";
}

public class MuiCore : SingleGameObject<MuiCore> {

    Dictionary<string, MuiBase> dictionary = new Dictionary<string, MuiBase>();

    public Camera uiCamera;

    Stack<MuiBase> stack = new Stack<MuiBase>();

    [SerializeField]
    string resourcePaht = "UI_Prefab/";

    private MuiBase GetMui(string name)
    {
        MuiBase ui = null;

        if (dictionary.TryGetValue(name, out ui))
        {
            return ui;
        }
        else
        {
            var go = GameObject.Find(name);

            if (go == null)
            {
                ui = GameObject.Instantiate<GameObject>(Resources.Load(resourcePaht + name) as GameObject).GetComponent<MuiBase>();
            }
            else
            {
                ui = go.GetComponent<MuiBase>();
            }


            if (ui == null)
            {
                Debug.LogError("1");
            }
            dictionary.Add(name, ui);
        }

        return ui;
    }

    public IEnumerator Init(string name, object param = null)
    {
        var mui = GetMui(name);
        yield return mui.Init(param);
        mui.gameObject.SetActive(false);
        mui.isHide = true;
    }

    public IEnumerator OpenIE(string name, object param = null)
    {
        MuiBase ui;

        while(stack.Count > 0)
        {
            ui = stack.Pop();
            ui.Hide();
        }

        ui = GetMui(name);

        yield return GetMui(name).Open(param); ;

        stack.Push(ui);
    }
    public void Open(string name, object param = null)
    {
        StartCoroutine(OpenIE(name, param));
    }

    public IEnumerator AddOpen(string name, object param = null)
    {
        var ui = GetMui(name);

        yield return GetMui(name).Open(param);

        stack.Push(ui);
    }

    //public void Hide(string name, object param = null)
    //{
    //    GetMui(name).isHide = true;
    //    GetMui(name).Hide(param);
    //}

    public void HideTop()
    {
        var ui = stack.Pop();
        ui.Hide();
    }

    public IEnumerator Close(string name, object param)
    {
        yield return GetMui(name).Close(param);
    }
}
