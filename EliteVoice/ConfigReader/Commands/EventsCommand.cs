using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saxon.Api;

namespace EliteVoice.ConfigReader.Commands
{
    class EventsCommand : AbstractCommand
    {
        public override int runCommand(XdmNode node)
        {
            runChilds(node);
            return 0;
        }
    }
}
