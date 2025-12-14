using Godot;
using InputSystem;

namespace PlayerSystem;

public class JumpingState : PlayerState {
    private readonly Player _player;
    private int _durationTicksLeft;

    public JumpingState(Player player) {
        _player = player;
    }

    public override void Exit() { }

    public override void IsGrounded(bool isGrounded) {
        if (isGrounded) {
            _player.movement.Y = 0;
            _player.SwitchState(PlayerStateId.Idle);
        }
    }

    public override void Input(InputEventDto dto) {
        if (dto is KeyDto keyDto) {
            _player.SetKeyPressed(keyDto.Identifier, keyDto.Pressed);
        }
    }

    public override void Process(double delta) { }

    public override void PhysicsProcess() {
        _player.movement.X = 0;
        _player.movement.X += _player.IsKeyPressed("D") ? 1 : 0;
        _player.movement.X += _player.IsKeyPressed("A") ? -1 : 0;
        _player.movement.X *= _player.movingSpeed;
        _durationTicksLeft--;
        _player.MoveCharacter();
        if (_durationTicksLeft <= 0) {
            _player.SwitchState(PlayerStateId.Falling);
        }
    }

    public override void Enter() {
        // GD.Print("Entering Jumping State");
        _player.movement.Y = _player.jumpForce;
        _durationTicksLeft = (int)(.3f * Engine.PhysicsTicksPerSecond); // TODO: Temporary fix
        _player.SetAnimation(PlayerAnimationId.Idle);

        // TODO: Switch animation
        // TODO: Switch to airborne state when animation ends
    }
}