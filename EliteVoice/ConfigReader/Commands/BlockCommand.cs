using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteVoice.ConfigReader.Commands
{
    class BlockCommand : AbstractCommand
    {
        public override int runCommand(IDictionary<string, object> parameters)
        {
            runChilds(parameters);
            return 0;
        }
    }
}
