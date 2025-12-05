using System;
using Godot;
using PlayerSystem;
using ServiceSystem;

public partial class Bat : AnimatedSprite2D, ITick {
    [Export]
    private Area2D _hitbox;

    [Export]
    private Vector2[] _patrolPoint;

    private int _currentPatrolPoint;
    private SceneManager _sceneManager;

    private enum State {
        Patrol,
        Pursue
    }

    private State _state = State.Patrol;
    private int _maxPatrolDistance = 50;
    private float _moveSpeed = 100f / Engine.PhysicsTicksPerSecond;
    private float _patrolThreshold = 5;

    public override void _Ready() {
        base._Ready();
        ServiceLocator serviceLocator = GetNode<ServiceLocator>(ServiceLocator.AutoloadPath);
        _sceneManager = serviceLocator.GetService<SceneManager>(ServiceName.SceneManager);
        _hitbox.BodyEntered += _HandleCollision;
        Play("default");
    }

    private void _HandleCollision(Node2D body) {
        if (body is Player) {
            // TODO: Kill player
            GD.Print("Bat killed player");
            CallDeferred(nameof(_ResetScene));
        }
    }

    private void _ResetScene() {
        _sceneManager.ChangeToCurrentScene();
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
        FlipH = Position.DirectionTo(_patrolPoint[_currentPatrolPoint]).X > 0;
        if (Position.DistanceTo(_patrolPoint[_currentPatrolPoint]) <= _patrolThreshold) {
            _currentPatrolPoint++;
            _currentPatrolPoint %= _patrolPoint.Length;
            return;
        }

        Position = Position.MoveToward(_patrolPoint[_currentPatrolPoint], _moveSpeed);
    }

    private void _PursueTick() { }
}