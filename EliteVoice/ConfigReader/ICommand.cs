using System;
using System.Collections.Generic;
using Saxon.Api;


namespace EliteVoice.ConfigReader
{
    interface ICommand
    {
        LinkedList<ICommand> getChilds();
        void addChild(ICommand command);
        IDictionary<string,string> getProperties();
        void addProperty(String key, String value);

        int runCommand(XdmNode node);
    }
}
