﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillListItem : DragableScrollItem
{
    [SerializeField]
    Button button;
    [SerializeField]
    ActiveSkillsConfig config;
    [SerializeField]
    Image icon;
    [SerializeField]
    Text skillName;
    public int id;

    public ulong skill_id;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Start()
    {
        HudEvent.Get(button.gameObject).onClick = OnClick;
    }

    public void SetInfo(ulong id)
    {
        gameObject.SetActive(true);
        skill_id = id;
        config = ActiveSkillsConfig.GetConfigDataById<ActiveSkillsConfig>(id);
        icon.SetSkillIcon(config.icon);
        skillName.text = config.name;
    }

    public void OnLongPress()
    {

    }

    public void OnClick()
    {
        Messenger<ActiveSkillsConfig>.Invoke(SA.PlayerClickSkill, config);
    }
}
