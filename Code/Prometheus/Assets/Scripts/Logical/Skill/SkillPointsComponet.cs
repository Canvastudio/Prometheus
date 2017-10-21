using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPointsComponet : MonoBehaviour {

    public LiveItem owner;
    public Dictionary<ulong, SkillPoint> pointDic = new Dictionary<ulong, SkillPoint>();

    public void ChangeSkillPointCount(ulong pointId, int count)
    {
        SkillPoint skillPoint;
        if (pointDic.TryGetValue(pointId, out skillPoint))
        {
            skillPoint.ChangeSkillPoint(count);
        }
        else
        {
            skillPoint = new SkillPoint(pointId);
            skillPoint.ChangeSkillPoint(count);
            pointDic.Add(pointId, skillPoint);
        }
    }
}
