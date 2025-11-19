#nullable enable
using System;
using System.Collections.Generic;

namespace StateMachineSystem;

public class StateMachine<T> where T : Enum {
    private readonly Dictionary<T, IState> _states = new();
    private IState? _currentState;

    public void AddState(T key, IState state) {
        _states[key] = state;
    }

    public void RemoveState(T key) {
        _states.Remove(key);
    }

    public void Process(double delta) {
        _currentState?.Process(delta);
    }

    public void PhysicsProcess() {
        _currentState?.PhysicsProcess();
    }

    public void SwitchState(T key) {
        IState? state = _states.GetValueOrDefault(key);
        _currentState?.Exit();
        _currentState = state;
        _currentState?.Enter();
    }
}