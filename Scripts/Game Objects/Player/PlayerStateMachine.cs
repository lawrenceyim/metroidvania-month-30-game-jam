using StateMachineSystem;

namespace PlayerSystem;

public class PlayerStateMachine : StateMachine<PlayerStateId> {
    public void IsGrounded(bool isGrounded) {
        (currentState as PlayerState)?.IsGrounded(isGrounded);
    }
}