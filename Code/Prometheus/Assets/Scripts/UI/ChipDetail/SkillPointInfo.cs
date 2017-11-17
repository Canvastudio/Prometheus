using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class SkillPointInfo : MonoBehaviour
{
    public int uid;

    [SerializeField]
    Text skillName;
    [SerializeField]
    Text pointCount;
    [SerializeField]
    Text skillActive;
    [SerializeField]
    Text describe;

    string strPointCount = "({0})";
    string green = "<color=#00ff00>{0}</color>";

    static StringBuilder sb = new StringBuilder(12);

    public void Set(ActiveSkillIns ins)
    {
        skillName.text = ins.config.name;
        pointCount.text = string.Format(strPointCount, ins.point.count.ToString());
        sb.Clean();
        var limit = ins.point.updateLimit;

        for (int i = 0; i < limit.Length; ++i)
        {
            if (i == ins.point.activeIndex)
            {
                sb.Append(string.Format(green, limit[i].ToString()));
            }
            else
            {
                sb.Append(limit[i].ToString());
            }

            if (i != limit.Length -1)
            {
                sb.Append("/");
            }
        }

        skillActive.text = sb.ToString();
        describe.text = ins.config.describe;
    }

    public void Set(PassiveSkillIns ins)
    {
        skillName.text = ins.passiveConfig.name;
        pointCount.text = string.Format(strPointCount, ins.point.count.ToString());
        sb.Clean();
        var limit = ins.point.updateLimit;

        for (int i = 0; i < limit.Length; ++i)
        {
            if (i == ins.point.activeIndex)
            {
                sb.Append(string.Format(green, limit[i].ToString()));
            }
            else
            {
                sb.Append(limit[i].ToString());
            }

            if (i != limit.Length - 1)
            {
                sb.Append("/");
            }
        }

        skillActive.text = sb.ToString();
        describe.text = ins.passiveConfig.describe;
    }
}
