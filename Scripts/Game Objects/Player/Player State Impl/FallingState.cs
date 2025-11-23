using InputSystem;
using PlayerSystem;

public class FallingState : PlayerState {
    private Player _player;
    private readonly float _gravityAcceleration = 0.05f;

    public FallingState(Player player) {
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
        _player._movement.Y += _gravityAcceleration;
        _player._movement.X = 0;
        _player._movement.X += _player.IsKeyPressed("D") ? 1 : 0;
        _player._movement.X += _player.IsKeyPressed("A") ? -1 : 0;
        _player._movement.X *= _player._speed;
        _player.Position += _player._movement;
    }

    public override void Enter() {
        _player._movement.Y = 0;
    }
}