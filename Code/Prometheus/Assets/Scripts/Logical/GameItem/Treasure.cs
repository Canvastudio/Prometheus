using UnityEngine;

public class Treasure : GameItemBase, IReactive {

    public BoxConfig config;
    public string pool_name;
    public int distance;

    BoxDropConfig cur_drop;

    SuperArrayValue<string> drop_desc;

    public void Reactive()
    {
        standBrick.brickType = BrickType.EMPTY;
        int wi = SuperTool.CreateWeightSection(drop_desc.ToList(1)).RanPoint();
        var nums = drop_desc.ToArray(0);
        int num = int.Parse(nums[wi]);//次数
        ulong id;
        switch (drop_desc.ToArray(3)[wi])
        {
            case "Stuff":
                id = ulong.Parse(drop_desc.ToArray(2)[wi]);
                StageCore.Instance.Player.inventory.ChangeStuffCount(id, num);
                break;
            case "Chip":
                id = ulong.Parse(drop_desc.ToArray(2)[wi]);
                StageCore.Instance.Player.inventory.AddChip(id);
                break;
            case "Equip":
                string key = drop_desc.ToArray(2)[wi];
                EquipConfig econfig = EquipConfig.GetConfigDataByKey<EquipConfig>(key);
                StageCore.Instance.Player.inventory.AddEquipment(econfig, config.level);
                break;
        }

        Debug.Log("TODO: 开宝箱获得材料: " + drop_desc.ToArray(3)[wi] + "id: " + drop_desc.ToArray(2)[wi] + " num: " + num);

        StageUIView.Instance.IniMat();

        //ArtSkill.ShowDrop(config.pickup, transform.position);

        Recycle();
    }

    public void Init()
    {
        var drop_Configs = ConfigDataBase.GetConfigDataList<BoxDropConfig>();
        
        for (int i = 0; i < drop_Configs.Count; ++i)
        {
            if (drop_Configs[i].distance >= distance)
            {
                cur_drop = drop_Configs[i];
                break;
            }
        }

        switch (config.level)
        {
            case 1:
                drop_desc = cur_drop.level1;
                break;
            case 2:
                drop_desc = cur_drop.level2;
                break;
            case 3:
                drop_desc = cur_drop.level3;
                break;
            case 4:
                drop_desc = cur_drop.level4;
                break;
        }
    }

    public override void Recycle()
    {
        base.Recycle();

        ObjPool<Treasure>.Instance.RecycleObj(pool_name, itemId);
    }
}
