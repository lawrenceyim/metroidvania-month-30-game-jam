using InputSystem;
using StateMachineSystem;

public abstract class PlayerState : IState {
    public abstract void Input(InputEventDto dto);
    
    public abstract void Process(double delta);

    public abstract void PhysicsProcess();

    public abstract void Enter();

    public abstract void Exit();
    
    public abstract void IsGrounded(bool isGrounded);
}
