using UnityEngine;

public class TimeEffect : Property
{
    [UnityEngine.SerializeField]
    float interval;
    [UnityEngine.SerializeField]
    float t;

    public TimeEffect(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        if (stateConfig.stateArgs[index].f != null)
        {
            interval = stateConfig.stateArgs[index].f[0];
        }
        else
        {
            Debug.Log("TimeEffect: " + config.name + " f is null!");
            interval = 1;
        }
    }

    public override void Active()
    {
        active = true;
    }

    public override void OnTimeChange(float time)
    {
        base.OnTimeChange(time);

        if (active)
        {
            t += time;

            if (t > interval)
            {
                ApplyChange();

                t -= interval;
            }

            totalTime += time;
        }
    }
}
