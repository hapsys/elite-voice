using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteVoice.ConfigReader
{
    abstract class AbstractCommand : ICommand
    {
        public ICommand parent { protected set; get; } = null;
        LinkedList<ICommand> commands = new LinkedList<ICommand>();
        IDictionary<string, string> properties = new Dictionary<string,string>();

        protected TextLogger logger = TextLogger.instance;

        public AbstractCommand()
        {
        }
        public void addChild(ICommand command)
        {
			((AbstractCommand)command).parent = this;
			commands.AddLast(command);
        }

        public virtual void addProperty(string key, string value)
        {
            properties.Add(key, value);
        }

        public LinkedList<ICommand> getChilds()
        {
            return commands;
        }

        public IDictionary<string, string> getProperties()
        {
            return properties;
        }

        public abstract int runCommand(IDictionary<string, Object> parameters);

        protected void runChilds(IDictionary<string, Object> parameters)
        {
            foreach (ICommand command in commands)
            {
                if (command.runCommand(parameters) < 0)
                {
                    break;
                }
            }
        }
    }
}
