using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    public class ServiceTime
    {
        public const int Tick2ms = 10000;

        public static int GetServiceTime()
        {
            return (int)(DateTime.Now.Ticks / Tick2ms);
        }
    }
  
}
