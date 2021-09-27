using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace EliteVoice.ConfigReader.Commands
{
	class CaseCommand : AbstractCommand
	{
		private Regex reg = null;
		private string eq = null;
		private string ieq = null;
		private string test = null;
		public override void addProperty(string key, string value)
		{
			base.addProperty(key, value);
			switch (key)
			{
				case "equal":
				case "equals":
					eq = value;
					break;
				case "iequal":
				case "iequals":
					ieq = value.ToLower();
					break;
				case "imatch":
				case "match":
					try
					{
						RegexOptions options = (key.Equals("imatch")) ? RegexOptions.IgnoreCase | RegexOptions.Compiled : RegexOptions.Compiled;
						reg = new Regex(value, options);
					}
					catch (ArgumentException e1)
					{
						reg = null;
						logger.log("Error parsing regexp \"" + value + "\"");

					}
					break;
				case "test":
					test = value;
					break;
			}
		}
		public override int runCommand(XmlElement node)
		{
			int result = 0;

			if (parent.GetType() == typeof(SwitchCommand))
			{
				bool success = false;
				string select = node.InnerText;
				logger.log("Select has value \"" + select + "\"");
				if (reg != null)
				{
					success = reg.IsMatch(select);
				}
				else if (eq != null)
				{
					success = eq.Equals(select);
				}
				else if (ieq != null)
				{
					success = ieq.Equals(select.ToLower());
				}
				else if (test != null)
				{
					XPathNavigator navigator = node.CreateNavigator();
					success =  XMLContext.instance.EvaluateBoolean(XMLContext.instance.getXPathExpression(test), navigator);
				}

				if (success)
				{
					result = -1;
					runChilds(node);
				}
			}
			else
			{
				logger.log("Error! Case command no has parent Switch command!");
			}

			return result;
		}
	}
}
