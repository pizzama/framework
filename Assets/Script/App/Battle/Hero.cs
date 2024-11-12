using SFramework.Actor;

namespace Game.Character
{
    public class Hero : SAnimatorFSMActor3D
    {
        protected void Start()
        {
            AddFSMState(new HeroIdleState());
            AddFSMState(new HeroMoveState());
            GetFSM().ChangeState<HeroIdleState>();
        }
    }
}
