using System;
using Godot;
using PlayerSystem;

public partial class Slime : CharacterBody2D, ITick {
	[Export]
	private float _leftPosition;

	[Export]
	private float _rightPosition;

	[Export]
	private bool _movingRight;

	[Export]
	private Area2D _hitbox;

	private float _moveSpeed = 6000f / Engine.PhysicsTicksPerSecond;
	private Vector2 _movement = Vector2.Zero;

	public override void _Ready() {
		_hitbox.BodyEntered += _HandleCollision;
		GD.Print(_moveSpeed);
	}

	private void _HandleCollision(Node2D body) {
		if (body is Player player) {
			// TODO: kill player
			GD.Print("Slime killed player");
		}
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		PhysicsTick();
	}

	public void PhysicsTick() {
		_movement.X = _movingRight ? _moveSpeed : -_moveSpeed;
		Velocity = _movement;
		MoveAndSlide();

		if (_movingRight) {
			if (Position.X >= _rightPosition) {
				_movingRight = false;
			}
		} else {
			if (Position.X <= _leftPosition) {
				_movingRight = true;
			}
		}
	}
}
