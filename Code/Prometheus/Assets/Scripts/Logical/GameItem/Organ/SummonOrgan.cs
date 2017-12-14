using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonOrgan : OrganBase
{
    public SummonSkillsConfig summonConfig;

    public override void Reactive()
    {
        SummonItem item;

        if (summonConfig.targetType == TargetType.Fort)
        {
            var go = GameObject.Instantiate(Resources.Load("prefab/" + summonConfig.prefab2) as GameObject, StageView.Instance.top);
            go.SetActive(true);
            item = go.GetComponent<SummonItem>();
            item.InitSummonItem(summonConfig, StageCore.Instance.Player);
            item.itemId = GlobalUid.Instance.GetUid();
            var range = summonConfig.carry.ToArray();
            int min = range[0];
            int max = range[1];

            List<Obstacle> list = StageCore.Instance.tagMgr.GetEntity<Obstacle>(ETag.GetETag(ST.OBSTACLE));

            for (int i = list.Count - 1; i >= 0; --i)
            {
                int distance = list[i].standBrick.pathNode.Distance(standBrick.pathNode);

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
        Clean();
    }
}
