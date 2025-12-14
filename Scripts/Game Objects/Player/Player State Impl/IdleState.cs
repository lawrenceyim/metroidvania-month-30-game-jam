using Godot;
using InputSystem;

namespace PlayerSystem;

public class IdleState : PlayerState {
    private Player _player;

    public IdleState(Player player) {
        _player = player;
    }

    public override void Input(InputEventDto dto) {
        if (dto is KeyDto keyDto) {
            _player.SetKeyPressed(keyDto.Identifier, keyDto.Pressed);

            if (keyDto.Identifier == "Space" && keyDto.Pressed) {
                _player.SwitchState(PlayerStateId.Jumping);
            }

            if (keyDto.Identifier == "A" && keyDto.Pressed || keyDto.Identifier == "D" && keyDto.Pressed) {
                _player.SwitchState(PlayerStateId.Moving);
            }
        }
    }

    public override void Process(double delta) { }

    public override void PhysicsProcess() {
        if (_player.IsKeyPressed("A") || _player.IsKeyPressed("D")) {
            _player.SwitchState(PlayerStateId.Moving);
        }
    }

    public override void Enter() {
        // GD.Print("Entering Idle State");
        _player.SetAnimation(PlayerAnimationId.Idle);
    }

    public override void Exit() { }
    
    public override void IsGrounded(bool isGrounded) {
        if (!isGrounded) {
            _player.SwitchState(PlayerStateId.Falling);
        }
    }
}