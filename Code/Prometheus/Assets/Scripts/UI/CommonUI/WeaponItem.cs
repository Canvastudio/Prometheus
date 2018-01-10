using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
public class WeaponItem : MonoBehaviour
{

    static StringBuilder sb = new StringBuilder(10);

    [SerializeField]
    Image weaponIcon;
    [SerializeField]
    Text weaponName;
    [SerializeField]
    Text mainProperty;
    [SerializeField]
    Text secondProperty;

    [Space(10)]
    [SerializeField]
    Button equipBtn;
    [SerializeField]
    Button identifyBtn;

    EquipmentIns equipment;

    private void Awake()
    {
        HudEvent.Get(equipBtn).onClick = OnEquip;
        HudEvent.Get(identifyBtn).onClick = OnIdentify;
    }

    private void OnEquip()
    {
        equipment.Equip();
    }

    private void OnIdentify()
    {
        equipment.Identify();
    }

    public void ShowWeaponInfo(EquipmentIns ins)
    {
  
        //weapicon
        weaponName.text = ins.config.eqpName.ToString();

        sb.Clean();
        string propertyName = ins.mainProperty.ToString().GetLocalStr();
        sb.Append(propertyName);
        sb.Append(" +");
        sb.Append(ins.mainPropertyValue.ToString());
        mainProperty.text = sb.ToString();

        sb.Clean();
        for (int i = 0; i < ins.optionalPropertys.Count; ++i)
        {
            propertyName = ins.optionalPropertys[i].ToString().GetLocalStr();
            sb.Append(propertyName);
            sb.Append(" +");
            sb.Append(ins.optionalPropertyValues[i].ToString());
            mainProperty.text = sb.ToString();
            sb.Append(",");
        }
        sb.Remove(sb.Length - 1, 1);
        secondProperty.text = sb.ToString();
    }
}