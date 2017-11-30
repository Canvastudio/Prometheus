
public class Just : Property
{
    public Just(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        stateType = StateEffectType.JustPropertyChange;
    }

    public override void Active()
    {
    }

    public override void Deactive()
    {
    }
}
