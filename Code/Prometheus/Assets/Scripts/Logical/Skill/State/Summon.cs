using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : StateEffectIns
{
    ulong sid;
    int time;
    SummonItem item;
    SummonSkillsConfig summonConfig;

    public Summon(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        sid = config.stateArgs[index].u[0];
        time = (int)config.stateArgs[index].f[0];

        summonConfig = ConfigDataBase.GetConfigDataById<SummonSkillsConfig>(sid);
    }

    public override void Active()
    {
        base.Active();

        if (item != null)
        {

        }
        else
        {
            if (summonConfig.targetType == TargetType.Fort)
            {
                var go = GameObject.Instantiate(Resources.Load("prefab/" + summonConfig.prefab2) as GameObject, StageView.Instance.top);
                go.SetActive(true);
                item = go.GetComponent<SummonItem>();
                item.InitSummonItem(summonConfig, owner);
                item.itemId = GlobalUid.Instance.GetUid();
                var range = summonConfig.carry.ToArray();
                int min = range[0];
                int max = range[1];

                List<Obstacle> list = StageCore.Instance.tagMgr.GetEntity<Obstacle>(ETag.GetETag(ST.OBSTACLE));

                for (int i = list.Count - 1; i >= 0; --i)
                {
                    int distance = list[i].standBrick.pathNode.Distance(owner.standBrick.pathNode);

                    if (distance > max || distance < min)
                    {
                        list.RemoveAt(i);
                    }
                }

                if (list.Count > 0)
                {
                    int index = Random.Range(0, list.Count);

                    Obstacle obstacle = list[index];
                    item.transform.position = obstacle.transform.position;
                    item.standBrick = obstacle.standBrick;
                }
            }
            else
            {
                var go = GameObject.Instantiate(Resources.Load("prefab/" + summonConfig.prefab2) as GameObject, StageCore.Instance.Player.transform);
                go.SetActive(true);
                item = go.GetComponent<SummonItem>();
                item.InitSummonItem(summonConfig, owner);

                item.transform.position = owner.transform.position;
                item.standBrick = StageCore.Instance.Player.standBrick;
            }
        }

        item.Active();
    }

    public override void Deactive()
    {
        base.Deactive();

        item.Deactive();
    }

    public override void Remove()
    {
        base.Remove();

        item.Deactive();

        GameObject.Destroy(item.gameObject);
    }
}
