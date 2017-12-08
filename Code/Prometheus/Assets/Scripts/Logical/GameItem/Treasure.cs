using UnityEngine;

public class Treasure : GameItemBase, IReactive {

    public BoxConfig config;
    public string pool_name;
    public int distance;

    BoxDropConfig cur_drop;

    SuperArrayValue<string> drop_desc;

    public void Reactive()
    {
        Debug.Log("TODO: 开宝箱获得材料！");

        standBrick.brickType = BrickType.EMPTY;

        var nums = drop_desc.ToArray(0);
        int i = Random.Range(0, nums.Length);
        int num = int.Parse(nums[i]);//次数
        int wi = SuperTool.CreateWeightSection(drop_desc.ToList(1)).RanPoint();
        ulong id = ulong.Parse(drop_desc.ToArray(2)[wi]);

        switch (drop_desc.ToArray(3)[wi])
        {
            case "Stuff":
                StageCore.Instance.Player.inventory.ChangeStuffCount(id, num);
                break;
            case "Chip":
                StageCore.Instance.Player.inventory.AddChip(id);
                break;
        }

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
