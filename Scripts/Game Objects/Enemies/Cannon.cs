using System.Collections.Generic;
using Godot;
using ServiceSystem;

public partial class Cannon : AnimatedSprite2D, ITick {
	[Export]
	private PackedScene _cannonball;

	[Export]
	private bool _facingRight;

	[Export]
	private Node2D _cannonballSpawn;

	private readonly TickTimer _cooldownTimer = new TickTimer();
	private int _cooldown = 5 * Engine.PhysicsTicksPerSecond;
	private List<Cannonball> _cannonballs = new();
	private float _cannonballVelocity = 180 / Engine.PhysicsTicksPerSecond;
	private GameClock _gameClock;

	public override void _Ready() {
		ServiceLocator serviceLocator = GetNode<ServiceLocator>(ServiceLocator.AutoloadPath);
		_gameClock = serviceLocator.GetService<GameClock>(ServiceName.GameClock);
		_gameClock.AddActiveScene(this, GetInstanceId());

		_FireCannon();
		_cooldownTimer.TimedOut += _FireCannon;
		_cooldownTimer.StartFixedTimer(true, _cooldown);
		AnimationFinished += _SpawnCannonBall;
	}

	public void PhysicsTick() {
		_cooldownTimer.PhysicsTick();
		foreach (Cannonball cannonball in _cannonballs) {
			cannonball.PhysicsTick();
		}
	}

	public override void _ExitTree() {
		_gameClock.RemoveActiveScene(GetInstanceId());
	}

	private void _SpawnCannonBall() {
		Cannonball cannonball = _cannonball.Instantiate() as Cannonball;
		AddChild(cannonball);
		cannonball.Position = _cannonballSpawn.Position;
		cannonball.Initialize(new Vector2(_facingRight ? _cannonballVelocity : -_cannonballVelocity, 0));
		_cannonballs.Add(cannonball);
		cannonball.Destroyed += () => _cannonballs.Remove(cannonball);
	}

	private void _FireCannon() {
		Play();
	}
}
