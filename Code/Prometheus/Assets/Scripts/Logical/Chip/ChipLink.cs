using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipLink : MonoBehaviour {

    [SerializeField]
    List<Image> itemsList;
    [SerializeField]
    Transform positive;
    [SerializeField]
    Transform negative;

    public void Set(ChipInventory chipInventory)
    {
        var color = SuperTool.CreateColor(chipInventory.config.color);

        for (int i = 0; i < itemsList.Count; ++i)
        {
            int v = chipInventory.model[i];

            if (v > 0)
            {
                itemsList[i].color = color;

                if (v == 2)
                {
                    positive.position = itemsList[i].transform.position;
                }
                else if (v == 3)
                {
                    negative.position = itemsList[i].transform.position;
                }
            }
            else
            {
                itemsList[i].color = Color.green;
            }
        }
    }
}
