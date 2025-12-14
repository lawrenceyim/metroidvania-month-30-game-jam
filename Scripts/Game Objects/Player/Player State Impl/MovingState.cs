using Godot;
using InputSystem;

namespace PlayerSystem;

public class MovingState : PlayerState {
    private readonly Player _player;

    public MovingState(Player player) {
        _player = player;
    }

    public override void Input(InputEventDto dto) {
        if (dto is KeyDto keyDto) {
            _player.SetKeyPressed(keyDto.Identifier, keyDto.Pressed);

            if (keyDto.Identifier == "Space" && keyDto.Pressed) {
                _player.SwitchState(PlayerStateId.Jumping);
            }
        }
    }

    public override void Process(double delta) { }

    public override void PhysicsProcess() {
        _player.movement.X = 0;
        _player.movement.X += _player.IsKeyPressed("D") ? 1 : 0;
        _player.movement.X += _player.IsKeyPressed("A") ? -1 : 0;
        _player.movement.X *= _player.movingSpeed;
        
        if (_player.movement.IsZeroApprox()) {
            _player.SwitchState(PlayerStateId.Idle);
            return;
        }

        _player.MoveCharacter();
    }

    public override void Enter() {
        // GD.Print("Entering Moving State");
        _player.SetAnimation(PlayerAnimationId.Moving);
    }

    public override void Exit() { }
    public override void IsGrounded(bool isGrounded) {
        if (!isGrounded) {
            _player.SwitchState(PlayerStateId.Falling);
        }
    }
}