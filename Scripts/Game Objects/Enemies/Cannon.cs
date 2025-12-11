using Godot;

public partial class Cannon : AnimatedSprite2D {
    private readonly TickTimer _cooldownTimer = new TickTimer();
    private int _cooldown = 5 * Engine.PhysicsTicksPerSecond;

    public override void _Ready() {
        _FireCannon();
        _cooldownTimer.TimedOut += _FireCannon;
        _cooldownTimer.StartFixedTimer(true, _cooldown);
    }

    public void _FireCannon() {
        Play();
        // TODO: Spawn projectile after animation finishes
    }
}