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
            {
                config = ConfigDataBase.GetConfigDataById<StuffConfig>(id),
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

    public void AddChip(ulong id, int cost = -1)
    {
        chipList.Add(new ChipInventory(id, cost));
    }


    public void AddChip(ChipInventory chip)
    {
        chipList.Add(chip);
    }

    public List<ChipInventory> GetChipList()
    {
        return chipList;
    }

    public List<ChipInventory> GetUnusedChipList()
    {
        List<ChipInventory> list = new List<ChipInventory>();

        foreach(var chip in chipList)
        {
            if (chip.boardInstance == null)
            {
                list.Add(chip);
            }
        }

        return list;
    }

    public void RemoveChip(ChipInventory chip)
    {
        for(int i = 0; i < chipList.Count; ++i)
        {
            if (chipList[i].uid == chip.uid)
            {
                if (chipList[i].boardInstance != null)
                {
                    //移除芯片盘上的芯片
                    ChipView.Instance.RemoveChipInstance(chipList[i].boardInstance);

                    //因为芯片盘只有每次手动关闭才会去刷新技能，所以这里得手动刷新下技能
                    var sp = chip.config.skillPoint;
                    int c = sp.Count();
                    {
                        for (int m = 0; m < c; ++m)
                        {
                            ulong skill_id = (ulong)sp[m, 0];
                            int count = sp[m, 1];
                            StageCore.Instance.Player.skillPointsComponet.ChangeSkillPointCount(skill_id, count * -1);
                        }
                    }

                    StageCore.Instance.Player.RefreshSkillPointStateToSkill();
                }
            }
        }
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
    public float cost;
    public int power_max;
    public int power_min;
    public int[] model;
    public ChipBoardInstance boardInstance;
    public ChipListItem listItem;
    public int uid = 0;
    private static int _uid = 0;

    public ChipInventory(ulong id, int cost = -1)
    {
        config = ConfigDataBase.GetConfigDataById<ChipConfig>(id);

        var power_range = config.power.ToArray();

        power_min = power_range[0];
        power_max = power_range[1];

        if (cost < 0)
        {
            this.cost = Random.Range(power_min, power_max);
        }
        else
        {
            this.cost = cost;
        }

        int model_count = config.model.Count();

        model = config.model.ToArray(Random.Range(0, model_count - 1));

        uid = _uid++;
    }
}
