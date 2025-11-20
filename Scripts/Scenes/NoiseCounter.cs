using System;

public class NoiseCounter {
    public event Action ThresholdReached;
    public long Threshold { get; }
    public long CurrentLevel { get; private set; } = 0;

    public NoiseCounter(long threshold) {
        Threshold = threshold;
    }

    public void Increment() {
        CurrentLevel++;

        if (CurrentLevel == Threshold) {
            ThresholdReached?.Invoke();
        }
    }
}