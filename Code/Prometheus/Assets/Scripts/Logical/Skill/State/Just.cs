
public class Just : StateEffectIns
{
    public Just(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        stateType = StateEffectType.JustPropertyChange;
    }

    protected override void Apply(object param)
    {

    }
}
