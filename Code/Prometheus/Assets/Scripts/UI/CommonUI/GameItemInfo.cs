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
        itemIcon.SetStageItemIcon(supply.config.prefab);
        itemName.text = config.name;
        itemDescribe.text = config.describe;
    }

    public void ShowTabletInfo(Tablet item)
    {
        var config = item.config;
        itemIcon.SetStageItemIcon(config.prefab);
        itemName.text = config.name;
        itemDescribe.text = config.describe;
    }


    public void ShowTreasureInfo(Treasure item)
    {
        var config = item.config;
        itemIcon.SetStageItemIcon(config.prefab);
        itemName.text = config.name;
        itemDescribe.text = config.describe;
    }
}
