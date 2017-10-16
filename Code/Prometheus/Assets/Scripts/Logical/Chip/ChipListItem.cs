using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipListItem : MonoBehaviour {

    [SerializeField]
    List<Image> itemsList;
    [SerializeField]
    GameObject btn;
    [SerializeField]
    GameObject positive;
    [SerializeField]
    GameObject negative;

    private Color color;

    public IEnumerator InitItem(ChipInventory chipInventory)
    {
        yield return GameExtend.waitForEndOfFrame;

        HudEvent.Get(btn).onLongPress = OnLongPress;

        color = SuperTool.CreateColor(chipInventory.config.color);

        for (int i = 0; i < itemsList.Count; ++i)
        {
            int v = chipInventory.model[i];

            if (v > 0)
            {
                itemsList[i].color = color;

                if (v == 2)
                {
                    positive.transform.position = itemsList[i].transform.position;
                }
                else if (v == 3)
                {
                    negative.transform.position = itemsList[i].transform.position;
                }
            }
            else
            {
                itemsList[i].color = Color.white;
            }
        }
    }

    private void OnLongPress()
    {

    }
}
