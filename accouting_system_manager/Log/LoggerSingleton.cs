using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace accouting_system_manager.Log
{
    public static class LoggerSingleton
    {
        private static FileLogger logger;

        public static FileLogger GetInstance
        {
            get
            {
                if (null == logger) logger = new FileLogger();

                return logger;
            }
        }
    }
}
