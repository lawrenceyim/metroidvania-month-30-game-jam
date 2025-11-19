using System.Collections.Generic;
using Godot;
using InputSystem;
using ServiceSystem;
using StateMachineSystem;

namespace PlayerSystem;

public partial class Player : CharacterBody2D, ITick, IInputState {
    private readonly StateMachine<PlayerStateId> _playerStateMachine = new();
    private readonly Dictionary<string, bool> _keyPressed = new();
    
    // refactor these to the active scene rather than the player character itself
    private InputStateMachine _inputStateMachine; 
    private GameClock _gameClock;

    public override void _Ready() {
        ServiceLocator serviceLocator = GetNode<ServiceLocator>(ServiceLocator.AutoloadPath);
        _inputStateMachine = serviceLocator.GetService<InputStateMachine>(ServiceName.InputStateMachine);
        _inputStateMachine.SetState(this);
        _gameClock = serviceLocator.GetService<GameClock>(ServiceName.GameClock);
        _gameClock.AddActiveScene(this, GetInstanceId());
        _InitializeStateMachine();
    }

    public override void _ExitTree() {
        _inputStateMachine.SetState(null);
        _gameClock.RemoveActiveScene(GetInstanceId());
    }

    public void PhysicsTick() {
        _playerStateMachine.PhysicsProcess();
    }

    private void _InitializeStateMachine() {
        _playerStateMachine.AddState(PlayerStateId.Idle, new IdleState());
        _playerStateMachine.AddState(PlayerStateId.Running, new RunningState(this));

        _playerStateMachine.SwitchState(PlayerStateId.Running);
    }

    public void SetKeyPressed(string key, bool keyPressed) {
        _keyPressed[key] = keyPressed;
    }

    public bool IsKeyPressed(string key) {
        return _keyPressed.GetValueOrDefault(key, false);
    }

    public void ProcessInput(InputEventDto eventDto) {
        _playerStateMachine?.Input(eventDto);
    }
}