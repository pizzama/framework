using SFramework.Actor;
using SFramework.Game.App;
using UnityEngine;
namespace SFramework.Actor.Ability
{
    public class SFOrientation2DAbility : SFAbility
    {
        public enum FacingModes { None, MovementDirection, WeaponDirection, Both }
        public enum FacingBases { WeaponAngle, MousePositionX, SceneReticlePositionX }
        public float AbsoluteThresholdMovement = 0.5f;/// the threshold at which movement is considered
        public SFActor.SFActorFacingDirections CurrentFacingDirection = SFActor.SFActorFacingDirections.Right;
        public override void InitAbility()
        {
            base.InitAbility();
        }

        public override void EnterAbility()
        {

        }

        public override void UpdateAbility()
        {
            base.UpdateAbility();
            if (conditionMachine.CurrentState != SFAbilityStates.AbilityConditions.Normal)
            {
                return;
            }

            if (!AbilityAuthorized)
            {
                return;
            }
        }

        public override void ExitAbility()
        {

        }

        protected virtual void determineFacingDirection()
        {
            if (actControl.CurrentDirection == Vector3.zero)
            {
                ApplyCurrentDirection();
            }
        }

        protected virtual void ApplyCurrentDirection()
        {
            switch (CurrentFacingDirection)
            {
                case SFActor.SFActorFacingDirections.Right:
                    actControl.CurrentDirection = Vector3.right;
                    break;
                case SFActor.SFActorFacingDirections.Left:
                    actControl.CurrentDirection = Vector3.left;
                    break;
                case SFActor.SFActorFacingDirections.Up:
                    actControl.CurrentDirection = Vector3.up;
                    break;
                case SFActor.SFActorFacingDirections.Down:
                    actControl.CurrentDirection = Vector3.down;
                    break;
            }
        }

    }
}