using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipSquare : MonoBehaviour {

    [SerializeField]
    Image image;

    public int power = 1;

    public ChipGrid chipGrid = ChipGrid.None;

    public static string[] chipSquareSpriteName = new string[]
    {
        "black_chipsquare",
        "white_chipsquare",
        "blue_chipsquare",
        "red_chipsquare",
        "yellow_chipsquare",
        "green_chipsquare",
    };


    private string ChipGridTypeToSpriteName(ChipGrid type)
    {
        return chipSquareSpriteName[(int)type];
    }

    public void InitChipSquare(ChipGrid chipGrid, int power)
    {
        this.chipGrid = chipGrid;

        image.sprite = ChipBoard.Instance.spriteAtlas.GetSprite(ChipGridTypeToSpriteName(chipGrid));

        if (chipGrid == ChipGrid.Power)
        {
            this.power = power;
        }
    }
}
