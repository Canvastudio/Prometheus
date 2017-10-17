using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ChipSquareState
{
    Free,
    Use,
    Passitive,
    Negative,
}

public class ChipSquare : MonoBehaviour {

    [SerializeField]
    Image image;

    public int power = 1;

    private ChipGrid _chipGrid = ChipGrid.None;

    public ChipBoardInstance boardInstance;
    public int index;
    private ChipSquareState _state = ChipSquareState.Free;

    public static string[] chipSquareSpriteName = new string[]
    {
        "black_chipsquare",
        "white_chipsquare",
        "blue_chipsquare",
        "red_chipsquare",
        "yellow_chipsquare",
        "green_chipsquare",
    };

    public ChipGrid chipGrid
    {
        get
        {
            return _chipGrid;
        }

        set
        {
            if (value == ChipGrid.None)
            {
                image.color = Color.red;
            }

            _chipGrid = value;
        }
    }

    public ChipSquareState state
    {
        get
        {
            return _state;
        }

        set
        {
            if (value != ChipSquareState.Free)
            {
                image.color = Color.red;
            }

            _state = value;
        }
    }

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

    public void Empoly(ChipBoardInstance boardInstance, int index)
    {
        this.boardInstance = boardInstance;
        this.index = index;
    }
}
