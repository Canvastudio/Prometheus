using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public static class UIExtend {

    private static string hpft = "{0}/{1}";
    private static StringBuilder stringBuilder = new StringBuilder(20);

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

    public static void SetStageItemIcon(this Image image, string name)
    {
        image.sprite = AtlasCore.Instance.Load("Stage").GetSprite(name);
    }

    public static void SetPropertyText(this Text t, LiveItem item, GameProperty property)
    {
        t.text = item.Property.GetIntProperty(property).ToString();
    }

    public static void SetHpText(this Text t, LiveItem item)
    {
        t.text = string.Format(hpft, item.Property.GetIntProperty(GameProperty.nhp), item.Property.GetIntProperty(GameProperty.mhp));
    }

    public static void SetChipDescrible(this Text t, ChipConfig config)
    {
        var describes = config.descrip.ToArray();
        stringBuilder.Clean();
        foreach(var d in describes)
        {
            if (!string.IsNullOrEmpty(d))
            {
                stringBuilder.Append(d);
                stringBuilder.Append("\n");
            }
        }

        t.text = stringBuilder.ToString();
    }

    public static void SetBrickIcon(this Image image, int row, int col)
    {
        string name = RuleBox.GetBox(row, col);
        image.sprite = AtlasCore.Instance.GetSpriteFormAtlas("Stage", name);
        image.SetNativeSize();
    }
}
