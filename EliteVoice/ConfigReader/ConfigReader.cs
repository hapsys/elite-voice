using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using EliteVoice.ConfigReader.Commands;

namespace EliteVoice.ConfigReader
{
    class ConfigReader
    {
        private IDictionary<string, Type> registration = new Dictionary<string, Type>();
        public IDictionary<string, EventsCommand> events { get; } = new Dictionary<string, EventsCommand>();

        TextLogger logger = TextLogger.instance;

        private string config;
        public ConfigReader(string config)
        {
            registration.Add("TextToSpeech", Type.GetType("EliteVoice.ConfigReader.Commands.TextToSpeechCommand"));
            registration.Add("Text", Type.GetType("EliteVoice.ConfigReader.Commands.TextCommand"));
            registration.Add("Pause", Type.GetType("EliteVoice.ConfigReader.Commands.PauseCommand"));
            registration.Add("Play", Type.GetType("EliteVoice.ConfigReader.Commands.PlaySoundCommand"));

            this.config = config;
        }

        public ICommand getEvent(string eventName)
        {
            ICommand result = null;
            if (events.ContainsKey(eventName))
            {
                result = events[eventName];
            }
            return result;
        }

        public void parse()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(config);
            XmlNode root = xml.DocumentElement;
            if (root.HasChildNodes)
            {
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    if (root.ChildNodes[i].NodeType == XmlNodeType.Element && root.ChildNodes[i].Name.Equals("Event"))
                    {
                        XmlElement elm = (XmlElement)root.ChildNodes[i];
                        string name = elm.GetAttribute("name");
                        if (name.Length > 0)
                        {
                            EventsCommand command = new EventsCommand();
                            readContent(elm, command);
                            logger.log("Append command: {" + name + "}");
                            events.Add(name, command);
                        }
                    }
                }
            }

        }

        private void readContent(XmlElement current, ICommand parent)
        {
            for (int i = 0; i < current.Attributes.Count; i++)
            {
                parent.addProperty(current.Attributes[i].Name, current.Attributes[i].Value);
            }
            parent.addProperty("@text", current.InnerText);
            if (current.HasChildNodes)
            {
                for (int i = 0; i < current.ChildNodes.Count; i++)
                {
                    if (current.ChildNodes[i].NodeType == XmlNodeType.Element)
                    {
                        string cmdName = current.ChildNodes[i].Name;
                        if (registration.ContainsKey(cmdName))
                        {
                            ICommand command = (ICommand)Activator.CreateInstance(registration[cmdName]);
                            readContent((XmlElement)current.ChildNodes[i], command);
                            parent.addChild(command);
                        }
                    }
                }
            }
        }
    }
}
