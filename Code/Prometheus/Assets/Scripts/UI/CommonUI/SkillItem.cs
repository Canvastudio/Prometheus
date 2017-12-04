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

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowSkillInfo(ActiveSkillIns ins)
    {
        if (ins is MonsterActiveSkillIns)
        {
            var mins = ins as MonsterActiveSkillIns;
            skillCooldown.gameObject.SetActive(true);
            skillCooldown.text = mins.cooldown.ToString();
        }
        else
        {
            skillCooldown.gameObject.SetActive(false);
        }

        describe.text = ins.config.describe;
        skillName.text = ins.config.name;
        skillIcon.SetSkillIcon(ins.config.icon);

        gameObject.SetActive(true);
    }
    public void ShowSkillInfo(PassiveSkillIns ins)
    {
        skillCooldown.gameObject.SetActive(false);
        describe.text = ins.passiveConfig.describe;
        skillName.text = ins.passiveConfig.name;
        skillIcon.SetSkillIcon(ins.passiveConfig.icon);

        gameObject.SetActive(true);
    }
}
