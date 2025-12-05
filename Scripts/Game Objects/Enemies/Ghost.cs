using Godot;
using PlayerSystem;
using ServiceSystem;

public partial class Ghost : AnimatedSprite2D, ITick {
	[Export]
	private Area2D _hitbox;

	[Export] // replace later
	private Player _player;

	private float _moveSpeed = 30f / Engine.PhysicsTicksPerSecond;
	private SceneManager _sceneManager;

	public override void _Ready() {
		base._Ready();
		ServiceLocator serviceLocator = GetNode<ServiceLocator>(ServiceLocator.AutoloadPath);
		_sceneManager = serviceLocator.GetService<SceneManager>(ServiceName.SceneManager);
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
			GD.Print("Ghost killed player");
			CallDeferred(nameof(_ResetScene));
		}
	}

	private void _ResetScene() {
		_sceneManager.ChangeToCurrentScene();
	}

	public void PhysicsTick() {
		Position = Position.MoveToward(_player.Position, _moveSpeed);
	}
}
