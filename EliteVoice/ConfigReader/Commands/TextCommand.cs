using System;
using System.Collections.Generic;
using System.Linq;
using Saxon.Api;


namespace EliteVoice.ConfigReader.Commands
{
    class TextCommand : AbstractCommand
    {
		public override int runCommand(XdmNode node)
        {

            Speech sp = Speech.instance;
            string text = "";
            if (getProperties().ContainsKey("select"))
            {
                string exp = getProperties()["select"];
                XdmValue iter = XMLContext.instance.xpath.Evaluate(exp, node);
                foreach (XdmItem child in iter)
                {
                    /*
                    switch (child.NodeType)
                    {
                        case XmlNodeType.Element:
                            text += child.InnerText;
                            break;
                        case XmlNodeType.Text:
                            text += child.InnerText;
                            break;
                    }
                    */
                    if (child.IsNode())
                    {
                        text += ((XdmNode)child).StringValue;
                    } else if (child.IsAtomic())
                    {
                        text += child.Simplify.ToString();
                    }
                    //
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
