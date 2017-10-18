using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ChipSquareState
{
    Free,
    Use,
    Positive,
    Negative,
}

public class ChipSquare : MonoBehaviour {

    [SerializeField]
    Image image;

    [SerializeField]
    private int _power;
    public int power
    {
        get
        {
            if (_chipGrid == ChipGrid.Power)
            {
                return _power;
            }
            else
            {
                return boardInstance.powerSquare.leftPower;
            }
        }
        set
        {
            _power = value;
        }
    }

    public int leftPower = int.MinValue;
    public int assigned_power = int.MinValue;

    [SerializeField]
    private ChipGrid _chipGrid = ChipGrid.None;

    public ChipBoardInstance boardInstance;
    public int index;
    [SerializeField]
    private ChipSquareState _state = ChipSquareState.Free;

    public int row;
    public int col;

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
            else 
            {
                image.color = Color.white;
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
