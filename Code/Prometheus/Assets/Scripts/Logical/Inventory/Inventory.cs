using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {

    Dictionary<ulong, StuffInventory> stuffDic = new Dictionary<ulong, StuffInventory>();

    Dictionary<ulong, ChipInventory> chipDic = new Dictionary<ulong, ChipInventory>();

    public void AddStuff(ulong id, int count)
    {
        Debug.Log("获得stuff: " + id.ToString() + " " + count.ToString());
        StuffInventory stuffInventory;

        if (stuffDic.TryGetValue(id, out stuffInventory))
        {
            stuffInventory.count += count;
        }
        else
        {
            stuffDic.Add(id, new StuffInventory()
            { config = ConfigDataBase.GetConfigDataById<StuffConfig>(id),
                count = count
            });
        }
    }

    public void AddChip(ulong id, int count)
    {
        Debug.Log("获得chip: " + id.ToString() + " " + count.ToString());
        ChipInventory chipInventory;

        if (chipDic.TryGetValue(id, out chipInventory))
        {
            chipInventory.count += count;
        }
        else
        {
            chipDic.Add(id, new ChipInventory()
            {
                config = ConfigDataBase.GetConfigDataById<ChipConfig>(id),
                count = count
            });
        }
    }
}

public class StuffInventory
{
    public StuffConfig config;

    public int count;
}

public class ChipInventory
{
    public ChipConfig config;

    public int count;
}
