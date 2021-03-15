using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMState
{
    protected FSM fsm;

    public FSMState()
    {
    }

    public void SetFSM(FSM _fsm)
    {
        fsm = _fsm;
    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnExit()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void LateUpdate()
    {

    }

    protected void ChangeState(int _stateIndex)
    {
        fsm.ChangeState(_stateIndex);
    }
}
