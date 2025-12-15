using Godot;
using PlayerSystem;
using ServiceSystem;

public partial class GameLevel : Node2D, ITick {
    [Export]
    private long _noiseThreshold;

    [Export]
    private Area2D _goalArea; // Probably better to create Dict<Area2D, SceneId> to change scenes to

    [Export]
    private SceneId _sceneId;

    [Export]
    private AudioStreamPlayer _audioStreamPlayer;

    [Export]
    private AudioStream _regularMusic;

    [Export]
    private AudioStream _panicMusic;

    private readonly TickTimer _timeLeftTimer = new TickTimer();
    private GameClock _gameClock;
    private SceneManager _sceneManager;
    private int _ticksPerStage = 20 * Engine.PhysicsTicksPerSecond;
    private bool _isPanicked = false;

    public override void _Ready() {
        ServiceLocator serviceLocator = GetNode<ServiceLocator>(ServiceLocator.AutoloadPath);
        _gameClock = serviceLocator.GetService<GameClock>(ServiceName.GameClock);
        _sceneManager = serviceLocator.GetService<SceneManager>(ServiceName.SceneManager);
        _sceneManager.SetCurrentSceneId(_sceneId);

        _gameClock.AddActiveScene(this, GetInstanceId());

        _timeLeftTimer.StartFixedTimer(false, _ticksPerStage);
        _timeLeftTimer.TimedOut += _HandleTimeOut;
        // _noiseCounter = new NoiseCounter(_noiseThreshold);
        // _noiseCounter.ThresholdReached += _GameOver;
        // _goalArea.BodyEntered += _GoalEntered;
    }

    public override void _ExitTree() {
        _gameClock.RemoveActiveScene(GetInstanceId());
    }

    public void PhysicsTick() {
        _timeLeftTimer.PhysicsTick();
    }

    private void _HandleTimeOut() {
        GD.Print("HandleTimeOut");
        if (_isPanicked) {
            GD.Print("Panicked");
            _GameOver();
        } else {
            GD.Print("Wasn't panicked");
            _isPanicked = true;
            _audioStreamPlayer.Stream = _panicMusic;
            _audioStreamPlayer.Play();
            _timeLeftTimer.StartFixedTimer(false, _ticksPerStage);
        }
    }

    private void _GoalEntered(Node2D body) {
        if (body is Player player) {
            // TODO: Add function to handle going to the next level
        }
    }

    private void _GameOver() {
        _sceneManager.ChangeToCurrentScene();
    }
}