using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EliteVoice.ConfigReader.Commands
{
    class PauseCommand : AbstractCommand
    {
        public override int runCommand(IDictionary<string, Object> parameters)
        {
            if (getProperties().ContainsKey("value"))
            {
                int val = Int32.Parse(getProperties()["value"]);
                Thread.Sleep(val);
            }
            return 0;
        }
    }
}
