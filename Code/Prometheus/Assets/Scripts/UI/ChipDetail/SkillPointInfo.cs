using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class SkillPointInfo : MonoBehaviour
{
    public ulong uid;

    [SerializeField]
    Text skillName;
    [SerializeField]
    Text pointCount;
    [SerializeField]
    Text skillActive;
    [SerializeField]
    Text describe;

    string strPointCount = "({0})";
    static string unlockPoint;
    static string grey;

    static StringBuilder sb = new StringBuilder(12);

    private void Awake()
    {
        if (unlockPoint == null)
        {
            unlockPoint = UIExtend.richColor.Replace("$", ArtColor.Instance.unlock_lv.ToX6());
        }

        if (grey == null)
        {
            grey = UIExtend.richColor.Replace("$", ArtColor.Instance.lock_lv.ToX6());
        }
    }

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
                sb.Append(string.Format(unlockPoint, limit[i].ToString()));
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
        if (ins.point != null)
        {
            pointCount.text = string.Format(strPointCount, ins.point.count.ToString());
        }
        sb.Clean();
        var limit = ins.point.updateLimit;

        for (int i = 0; i < limit.Length; ++i)
        {
            if (i == ins.point.activeIndex)
            {
                sb.Append(string.Format(unlockPoint, limit[i].ToString()));
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

    public void Set(SkillPoint sp)
    {
        ulong skillId = sp.skillId;
        
        if (skillId == 0)
        {
            skillId = sp.skillIds[0];
        }

        SkillType type = FightComponet.IdToSkillType(skillId);

        switch(type)
        {
            case SkillType.Active:
                var aconfig = ActiveSkillsConfig.GetConfigDataById<ActiveSkillsConfig>(skillId);
                skillName.text = aconfig.name;
                if (sp.activeIndex >= 0)
                {
                    describe.text = aconfig.describe;
                    describe.gameObject.SetActive(true);
                    transform.Rt().sizeDelta = new Vector2(700, 140);
                }
                else
                {
                    describe.gameObject.SetActive(false);
                    transform.Rt().sizeDelta = new Vector2(700, 80);
                }
                break;
            case SkillType.Passive:
                var pconfig = PassiveSkillsConfig.GetConfigDataById<PassiveSkillsConfig>(skillId);
                skillName.text = pconfig.name;
                if (sp.activeIndex >= 0)
                {
                    describe.text = pconfig.describe;
                    describe.gameObject.SetActive(true);
                    transform.Rt().sizeDelta = new Vector2(700, 140);
                }
                else
                {
                    describe.gameObject.SetActive(false);
                    transform.Rt().sizeDelta = new Vector2(700, 80);
                }
                break;
        }

        pointCount.text = string.Format(strPointCount, sp.count.ToString());

        sb.Clean();

        var limit = sp.updateLimit;

        for (int i = 0; i < limit.Length; ++i)
        {
            if (i == sp.activeIndex)
            {
                sb.Append(string.Format(unlockPoint, limit[i].ToString()));
            }
            else
            {
                sb.Append(string.Format(grey, limit[i].ToString()));
            }

            if (i != limit.Length - 1)
            {
                sb.Append(string.Format(grey,"/"));
            }
        }

        skillActive.text = sb.ToString();

        if (sp.activeIndex >= 0)
        {
            skillName.color = Color.green;
            transform.SetAsFirstSibling();
        }
        else
        {
            skillName.color = ArtColor.Instance.lock_lv;
            pointCount.color = ArtColor.Instance.lock_lv;
        }

    }
}
