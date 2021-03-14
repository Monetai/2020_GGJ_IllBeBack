using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    public IFSMController controller = null;
    private readonly Dictionary<int, FSMState> states = new Dictionary<int, FSMState>();
    private FSMState currentState = null;
    private FSMState requestedState = null;

    public FSM(IFSMController _controller)
    {
        controller = _controller;
    }

    public T GetController<T>()
    {
        return (T)controller;
    }

    public void AddState<T>(int _index) where T : FSMState, new() 
    {
        T stateToAdd = new T();
        stateToAdd.SetFSM(this);
        states.Add(_index, stateToAdd);
    }

    public FSMState AddAndGetState<T>(int _index) where T : FSMState, new()
    {
        AddState<T>(_index);
        return GetState(_index);
    }

    public FSMState GetState(int _index)
    {
        states.TryGetValue(_index, out FSMState foundState);
        return foundState;
    }

    public bool IsInState<T>() where T : FSMState
    {
        return typeof(T) == currentState.GetType();
    }

    public void Start(int _baseStateIndex)
    {
        ChangeState(_baseStateIndex, true);
    }

    public void ChangeState(int _newState, bool immediate = false)
    {
        states.TryGetValue(_newState, out FSMState startState);
        if (startState != null)
        {
            requestedState = startState;
            if( immediate ) { ProcessChangeState(); }
        }
        else
        {
            Debug.LogError($"Can't find state with index : {_newState}.");
        }
    }

    private void ProcessChangeState()
    {
        if(requestedState != null)
        {
            currentState?.OnExit();
            currentState = requestedState;
            requestedState = null;
            currentState.OnEnter();
        }
        else
        {
            Debug.LogError($"Can't change to a null state.");
        }
    }

    public virtual void Update()
    {
        currentState?.Update();
    }

    public virtual void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public virtual void LateUpdate()
    {
        currentState?.LateUpdate();

        if(requestedState != null)
        {
            ProcessChangeState();
        }
    }

    public int GetCurrentStateIndex()
    {
        foreach (KeyValuePair<int, FSMState> state in states)
        {
            if(state.Value == currentState)
            {
                return state.Key;
            }
        }

        return 1;
    }
}
