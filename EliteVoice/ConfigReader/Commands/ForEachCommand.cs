using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saxon.Api;

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
		public override int runCommand(XdmNode node)
		{
			if (selectParam != null && selectParam.Length > 0)
			{
				XdmValue nodes = XMLContext.instance.xpath.Evaluate(selectParam, node);
				if (nodes.Count > 0)
                {
					foreach (XdmNode element in nodes)
                    {
						runChilds(element);
					}

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
