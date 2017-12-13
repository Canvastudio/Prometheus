using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{

    [SerializeField]
    Text describe;
    [SerializeField]
    Text skillName;
    [SerializeField]
    Text skillCooldown;
    [SerializeField]
    Image skillIcon;
    [SerializeField]
    GameObject cd;
    [SerializeField]
    GameObject cost;
    [SerializeField]
    Text c1;
    [SerializeField]
    Text c2;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowSkillInfo(ActiveSkillIns ins)
    {
        if (ins is MonsterActiveSkillIns)
        {
            var mins = ins as MonsterActiveSkillIns;
            cd.gameObject.SetActive(true);
            var cdv = (float)(Mathf.FloorToInt(mins.cooldown * 10f)) / 10f;
            skillCooldown.text = cdv.ToString();
            cost.SetActive(false);
        }
        else
        {
            int n = ins.config.stuffCost.stuffs.Count();
            for (int i = 0; i < n; ++i)
            {
                c2.text = ins.config.stuffCost.values[i].ToString();
            }
            cd.gameObject.SetActive(false);
            cost.SetActive(true);
        }

        describe.text = ins.config.describe;
        skillName.text = ins.config.name;
        skillIcon.SetSkillIcon(ins.config.icon);



        gameObject.SetActive(true);
    }

     public void ShowSkillInfo(ActiveSkillsConfig config)
    {
        int n = config.stuffCost.stuffs.Count();
        for (int i = 0; i < n; ++i)
        {
            c2.text = config.stuffCost.values[i].ToString();
        }
        cd.gameObject.SetActive(false);
        cost.SetActive(true);

        cd.gameObject.SetActive(false);
        describe.text = config.describe;
        skillName.text = config.name;
        skillIcon.SetSkillIcon(config.icon);

        gameObject.SetActive(true);
    }

    public void ShowSkillInfo(PassiveSkillIns ins)
    {
        cost.SetActive(false);
        cd.gameObject.SetActive(false);

        describe.text = ins.passiveConfig.describe;
        skillName.text = ins.passiveConfig.name;
        skillIcon.SetSkillIcon(ins.passiveConfig.icon);

        gameObject.SetActive(true);
    }
}
