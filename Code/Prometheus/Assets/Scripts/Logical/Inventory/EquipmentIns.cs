using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentIns {

    bool equip = false;
    bool identify = false;

    public EquipConfig config;
    int quality;

    public GameProperty mainProperty;
    public float mainPropertyValue;

    public List<GameProperty> optionalPropertys;
    public List<float> optionalPropertyValues;

    public List<SkillPointsConfig> skillPoints;
    public List<int> skillPointsCount;

    public EquipmentIns(EquipConfig e, int q)
    {
        config = e;
        quality = q;

        mainProperty = config.mainPro;
        int min = config.mainProValue[0];
        int max = config.mainProValue[1];
        int value = Random.Range(min, max + 1);
        float mul = config.mainProMul[q];
        mainPropertyValue = value * mul;

        int optionalCount = config.optionalProNum[q];
        optionalPropertys = new List<GameProperty>(optionalCount);
        optionalPropertyValues = new List<float>(optionalCount);
        Draft draft = new Draft(optionalCount);
        for(int i = 0; i < optionalCount; ++i)
        {
            int index = draft.Ran();
            GameProperty p = config.optionalPro[index];
            min = config.optionalProValue[index, 0];
            max = config.optionalProValue[index, 1];
            value = Random.Range(min, max + 1);
            optionalPropertys.Add(p);
            optionalPropertyValues.Add(value);
        }

        skillPoints = config.skillPoint.ToList();
        skillPointsCount = config.skillPointCount.ToList();
    }

    public void Equip()
    {
        equip = true;

        var player = StageCore.Instance.Player;

        player.Property.AddFloatProperty(mainProperty, mainPropertyValue);

        if (identify)
        {
            for(int i = 0; i < optionalPropertys.Count; ++i)
            {
                player.Property.AddFloatProperty(optionalPropertys[i], optionalPropertyValues[i]);
            }

            for(int i = 0; i < skillPoints.Count; ++i)
            {
                player.skillPointsComponet.ChangeSkillPointCount(skillPoints[i], skillPointsCount[i]);
            }
        }
    }

    public void Unequip()
    {
        equip = false;

        var player = StageCore.Instance.Player;
        player.Property.AddFloatProperty(mainProperty, -mainPropertyValue);
        if (identify)
        {
            for (int i = 0; i < optionalPropertys.Count; ++i)
            {
                player.Property.AddFloatProperty(optionalPropertys[i], -optionalPropertyValues[i]);
            }

            for (int i = 0; i < skillPoints.Count; ++i)
            {
                player.skillPointsComponet.ChangeSkillPointCount(skillPoints[i], -skillPointsCount[i]);
            }
        }
    }

    public void Identify()
    {
        identify = true;

        if (equip)
        {
            var player = StageCore.Instance.Player;
            for (int i = 0; i < optionalPropertys.Count; ++i)
            {
                player.Property.AddFloatProperty(optionalPropertys[i], optionalPropertyValues[i]);
            }

            for (int i = 0; i < skillPoints.Count; ++i)
            {
                player.skillPointsComponet.ChangeSkillPointCount(skillPoints[i], skillPointsCount[i]);
            }
        }
    }
}
