using System;
using Godot;
using PlayerSystem;
using ServiceSystem;

public partial class Cannonball : Sprite2D, ITick {
    public event Action Destroyed;
    
    [Export]
    private Area2D _hitbox;

    private Vector2 _velocity;
    private SceneManager _sceneManager;
    private TickTimer _tickTimer = new TickTimer();

    public void Initialize(Vector2 velocity) {
        _velocity = velocity;
        _hitbox.BodyEntered += _HandleCollision;
        _tickTimer.TimedOut += () => {
            Destroyed?.Invoke();
            QueueFree();
        };
        _tickTimer.StartFixedTimer(false, 15 * Engine.PhysicsTicksPerSecond);

        ServiceLocator serviceLocator = GetNode<ServiceLocator>(ServiceLocator.AutoloadPath);
        _sceneManager = serviceLocator.GetService<SceneManager>(ServiceName.SceneManager);
    }

    public void PhysicsTick() {
        Position += _velocity;
    }

    private void _HandleCollision(Node2D body) {
        if (body is Player) {
            Destroyed?.Invoke();
            CallDeferred(nameof(_ResetScene));
        }
    }

    private void _ResetScene() {
        _sceneManager.ChangeToCurrentScene();
    }
}