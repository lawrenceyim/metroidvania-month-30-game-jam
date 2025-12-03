using Godot;
using System;
using PlayerSystem;
using ServiceSystem;

public partial class Hazard : Sprite2D {
    [Export]
    private Area2D _area2D;

    private SceneManager _sceneManager;

    public override void _Ready() {
        _area2D.BodyEntered += HandleCollision;

        ServiceLocator serviceLocator = GetNode<ServiceLocator>(ServiceLocator.AutoloadPath);
        _sceneManager = serviceLocator.GetService<SceneManager>(ServiceName.SceneManager);
    }

    private void HandleCollision(Node2D body) {
        if (body is Player) {
            // TODO: Kill player
            GD.Print("Spike killed player");
            _sceneManager.ChangeToCurrentScene();
        }
    }
}