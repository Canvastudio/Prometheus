using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDetailItem : MonoBehaviour {

    [SerializeField]
    Image skillIcon;
    [SerializeField]
    Text skillName;
    [SerializeField]
    Text skillUseing;
    [SerializeField]
    Text skillDescr;
    [SerializeField]
    Button button;

    ActiveSkillsConfig activeSkillsConfig;
    PassiveSkillsConfig PassiveSkillsConfig;

    public void Set(ActiveSkillsConfig config)
    {
        activeSkillsConfig = config;
        HudEvent.Get(button).onClick = OnClick;
        skillIcon.sprite = StageView.Instance.skillAtals.GetSprite(config.icon);
        skillName.text = config.name;
        skillDescr.text = config.describe;
    }

    public void Set(PassiveSkillsConfig config)
    {
        PassiveSkillsConfig = config;
        skillIcon.sprite = StageView.Instance.skillAtals.GetSprite(config.icon);
        skillName.text = config.name;
        skillDescr.text = config.describe;
        HudEvent.Get(button).onClick = null;
    }

    private void OnClick()
    {
        Messenger<ActiveSkillsConfig>.Invoke(SA.PlayerClickSkill, activeSkillsConfig);
    }
}
