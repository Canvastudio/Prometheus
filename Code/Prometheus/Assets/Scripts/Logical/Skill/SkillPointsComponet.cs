using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPointsComponet : MonoBehaviour {

    public List<SkillPoint> pointList = new List<SkillPoint>();

    public void ChangeSkillPointCount(ulong pointId, int count)
    {
        for (int i = 0; i < pointList.Count; ++i)
        {
            if (pointList[i].id == pointId)
            {
                pointList[i].ChangeSkillPoint(count);
                return;
            }
        }

        //如果没有找到，新建
        SkillPoint skillPoint;
        skillPoint = new SkillPoint(pointId);
        skillPoint.ChangeSkillPoint(count);
        pointList.Add(skillPoint);
        
    }
}
