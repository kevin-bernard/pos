using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace accouting_system_manager.Util
{
    public class ReportAction
    {
        public delegate void RunAction(RunActionProgress progress);

        public event RunAction OnRunAction;

        public class RunActionProgress
        {
            public int Cursor { get; set; } = 0;

            public int NbData { get; set; } = 0;

            public string Message { get; set; }
        }
    }
}
