using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameItemInfo : MonoBehaviour {

    [SerializeField]
    Image itemIcon;
    [SerializeField]
    Text itemName;
    [SerializeField]
    Text itemDescribe;

    public void ShowSupplyInfo(Supply supply)
    {
        var config = supply.config;
        itemName.text = config.name;
        itemDescribe.text = config.describe;
    }
}
