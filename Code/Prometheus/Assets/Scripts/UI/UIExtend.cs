using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIExtend {

    private static string hpft = "{0}/{1}";

    public static void SetSkillIcon(this Image image, string name)
    {
        image.sprite = StageView.Instance.skillAtals.GetSprite(name);
    }

    public static void SetStateIcon(this Image image, string name)
    {
        image.sprite = StageView.Instance.stateAtlas.GetSprite(name);
    }

    public static void SetItemIcon(this Image image, string name)
    {
        image.sprite = StageView.Instance.itemAtlas.GetSprite(name);
    }

    public static void SetPropertyText(this Text t, LiveItem item, GameProperty property)
    {
        t.text = item.Property.GetIntProperty(property).ToString();
    }

    public static void SetHpText(this Text t, LiveItem item)
    {
        t.text = string.Format(hpft, item.Property.GetIntProperty(GameProperty.nhp), item.Property.GetIntProperty(GameProperty.mhp));
    }
}
