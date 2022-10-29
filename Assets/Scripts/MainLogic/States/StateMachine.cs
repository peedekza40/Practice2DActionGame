using System;
using Character.Combat.States;
using Constants;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class StateMachine : MonoBehaviour
{
    public StateId Id;

    private State mainStateType;

    private State currentState;
    private State nextState;

    // Update is called once per frame
    void Update()
    {
        if (nextState != null)
        {
            SetState(nextState);
        }

        if (currentState != null)
            currentState.OnUpdate();
    }

    private void SetState(State _newState)
    {
        nextState = null;
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = _newState;
        currentState.OnEnter(this);
    }

    public void SetNextState(State _newState)
    {
        if (_newState != null)
        {
            nextState = _newState;
        }
    }

    private void LateUpdate()
    {
        if (currentState != null)
            currentState.OnLateUpdate();
    }

    private void FixedUpdate()
    {
        if (currentState != null)
            currentState.OnFixedUpdate();
    }

    public void SetNextStateToMain()
    {
        nextState = mainStateType;
    }

    private void Awake()
    {
        SetNextStateToMain();
    }


    private void OnValidate()
    {
        if (mainStateType == null)
        {
            switch (Id)
            {
                case StateId.Combat : 
                    mainStateType = new IdleCombatState();
                    break;
            }
        }
    }

    public bool IsCurrentState(Type stateType)
    {
        return currentState?.GetType() == stateType;
    }

    public float GetCurrentStateTime()
    {
        return currentState?.time ?? 0f;
    }
}