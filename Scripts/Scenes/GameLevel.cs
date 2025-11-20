using Godot;

public partial class GameLevel : Node2D {
    [Export]
    private long _noiseThreshold;

    private NoiseCounter _noiseCounter;

    public override void _Ready() {
        _noiseCounter = new NoiseCounter(_noiseThreshold);
        _noiseCounter.ThresholdReached += _GameOver;
    }

    private void _GameOver() {
        // TODO: Add function to handle game over
    }
    
}