using Godot;
using PlayerSystem;

public partial class Ghost : AnimatedSprite2D, ITick {
	[Export]
	private Area2D _hitbox;

	[Export] // replace later
	private Player _player;

	private float _moveSpeed = 30f / Engine.PhysicsTicksPerSecond;

	public override void _Ready() {
		base._Ready();
		_hitbox.BodyEntered += _HandleCollision;
		GD.Print($"Player found {_player != null}");
		Play();
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		PhysicsTick();
	}

	private void _HandleCollision(Node2D body) {
		if (body is Player) {
			// kill player
			GD.Print("Ghost killed player");
		}
	}

	public void PhysicsTick() {
		Position = Position.MoveToward(_player.Position, _moveSpeed);
	}
}
