using Godot;
using InputSystem;
using PlayerSystem;

public class FallingState : PlayerState {
    private Player _player;

    public FallingState(Player player) {
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
        _player.movement.Y += _player.gravityForce;
        _player.movement.X = 0;
        _player.movement.X += _player.IsKeyPressed("D") ? 1 : 0;
        _player.movement.X += _player.IsKeyPressed("A") ? -1 : 0;
        _player.movement.X *= _player.movingSpeed;
        _player.MoveCharacter();
    }

    public override void Enter() {
        // GD.Print("Entering Falling State");
        _player.movement.Y = 0;
        _player.SetAnimation(PlayerAnimationId.Jump);
    }
}