using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipBoardInstance : MonoBehaviour {

    public ChipGrid chipGrid;

    public ChipInventory chipInventory;

    [SerializeField]
    List<Image> itemsList;
    [SerializeField]
    GameObject positive;
    [SerializeField]
    GameObject negative;

    public Vector3 lastLocalPos;

    public void Init(ChipListItem chipItem)
    {
        chipInventory = chipItem.chipInventory;
        chipItem.chipInventory.boardInstance = this;

        for (int i = 0; i < itemsList.Count; ++i)
        {
            int v = chipInventory.model[i];

            if (v > 0)
            {
                itemsList[i].gameObject.SetActive(true);

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
                itemsList[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnLongPress()
    {

    }

}
