using SFramework.StateMachine;
using UnityEngine;

public class FlowerFSM : SFSM
{
    protected override void initFSM()
    {
        AddState(new FlowerEmpty());
    }
}