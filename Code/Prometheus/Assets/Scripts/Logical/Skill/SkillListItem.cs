﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillListItem : DragableScrollItem
{
    [SerializeField]
    Button button;

    public int id;

    public ulong skill_id;

    public void Start()
    {
        HudEvent.Get(button.gameObject).onClick = OnClick;
    }

    public void SetInfo(ulong id)
    {
        skill_id = id;
        button.image.sprite = HelpFunction.GetSkillIcon(id, StageView.Instance.skillAtals);
    }

    public void OnLongPress()
    {

    }

    public void OnClick()
    {

    }
}