using Godot;
using InputSystem;

namespace PlayerSystem;

public class RunningState : PlayerState {
    private readonly Player _player;
    private readonly float _runningSpeed = 1f; // Per Tick

    public RunningState(Player player) {
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
        Vector2 _movement = Vector2.Zero;
        _movement.X += _player.IsKeyPressed("D") ? 1 : 0;
        _movement.X += _player.IsKeyPressed("A") ? -1 : 0;
        _player.Position += _movement * _runningSpeed;
    }

    public override void Enter() { }

    public override void Exit() { }
}