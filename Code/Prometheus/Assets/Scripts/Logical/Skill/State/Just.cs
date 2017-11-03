
public class Just : StateIns
{
    public Just(LiveItem owner, StateConfig config, int index, bool passive) : base(owner, config, index, passive)
    {
        stateType = StateEffectType.JustPropertyChange;
    }

    protected override void Apply(object param)
    {

    }
}
