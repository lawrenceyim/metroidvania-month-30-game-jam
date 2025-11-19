using Godot;
using ServiceSystem;
using StateMachineSystem;

namespace PlayerSystem;

public partial class Player : CharacterBody2D, ITick {
    private StateMachine<PlayerStateId> _stateMachine = new();

    public override void _Ready() {
        ServiceLocator serviceLocator = GetNode<ServiceLocator>(ServiceLocator.AutoloadPath);
    }

    public void PhysicsTick() {
        _stateMachine.PhysicsProcess();
    }

    private void _InitializeStateMachine() {
        _stateMachine.AddState(PlayerStateId.Idle, new IdleState());

        IdleState idleState = new IdleState();
        idleState.Enter();
    }

    private class IdleState : IState {
        public void Process(double delta) { }
        public void PhysicsProcess() { }

        public void Enter() {
            // Start playing idle animation
        }

        public void Exit() {
            // Stop playing idle animation
        }
    }
}