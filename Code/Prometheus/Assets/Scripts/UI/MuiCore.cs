using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UiName
{
    ChipView,
    ChipUpdateView,
    SkillInfoView,
    BrickView,
}

public class MuiCore : SingleGameObject<MuiCore> {

    Dictionary<string, MuiBase> dictionary = new Dictionary<string, MuiBase>();

    public Camera uiCamera;

    List<IManagedUI> showList = new List<IManagedUI>();

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
            ui = GameObject.Find(name).GetComponent<MuiBase>();

            if (ui == null)
            {
                ui = GameObject.Instantiate<GameObject>(Resources.Load(resourcePaht + name) as GameObject).GetComponent<MuiBase>();
            }

            dictionary.Add(name, ui);
        }

        return ui;
    }

    public IEnumerator Init(string name, object param)
    {
        yield return GetMui(name).Init(param);
    }

    public IEnumerator Open(string name, object param)
    {
        yield return GetMui(name).Open(param);
    }

    public IEnumerator Hide(string name, object param)
    {
        yield return GetMui(name).Hide(param);
    }

    public IEnumerator Close(string name, object param)
    {
        yield return GetMui(name).Close(param);
    }
}
