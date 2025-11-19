using InputSystem;

namespace PlayerSystem;

public class IdleState : PlayerState {
    public override void Input(InputEventDto dto) { }

    public override void Process(double delta) { }

    public override void PhysicsProcess() { }

    public override void Enter() { }

    public override void Exit() { }
}