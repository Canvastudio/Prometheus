using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public static class UIExtend {

    private static string hpft = "{0}/{1}";

    static string[] gray_hp = new string[]
        {
            "<color=grey>000</color>{0}", "<color=grey>00</color>{0}", "<color=grey>0</color>{0}", "{0}",
        };


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
        image.sprite = AtlasCore.Instance.GetSpriteFormAtlas("stage", name);
    }

    public static void SetPropertyText(this Text t, LiveItem item, GameProperty property)
    {
        t.text = item.Property.GetIntProperty(property).ToString();
    }

    public static void SetOneFloat(this Text t, float f)
    {
        float ff = f * 10f;
        ff = ((float)Mathf.CeilToInt(ff)) / 10f;
        t.text = ff.ToString();
    }

    public static void SetHpText(this Text t, LiveItem item)
    {
        t.text = string.Format(hpft, item.Property.GetIntProperty(GameProperty.nhp), item.Property.GetIntProperty(GameProperty.mhp));
    }

    public static void SetIconHpText(this Text t, LiveItem item)
    {
        int nhp = item.Property.GetIntProperty(GameProperty.nhp);

        int v = 10;
        for (int i = 0; i < 4; ++i)
        {
            if (nhp / v == 0)
            {
                t.text = string.Format(gray_hp[i], nhp.ToString());
                return;
            }

            v *= 10;
        }
    }

    public static void SetIconAtkText(this Text t, LiveItem item)
    {
        int nhp = item.Property.GetIntProperty(GameProperty.attack);

        int v = 10;
        for (int i = 1; i < 4; ++i)
        {
            if (nhp / v == 0)
            {
                t.text = string.Format(gray_hp[i], nhp.ToString());
                return;
            }

            v *= 10;
        }
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
        image.color = RuleBox.GetBoxColor();
        image.SetNativeSize();
    }

    public static void SetBlockIcon(this Image image, int row, int col)
    {
        string name = RuleBox.GetBlock(row, col);
        image.sprite = AtlasCore.Instance.GetSpriteFormAtlas("Stage", name);
        image.color = RuleBox.GetBlockColor();
        image.SetNativeSize();
    }

    public static void SetCoverIcon(this Image image, int row, int col)
    {
        string name = RuleBox.GetUnExplored(row, col);
        image.sprite = AtlasCore.Instance.GetSpriteFormAtlas("Stage", name);
        image.color = RuleBox.GetUnExploredColor();
        image.SetNativeSize();
    }

}
