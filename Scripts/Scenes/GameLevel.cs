using Godot;
using PlayerSystem;
using ServiceSystem;

public partial class GameLevel : Node2D {
    [Export]
    private long _noiseThreshold;

    [Export]
    private Area2D _goalArea; // Probably better to create Dict<Area2D, SceneId> to change scenes to

    private GameClock _gameClock;
    private NoiseCounter _noiseCounter;

    public override void _Ready() {
        ServiceLocator serviceLocator = GetNode<ServiceLocator>(ServiceLocator.AutoloadPath);
        _gameClock = serviceLocator.GetService<GameClock>(ServiceName.GameClock);

        _noiseCounter = new NoiseCounter(_noiseThreshold);
        _noiseCounter.ThresholdReached += _GameOver;
        _goalArea.BodyEntered += _GoalEntered;
    }

    private void _GoalEntered(Node2D body) {
        if (body is Player player) {
            // TODO: Add function to handle going to the next level
        }
    }

    private void _GameOver() {
        // TODO: Add function to handle game over
    }
}