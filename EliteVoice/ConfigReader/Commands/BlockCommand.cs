using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EliteVoice.ConfigReader.Commands
{
    class BlockCommand : AbstractCommand
    {
        public override int runCommand(XmlElement node)
        {
            runChilds(node);
            return 0;
        }
    }
}
