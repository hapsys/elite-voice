using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using SpeechLib;

namespace EliteVoice.ConfigReader.Commands
{
    class TextCommand : AbstractCommand
    {
		public override int runCommand(XmlElement node)
        {

            Speech sp = Speech.instance;
            string text = "";
            if (getProperties().ContainsKey("select"))
            {
                XPathNavigator navigator = node.CreateNavigator();
                //text = XMLContext.instance.EvaluateSting(XMLContext.instance.getXPathExpression(getProperties()["select"]), navigator);
                XmlNodeList iter = node.SelectNodes(getProperties()["select"]);
                foreach (XmlNode child in iter)
                {
                    switch (child.NodeType)
                    {
                        case XmlNodeType.Element:
                            text += child.InnerText;
                            break;
                        case XmlNodeType.Text:
                            text += child.InnerText;
                            break;
                    }
                }
                logger.log("Text Select " + text);
            }
            else if (getProperties().ContainsKey("@text"))
            {
                text = getProperties()["@text"];
				//logger.log("Replacers count " + EventContext.instance.replacers.Count);
				foreach (Replacer rp in EventContext.instance.replacers)
				{
					text = rp.Replace(text);
				}
            }

            if (text != null && text.Length > 0)
            {
                sp.speak(text);
            }
            return 0;
        }
    }
}
