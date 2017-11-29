using UnityEngine;

public class TimeEffect : Property
{
    [UnityEngine.SerializeField]
    float interval;
    [UnityEngine.SerializeField]
    float t;
    [SerializeField]
    float total;

    public TimeEffect(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        interval = stateConfig.stateArgs[index].f[0];
    }

    public override void Active()
    {
    }

    public override void OnTimeChange(float time)
    {
        base.OnTimeChange(time);

        if (active)
        {
            t += time;

            if (t > interval)
            {
                ResetChange();
                ApplyChange();

                t -= interval;
                total += interval;
            }
        }
    }
}
