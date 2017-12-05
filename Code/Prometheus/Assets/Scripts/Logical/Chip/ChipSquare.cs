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
    Power,
}

public class ChipSquare : MonoBehaviour {

    [SerializeField]
    Image image;

    [SerializeField]
    private ChipGrid _chipGrid = ChipGrid.None;

    public ChipBoardInstance boardInstance;
    public BoardSupplyInstance supplyInstance;

    public int index;
    [SerializeField]
    private ChipSquareState _state = ChipSquareState.Free;

    public int row;
    public int col;

    private bool _isActive;
    public bool isActive
    {
        get
        {
            return _isActive;
        }
        set
        {
            gameObject.SetActive(value);
            _isActive = value;
        }
    }


    private int _temp;

    public static string[] chipSquareSpriteName = new string[]
    {
        "chip_block",
        "white_chipsquare",
        "blue_area",
        "red_area",
        "yellow_area",
        "white_chipsquare",//"green_chipsquare",
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
            //if (value != ChipSquareState.Free)
            //{
            //    image.color = Color.red;
            //}
            //else 
            //{
            //    image.color = Color.white;
            //}

            _state = value;
        }
    }

    private string ChipGridTypeToSpriteName(ChipGrid type)
    {
        return chipSquareSpriteName[(int)type];
    }

    public void InitChipSquare(ChipGrid chipGrid)
    {
        this.chipGrid = chipGrid;

        if (chipGrid != ChipGrid.Normal && chipGrid != ChipGrid.Power)
        {
            image.enabled = true;
            image.sprite = AtlasCore.Instance.Load("Chip").GetSprite(ChipGridTypeToSpriteName(chipGrid));
        }
        else
        {
            image.enabled = false;
        }
    }

    private void OnCheckPowerState()
    {

    }
}
