using Godot;
using System;
using PlayerSystem;

public partial class Hazard : Sprite2D {
    [Export]
    private Area2D _area2D;
    
    public override void _Ready() {
        _area2D.BodyEntered += HandleCollision;
    }

    private void HandleCollision(Node2D body) {
        if (body is Player) {
            // TODO: Kill player
            GD.Print("Spike killed player");
        }
    }
}