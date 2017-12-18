using System.Collections;
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
    public ulong id;
    [SerializeField]
    Text count;
    [SerializeField]
    Image frame;

    [SerializeField]
    Sprite normalSkillFrame;
    [SerializeField]
    Sprite organSkillFrame;

    public ActiveSkillIns ins;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Start()
    {
        HudEvent.Get(button.gameObject).onClick = OnClick;
        HudEvent.Get(button.gameObject).onLongPress = OnLongPress;
        HudEvent.Get(button.gameObject).onLongPressRelease = OnLongPressRelease;
    }

    public void SetInfo(ActiveSkillIns ins)
    {
        gameObject.SetActive(true);
        this.ins = ins;
        config = ins.config;
        icon.SetSkillIcon(config.icon);
        skillName.text = config.name;
        RefreshCount();

        if (ins.count > 0)
        {
            frame.sprite = organSkillFrame;
        }
        else
        {
            frame.sprite = normalSkillFrame;
        }
    }

    public void OnLongPress()
    {
        StageUIView.Instance.ShowSkillInfo(config);
    }

    public void OnLongPressRelease()
    {
        StageUIView.Instance.HideSkillInfo();
    }

    public void OnClick()
    {
        Messenger<ActiveSkillIns>.Invoke(SA.PlayerClickActiveSkill, ins);
    }
    
    public void RefreshCount()
    {
        if (ins.count > 0)
        {
            count.text = ins.count.ToString();
            count.gameObject.SetActive(true);
        }
        else
        {
            count.gameObject.SetActive(false);
        }
    }
}


