using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoardInstanceNode : MonoBehaviour {

    [SerializeField]
    Image background;
    [SerializeField]
    Image foreground;

    public void Set(int v, Color color)
    {
        if (v > 0)
        {
            background.gameObject.SetActive(true);
            background.color = color;
            foreground.gameObject.SetActive(true);

            if (v == 1)
            {
                foreground.sprite = ChipBoard.Instance.normalSpirte;
            }
            else if (v == 2)
            {
                foreground.sprite = ChipBoard.Instance.positiveSprite;
            }
            else if (v == 3)
            {
                foreground.sprite = ChipBoard.Instance.negativeSprite;
            }
        }
        else
        {
            background.gameObject.SetActive(false);
            foreground.gameObject.SetActive(false);
        }
    }
}
