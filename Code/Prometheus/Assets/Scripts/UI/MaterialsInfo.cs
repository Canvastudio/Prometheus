using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialsInfo : MonoBehaviour {

    [SerializeField]
    Text[] owned;
    [SerializeField]
    Text[] cost;

    public void RefreshOwned()
    {
        var inventory = StageCore.Instance.Player.inventory;

        for (int i = 0; i < 3; ++i)
        {

            Stuff stuff = (Stuff)i;
            owned[i].text = inventory.GetStuffCount(stuff).ToString();
        }
    }

    public void SetCost(Stuff stuff, int _cost)
    {
        cost[((int)stuff)].text = "-" + _cost.ToString();
        cost[((int)stuff)].gameObject.SetActive(true);
    }

    public void CleanCost()
    {
        for (int i = 0; i < 3; ++i)
        {
            cost[i].gameObject.SetActive(false);
        }
    }
}
