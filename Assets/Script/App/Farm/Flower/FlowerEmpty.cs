using App.Farm;
using SFramework.StateMachine;
using UnityEngine;

public class FlowerEmpty : SFSMState
{
    private FlowerEntity _entity;
    public override void EnterState()
    {
        _entity?.Empty();
    }

    public override void ExitState()
    {
    }

    public override void InitState()
    {
        _entity = (FlowerEntity)Machine.Owner;
    }

    public override void UpdateState()
    {
    }
}