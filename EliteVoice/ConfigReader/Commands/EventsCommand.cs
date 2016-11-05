using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteVoice.ConfigReader.Commands
{
    class EventsCommand : AbstractCommand
    {
        public override int runCommand(IDictionary<string, Object> parameters)
        {
            runChilds(parameters);
            return 0;
        }
    }
}
