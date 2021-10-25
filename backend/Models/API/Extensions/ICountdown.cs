using System;

namespace Common.API.Extensions
{
    /// <summary>
    /// Counts down a specific time span
    /// </summary>
    public interface ICountdown
    {
        public bool IsRunning { get; }
        public double ElapsedMilliseconds { get; }
        public void StartCountDown(TimeSpan time);
        public void Stop();
        public void Reset();
       
    }
}
