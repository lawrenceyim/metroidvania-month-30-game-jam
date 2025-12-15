using System;
using System.Collections.Generic;
using Godot;
using Godot.NativeInterop;
using InputSystem;
using ServiceSystem;

namespace PlayerSystem;

public partial class Player : CharacterBody2D, ITick, IInputState {
    public event Action IncreaseNoiseLevel;

    internal Vector2 movement = Vector2.Zero;
    internal float movingSpeed = 3f;
    internal float gravityForce = .1f;
    internal float jumpForce = -300 / Engine.PhysicsTicksPerSecond;

    [Export]
    private AnimatedSprite2D _sprite;

    [Export]
    private ShapeCast2D _terrainCheck;

    [Export]
    private CollisionShape2D _hitbox;

    [Export]
    private AudioStreamPlayer _sfx;

    [Export]
    private AudioStream _walkingSfx;

    [Export]
    private AudioStream _jumpingSfx;

    private readonly PlayerStateMachine _playerStateMachine = new();
    private readonly Dictionary<string, bool> _keyPressed = new();

    // refactor these to the active scene rather than the player character itself
    private InputStateMachine _inputStateMachine;
    private GameClock _gameClock;
    private TickTimer _groundCheckCooldown;
    private SfxId _currentSfx = SfxId.StopPlaying;

    public override void _Ready() {
        ServiceLocator serviceLocator = GetNode<ServiceLocator>(ServiceLocator.AutoloadPath);
        _inputStateMachine = serviceLocator.GetService<InputStateMachine>(ServiceName.InputStateMachine);
        _inputStateMachine.SetState(this);
        _gameClock = serviceLocator.GetService<GameClock>(ServiceName.GameClock);
        _gameClock.AddActiveScene(this, GetInstanceId());
        _InitializeStateMachine();
        _sprite.Play();
        _sfx.Finished += _handleSfxLoop;
    }

    public override void _ExitTree() {
        _inputStateMachine.SetState(null);
        _gameClock.RemoveActiveScene(GetInstanceId());
    }

    public void PhysicsTick() {
        _playerStateMachine.PhysicsProcess();

        // Main level should have a counter for this. Something like Ticks Per Second * 60 seconds or something
        if (_playerStateMachine.GetCurrentKey() == PlayerStateId.Moving) {
            IncreaseNoiseLevel?.Invoke();
        }

        _IsOnTerrain();
    }

    public void SetKeyPressed(string key, bool keyPressed) {
        _keyPressed[key] = keyPressed;
    }

    public bool IsKeyPressed(string key) {
        return _keyPressed.GetValueOrDefault(key, false);
    }

    public void ProcessInput(InputEventDto eventDto) {
        _playerStateMachine?.Input(eventDto);
    }

    public void SwitchState(PlayerStateId newState) {
        _playerStateMachine.SwitchState(newState);
    }

    public void SetAnimation(PlayerAnimationId id) {
        string animationName = id switch {
            PlayerAnimationId.Idle => "Idle",
            PlayerAnimationId.Moving => "Move",
            PlayerAnimationId.Jump => "Jump",
        };
        // GD.Print($"Setting animation: {animationName}");
        _sprite.SetAnimation(animationName);
    }

    public void SetDirectionFaced(bool facingRight) {
        _sprite.FlipH = !facingRight;
    }

    public void MoveCharacter() {
        Velocity = movement * Engine.PhysicsTicksPerSecond;
        MoveAndSlide();
        if (movement.X > 0) {
            SetDirectionFaced(true);
        } else if (movement.X < 0) {
            SetDirectionFaced(false);
        }
    }

    public void PlaySfx(SfxId sfxId) {
        _currentSfx = sfxId;
        switch (sfxId) {
            case SfxId.StopPlaying:
                _sfx.Stop();
                break;
            case SfxId.Jumping:
                _sfx.Stream = _jumpingSfx;
                _sfx.Play();
                break;
            case SfxId.Walking:
                _sfx.Stream = _walkingSfx;
                _sfx.Play();
                break;
        }
    }

    private void _handleSfxLoop() {
        switch (_currentSfx) {
            case SfxId.StopPlaying:
                break;
            case SfxId.Jumping:
                break;
            case SfxId.Walking:
                _sfx.Play();
                break;
        }
    }

    private void _IsOnTerrain() {
        _terrainCheck.ForceShapecastUpdate();

        if (!_terrainCheck.IsColliding()) {
            _playerStateMachine.IsGrounded(false);
            return;
        }

        _playerStateMachine.IsGrounded(true);
    }

    private void _InitializeStateMachine() {
        _playerStateMachine.AddState(PlayerStateId.Idle, new IdleState(this));
        _playerStateMachine.AddState(PlayerStateId.Moving, new MovingState(this));
        _playerStateMachine.AddState(PlayerStateId.Jumping, new JumpingState(this));
        _playerStateMachine.AddState(PlayerStateId.Falling, new FallingState(this));

        _playerStateMachine.SwitchState(PlayerStateId.Idle);
    }

    public enum SfxId {
        StopPlaying,
        Walking,
        Jumping,
    }
}