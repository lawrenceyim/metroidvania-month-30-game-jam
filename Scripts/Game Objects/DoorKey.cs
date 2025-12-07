using Godot;
using PlayerSystem;

public partial class DoorKey : AnimatedSprite2D {
    [Export]
    private Area2D _hitbox;

    [Export]
    private Door _door;

    public override void _Ready() {
        _hitbox.BodyEntered += _HandleCollision;
    }

    private void _HandleCollision(Node2D body) {
        if (body is Player) {
            GD.Print("Player touched key");
            _door.UnlockDoor();
            QueueFree();
        }
    }
}