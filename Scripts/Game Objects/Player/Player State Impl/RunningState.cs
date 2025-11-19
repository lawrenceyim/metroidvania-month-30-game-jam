using Godot;
using InputSystem;

namespace PlayerSystem;

public class RunningState : PlayerState {
    private Vector2 _movement = Vector2.Zero;
    private Player _player;
    private float _runningSpeed = 1f; // Per Tick

    public RunningState(Player player) {
        _player = player;
    }

    public override void Input(InputEventDto dto) {
        if (dto is KeyDto keyDto) {
            _player.SetKeyPressed(keyDto.Identifier, keyDto.Pressed);
        }
    }

    public override void Process(double delta) { }

    public override void PhysicsProcess() {
        _movement = Vector2.Zero;
        _movement.X += _player.IsKeyPressed("D") ? 1 : 0;
        _movement.X += _player.IsKeyPressed("A") ? -1 : 0;

        _player.Position += _movement.Normalized() * _runningSpeed;
    }

    public override void Enter() { }

    public override void Exit() { }
}