using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace EliteVoice.ConfigReader.Commands
{
	class ForEachCommand : AbstractCommand
	{

		private string selectParam = null;
		public override void addProperty(string key, string value)
		{
			base.addProperty(key, value);
			switch (key)
			{
				case "select":
					selectParam = value;
					break;
			}
		}
		public override int runCommand(XmlElement node)
		{
			if (selectParam != null && selectParam.Length > 0)
			{
				XmlNodeList nodes = node.SelectNodes(selectParam);
				foreach (XmlNode element in nodes)
                {
					runChilds((XmlElement)element);

				}
			}
			else
			{
				logger.log("Check \"select\" parameter at ForEach command!");
			}
			return 0;
		}
	}
}
