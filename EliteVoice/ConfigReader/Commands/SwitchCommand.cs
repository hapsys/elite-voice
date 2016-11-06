using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteVoice.ConfigReader.Commands
{
	class SwitchCommand : AbstractCommand
	{

		public string select { get; private set; } = null;
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
		public override int runCommand(IDictionary<string, object> parameters)
		{
			select = null;
			if (selectParam != null && selectParam.Length > 0)
			{
				select = "";
				if (parameters.ContainsKey(selectParam))
				{
					select = (parameters[selectParam] != null)? parameters[selectParam].ToString() : "";
				}
				runChilds(parameters);
			}
			else
			{
				logger.log("Check \"select\" parameter at Switch command!");
			}
			return 0;
		}
	}
}
