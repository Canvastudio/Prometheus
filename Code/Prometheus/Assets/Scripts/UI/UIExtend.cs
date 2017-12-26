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

    static string NormalFormat = "<color=white>{0}</color>";
    static string UpFormat = "<color=green>{0}</color>";
    static string downFormat = "<color=white>{0}</color>";

    public static string richColor = "<color=#$>{0}</color>";

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
        float p1 = item.Property.GetIntProperty(property);
        float p2 = item.OriginProperty.GetIntProperty(property);

        t.text = p1.ToString();

        if (p1 < p2)
        {
            t.color = Color.red;
        }
        else if (p1 > p2)
        {
            t.color = Color.green;
        }
        else
        {
            t.color = Color.white;
        }
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
        int atk = item.Property.GetIntProperty(GameProperty.attack);
        int oatk = item.OriginProperty.GetIntProperty(GameProperty.attack);
        string satk;
        if (atk < oatk)
        {
            satk = string.Format(downFormat, atk);
        }
        else if (atk > oatk)
        {
            satk = string.Format(UpFormat, atk);
        }
        else
        {
            satk = atk.ToString();
        }
        int v = 10;
        for (int i = 1; i < 4; ++i)
        {
            if (atk / v == 0)
            {
                t.text = string.Format(gray_hp[i], satk);
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

    static public string ToX6(this Color c)
    {
        int i = 0xFFFFFF & (ColorToInt(c) >> 8);
        return DecimalToHex(i);
    }

    /// <summary>
    /// Convert the specified color to RGBA32 integer format.
    /// </summary>

    static public int ColorToInt(Color c)
    {
        int retVal = 0;
        retVal |= Mathf.RoundToInt(c.r * 255f) << 24;
        retVal |= Mathf.RoundToInt(c.g * 255f) << 16;
        retVal |= Mathf.RoundToInt(c.b * 255f) << 8;
        retVal |= Mathf.RoundToInt(c.a * 255f);
        return retVal;
    }



    static public string DecimalToHex(int num)
    {
        num &= 0xFFFFFF;
#if UNITY_FLASH
  StringBuilder sb = new StringBuilder();
  sb.Append(DecimalToHexChar((num >> 20) & 0xF));
  sb.Append(DecimalToHexChar((num >> 16) & 0xF));
  sb.Append(DecimalToHexChar((num >> 12) & 0xF));
  sb.Append(DecimalToHexChar((num >> 8) & 0xF));
  sb.Append(DecimalToHexChar((num >> 4) & 0xF));
  sb.Append(DecimalToHexChar(num & 0xF));
  return sb.ToString();
#else
        return num.ToString("X6");
#endif
    }

}
