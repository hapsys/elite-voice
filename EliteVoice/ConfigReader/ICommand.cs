using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteVoice.ConfigReader
{
    interface ICommand
    {
        LinkedList<ICommand> getChilds();
        void addChild(ICommand command);
        IDictionary<string,string> getProperties();
        void addProperty(String key, String value);

        int runCommand(IDictionary<string, Object> parameters);
    }
}
