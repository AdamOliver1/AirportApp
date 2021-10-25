using Common.API.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Extentions
{
    public class Countdown : ICountdown
    {
        private DateTime _startTime;
        private DateTime _stopTime;
        private TimeSpan _countTime;
        bool _running = false;
        bool _isReset = false;
        public bool IsRunning => _running;
        public double ElapsedMilliseconds => GetTimePassed();
        public Countdown()
        {
            _startTime = new DateTime();
            _stopTime = new DateTime();
            _countTime = new TimeSpan();
        }

        public void StartCountDown(TimeSpan time)
        {
            _isReset = false;
            _countTime = time;
            _running = true;
            _startTime = DateTime.Now;
        }

        public void Stop()
        {
            _stopTime = DateTime.Now;
            _running = false;
        }

        public void Reset()
        {
            _isReset = true;
            _stopTime = DateTime.Now;
            _startTime = DateTime.Now;
            _running = false;
        }

        public double GetTimePassed()
        {
            if (_running)
                return (_countTime - (DateTime.Now - _startTime)).TotalMilliseconds;
            else if (!_running && !_isReset)
                return (_countTime - (_stopTime - _startTime)).TotalMilliseconds;
            else
                return 0;
        }
    }
}
