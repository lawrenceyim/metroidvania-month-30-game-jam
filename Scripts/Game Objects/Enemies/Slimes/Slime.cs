using Godot;
using PlayerSystem;

public partial class Slime : CharacterBody2D, ITick {
    [Export]
    private Vector2 _leftPosition;

    [Export]
    private Vector2 _rightPosition;

    [Export]
    private bool _movingRight;

    [Export]
    private Area2D _hitbox;

    private float _moveSpeed = 2f * Engine.PhysicsTicksPerSecond;
    private Vector2 _movement = Vector2.Zero;
    private float _distanceThreshold = 10f;

    public override void _Ready() {
        _hitbox.BodyEntered += _HandleCollision;
    }

    private void _HandleCollision(Node2D body) {
        if (body is Player player) {
            // TODO: kill player
            GD.Print("Killed player");
        }
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        PhysicsTick();
    }

    public void PhysicsTick() {
        _movement.X = _movingRight ? _moveSpeed : -_moveSpeed;
        Velocity = _movement;
        MoveAndSlide();

        if (_movingRight) {
            if (_rightPosition.DistanceTo(Position) <= _distanceThreshold) {
                _movingRight = false;
            }
        } else {
            if (_leftPosition.DistanceTo(Position) <= _distanceThreshold) {
                _movingRight = true;
            }
        }
    }
}