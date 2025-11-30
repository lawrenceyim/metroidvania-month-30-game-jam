using System;
using Godot;
using PlayerSystem;

public partial class Bat : AnimatedSprite2D, ITick {
    private static Random _random = new();
    private readonly TickTimer _patrolTimer = new TickTimer();

    [Export]
    private Area2D _hitbox;

    [Export]
    private Vector2 _patrolPoint;

    private enum State {
        Patrol,
        Pursue
    }

    private State _state = State.Patrol;
    private int _maxPatrolDistance = 50;
    private int _patrolDuration = 4 * Engine.PhysicsTicksPerSecond;
    private Vector2 _currentTarget;
    private float _moveSpeed = .5f;
    private float _patrolThreshold = 5;

    public override void _Ready() {
        base._Ready();
        _patrolTimer.TimedOut += _SelectPatrolPoint;
        _SelectPatrolPoint();
        _hitbox.BodyEntered += HandleCollision;
        Play("default");
    }

    private void HandleCollision(Node2D body) {
        if (body is Player player) {
            // TODO: Kill player
            GD.Print("Bat killed player");
        }
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        PhysicsTick();
    }

    public void PhysicsTick() {
        switch (_state) {
            case State.Patrol:
                _PatrolTick();
                break;
            case State.Pursue:
                _PursueTick();
                break;
        }
    }

    private void _PatrolTick() {
        _patrolTimer.PhysicsTick();
        FlipH = Position.DirectionTo(_currentTarget).X > 0;
        if (Position.DistanceTo(_currentTarget) < _patrolThreshold) {
            return;
        }
        
        Position = Position.MoveToward(_currentTarget, _moveSpeed);
    }

    private void _PursueTick() { }

    private void _SelectPatrolPoint() {
        _patrolTimer.StartFixedTimer(true, _patrolDuration);
        _currentTarget = _patrolPoint + new Vector2(_random.Next(0, _maxPatrolDistance), _random.Next(0, _maxPatrolDistance));
    }
}