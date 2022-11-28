using System.Diagnostics;

namespace Game
{
    internal class Time
    {
        private int _currentFramesPerSecond = 0;
        private double _timeSinceLastFpsProbe = 0.0;
        private int _framesSinceLastFpsProbe = 0;

        private int _currentFrame = 0;
        private double _deltaTimeInSeconds = 0.0;
        private double _currentTimeInSeconds = 0.0;

        private const int SampleCount = 500;
        private List<float> _deltaTimeSamples = new List<float>(SampleCount);

        private readonly Stopwatch _stopwatch = new();

        public double DeltaTime => _deltaTimeInSeconds;
        public float DeltaTimeF => (float)_deltaTimeInSeconds;
        public double TotalTime => _currentTimeInSeconds;
        public int CurrentFrame => _currentFrame;
        public int FramesPerSecond => _currentFramesPerSecond;
        public List<float> DeltaTimeSamples => _deltaTimeSamples;
        public float AverageDeltaTimeF => _deltaTimeSamples.Average();

        public void Start() => _stopwatch.Start();

        public void Update()
        {
            _deltaTimeInSeconds = 0.0;

            ++_currentFrame;

            _deltaTimeInSeconds = _stopwatch.Elapsed.TotalSeconds - _currentTimeInSeconds;
            _currentTimeInSeconds = _stopwatch.Elapsed.TotalSeconds;

            _timeSinceLastFpsProbe += _deltaTimeInSeconds;
            if (_timeSinceLastFpsProbe >= 1.0)
            {
                _currentFramesPerSecond = _currentFrame - _framesSinceLastFpsProbe;

                _timeSinceLastFpsProbe = 0.0;
                _framesSinceLastFpsProbe = _currentFrame;
            }

            _deltaTimeSamples.Add(DeltaTimeF);
            if (_deltaTimeSamples.Count > SampleCount)
            {
                _deltaTimeSamples.RemoveAt(0);
            }
        }
    }
}
