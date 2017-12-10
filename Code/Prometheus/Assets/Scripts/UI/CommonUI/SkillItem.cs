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
            skillCooldown.text = mins.cooldown.ToString();
        }
        else
        {
            cd.gameObject.SetActive(false);
        }

        describe.text = ins.config.describe;
        skillName.text = ins.config.name;
        skillIcon.SetSkillIcon(ins.config.icon);

        gameObject.SetActive(true);
    }

     public void ShowSkillInfo(ActiveSkillsConfig config)
    {
        cd.gameObject.SetActive(false);
        describe.text = config.describe;
        skillName.text = config.name;
        skillIcon.SetSkillIcon(config.icon);

        gameObject.SetActive(true);
    }

    public void ShowSkillInfo(PassiveSkillIns ins)
    {
        cd.gameObject.SetActive(false);

        describe.text = ins.passiveConfig.describe;
        skillName.text = ins.passiveConfig.name;
        skillIcon.SetSkillIcon(ins.passiveConfig.icon);

        gameObject.SetActive(true);
    }
}
