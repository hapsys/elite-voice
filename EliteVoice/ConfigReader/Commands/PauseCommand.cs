using System;
using System.Threading;
using Saxon.Api;

namespace EliteVoice.ConfigReader.Commands
{
    class PauseCommand : AbstractCommand
    {
        public override int runCommand(XdmNode node)
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
