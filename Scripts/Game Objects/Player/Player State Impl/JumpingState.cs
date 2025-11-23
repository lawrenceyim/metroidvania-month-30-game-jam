using Godot;
using InputSystem;

namespace PlayerSystem;

public class JumpingState : PlayerState {
    private readonly Player _player;
    private readonly float _airborneSpeed = 1f;
    private readonly float _jumpForce = -5f;
    private readonly float _gravityAcceleration = 0.05f;
    Vector2 _movement = Vector2.Zero;

    public JumpingState(Player player) {
        _player = player;
    }

    public override void Exit() { }

    public override void Input(InputEventDto dto) {
        if (dto is KeyDto keyDto) {
            _player.SetKeyPressed(keyDto.Identifier, keyDto.Pressed);
        }
    }

    public override void Process(double delta) { }

    public override void PhysicsProcess() {
        _movement.Y += _gravityAcceleration;
        _movement.X = 0;
        _movement.X += _player.IsKeyPressed("D") ? 1 : 0;
        _movement.X += _player.IsKeyPressed("A") ? -1 : 0;
        _movement.X *= _airborneSpeed;
        _player.Position += _movement;
    }

    public override void Enter() {
        _movement.Y = _jumpForce;

        // TODO: Switch animation
    }
}