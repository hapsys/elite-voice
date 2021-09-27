using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Saxon.Api;


namespace EliteVoice.ConfigReader.Commands
{
	class SwitchCommand : AbstractCommand
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
		public override int runCommand(XdmNode node)
		{
			if (selectParam != null && selectParam.Length > 0)
			{
				//XmlElement selectNode = (XmlElement)node.SelectSingleNode(selectParam);
				XdmItem selectNode = XMLContext.instance.xpath.EvaluateSingle(selectParam, node);
				if (selectNode != null && selectNode.IsNode())
                {
					runChilds((XdmNode)selectNode);
				}
			}
			else
			{
				logger.log("Check \"select\" parameter at Switch command!");
			}
			return 0;
		}
	}
}
