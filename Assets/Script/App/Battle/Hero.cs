using SFramework.Actor;

namespace Game.Character
{
    public class Hero : SFActor3D
    {
        protected override void Start()
        {
            AddFSMState(new HeroIdleState());
            AddFSMState(new HeroMoveState());
            GetFSM().ChangeState<HeroIdleState>();
        }
    }
}
