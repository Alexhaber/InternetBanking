using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Helpers
{
    public class SerialGenerator
    {
        private readonly object LockObj = new object();
        private long _lastTimestamp = DateTime.UtcNow.Ticks;
        private int _counter = 0;

        public async Task<string> GenerateSerial()
        {
            lock (LockObj)
            {
                long currentTimestamp = DateTime.UtcNow.Ticks;

                if (currentTimestamp == _lastTimestamp)
                {
                    _counter++;
                }
                else
                {
                    _counter = 0;
                    _lastTimestamp = currentTimestamp;
                }

                // Combina el timestamp y el contador, luego toma los primeros 9 dígitos
                string serial = $"{currentTimestamp}{_counter}".Substring(0, 9);
                return serial;
            }
        }
    }
}