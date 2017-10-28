using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory {

    Dictionary<ulong, StuffInventory> stuffDic = new Dictionary<ulong, StuffInventory>();

    [SerializeField]
    List<ChipInventory> chipList = new List<ChipInventory>();

    ulong[] ids = new ulong[]{ 1001, 1002, 1003, 1004 };

    public void ChangeStuffCount(ulong id, int count)
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

        Messenger.Invoke(SA.StuffCountChange);
    }

    public ulong StuffToId(Stuff stuff)
    {
        return ids[(int)stuff];
    }

    public void ChangeStuffCount(Stuff stuff, int count)
    {
        var id = StuffToId(stuff);
        ChangeStuffCount(id, count);
    }

    public int GetStuffCount(ulong id)
    {
        StuffInventory stuffInventory;
        if (stuffDic.TryGetValue(id, out stuffInventory))
        {
            return stuffInventory.count;
        }
        else
        {
            return 0;
        }
    }

    public int GetStuffCount(Stuff stuff)
    {
        var id = StuffToId(stuff);
        return GetStuffCount(id);
    }

    public void AddChip(ulong id)
    {
        chipList.Add(new ChipInventory(id));
    }


    public void AddChip(ChipInventory chip)
    {
        chipList.Add(chip);
    }

    public List<ChipInventory> GetUnusedChipList()
    {
        List<ChipInventory> list = new List<ChipInventory>();

        foreach(var chip in chipList)
        {
            if (chip.listItem == null)
            {
                list.Add(chip);
            }
        }

        return list;
    }
}

public class StuffInventory
{
    public StuffConfig config;

    public int count;
}

[System.Serializable]
public class ChipInventory
{
    public ChipConfig config;

    public float power;

    public int power_max;

    public int power_min;

    public int[] model;

    public ChipBoardInstance boardInstance;
    public ChipListItem listItem;

    public ChipInventory(ulong id)
    {
        config = ConfigDataBase.GetConfigDataById<ChipConfig>(id);

        var power_range = config.power.ToArray();

        power_min = power_range[0];
        power_max = power_range[1];

        
        if(id == 100011)
        {
            power = 1;
        }
        else if (id == 100026)
        {
            power = 2;
        }
        else
        {
            power = Random.Range(power_min, power_max);
        }

        int model_count = config.model.Count();

        model = config.model.ToArray(Random.Range(0, model_count - 1));
    }
}
