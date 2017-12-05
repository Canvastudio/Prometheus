using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipConnectionItem : MonoBehaviour {

    [SerializeField]
    Image[] points;
    [SerializeField]
    Image[] colors;
    [SerializeField]
    Image[] lights;

    [SerializeField]
    Image horVer;
    [SerializeField]
    Image slop;

    [SerializeField]
    Transform connectRoot;

    public Callback callback;

    List<GameObject> list = new List<GameObject>(10);

    ChipInventory chipInventory;

    public void CleanConnectImage()
    {
        foreach(var go in list)
        {
            Destroy(go);
        }

        list.Clear();
    }

    private void Awake()
    {
        slop.gameObject.SetActive(false);
        horVer.gameObject.SetActive(false);
    }

    public void ShowChipConnection(ChipInventory chip, bool powered)
    {
        chipInventory = chip;

        foreach(var light in lights)
        {
            light.gameObject.SetActive(powered);
        }

        var chipColor = SuperTool.CreateColor(chip.config.color);
        var models = chip.model;
        for(int i = 0; i < models.Length; ++i)
        {
            int v = models[i];
            if (v > 0)
            {
                points[i].gameObject.SetActive(true);
                colors[i].color = chipColor;
                if (v > 1)
                {
                    colors[i].gameObject.SetActive(false);

                    if (v == 2)
                    {
                        points[i].sprite = AtlasCore.Instance.Load("ChipCon").GetSprite("positive_point");
                        lights[i].sprite = AtlasCore.Instance.Load("ChipCon").GetSprite("positive_point_l");
                    }
                    else
                    {
                        points[i].sprite = AtlasCore.Instance.Load("ChipCon").GetSprite("negative_point");
                        lights[i].sprite = AtlasCore.Instance.Load("ChipCon").GetSprite("negative_point_l");
                    }
                }
                else
                {
                    points[i].sprite = AtlasCore.Instance.Load("ChipCon").GetSprite("point_bg");
                    lights[i].sprite = AtlasCore.Instance.Load("ChipCon").GetSprite("point_bg_l");
                    colors[i].gameObject.SetActive(true);
                }

                int c;
                Vector3 next;
                Vector3 cur = points[i].transform.localPosition;


                
                //右链接
                if ((c = GetRight(i))> 0)
                {
                    next = points[c].transform.localPosition;
                    CreateHorVerConnect(cur, next);
                }

                //下链接
                if ((c = GetDown(i)) > 0)
                {
                    next = points[c].transform.localPosition;
                    CreateHorVerConnect(cur, next);
                }
                    
       
                if (GetDown(i) < 0 &&GetLeft(i) < 0 && (c = GetLeftDown(i)) > 0)
                {
                    next = points[c].transform.localPosition;
                    CreateSlopConnect(cur, next);
                }

                if (GetDown(i) < 0 && GetRight(i) < 0 && (c = GetRightDown(i)) > 0)
                {
                    next = points[c].transform.localPosition;
                    CreateSlopConnect(cur, next);
                }
            }
            else
            {
                points[i].gameObject.SetActive(false);
            }

        }
    }

    private void CreateHorVerConnect(Vector3 v1, Vector3 v2)
    {
        var go = GameObject.Instantiate(horVer, connectRoot);
        go.transform.localPosition = (v1 + v2) / 2f;
        if (v1.y != v2.y)
        {
            go.transform.localEulerAngles = new Vector3(0, 0, 90f);
        }
        list.Add(go.gameObject);
        go.gameObject.SetActive(true);
    }

    private void CreateSlopConnect(Vector3 v1, Vector3 v2)
    {
        var go = GameObject.Instantiate(slop, connectRoot);
        go.transform.localPosition = (v1 + v2) / 2f;
        if (v1.x > v2.x)
        {
            go.transform.localEulerAngles = new Vector3(0, 0, 45);
        }
        else
        {
            go.transform.localEulerAngles = new Vector3(0, 0, -45);
        }
        list.Add(go.gameObject);
        go.gameObject.SetActive(true);
    }

    public int GetRight(int index)
    {
        int v = index % 3;

        if (v < 2)
        {

            index += 1;
            if (chipInventory.model[index] > 0)
            {
                return index;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }
    }

    public int GetDown(int index)
    {
        int v = index / 3;
        if (v < 2)
        {
            index = index + 3;
            if (chipInventory.model[index] > 0)
            {
                return index;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }
    }

    public int GetLeft(int index)
    {
        int v = index % 3;
        if (v > 0)
        {
            index -= 1;
            if (chipInventory.model[index] > 0)
            {
                return index;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }
    }

    public int GetLeftDown(int index)
    {
        int v1 = index % 3;
        int v2 = index / 3;

        if (v1 > 0 && v2 < 2)
        {
            index += 2;
            if (chipInventory.model[index] > 0)
            {
                return index;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }
    }

    public int GetRightDown(int index)
    {
        int v1 = index % 3;
        int v2 = index / 3;

        if (v1 < 2 && v2 < 2)
        {
            index += 4;
            if (chipInventory.model[index] > 0)
            {
                return index;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }
    }
}
