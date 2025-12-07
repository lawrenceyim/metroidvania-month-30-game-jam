using Godot;

public partial class Door : Sprite2D {
    [Export]
    private StaticBody2D _hitbox;

    [Export]
    private AtlasTexture _lockedTexture;

    [Export]
    private AtlasTexture _openedTexture;

    public override void _Ready() {
        LockDoor();
    }

    public void LockDoor() {
        GD.Print("Locking door");
        Texture = _lockedTexture.Atlas;
        CallDeferred(nameof(_SetCollisionShapeState), false);
    }

    public void UnlockDoor() {
        GD.Print("Unlocking door");
        Texture = _openedTexture.Atlas;
        CallDeferred(nameof(_SetCollisionShapeState), true);
    }

    private void _SetCollisionShapeState(bool disabled) {
        foreach (Node node in _hitbox.GetChildren()) {
            if (node is CollisionShape2D shape) {
                GD.Print($"Disabled collision shape for door: {disabled}");
                shape.Disabled = disabled;
            }
        }
    }
}