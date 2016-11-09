using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EliteVoice.ConfigReader.Commands
{
	class ReplaceCommand: AbstractCommand
	{
		private string source = null;

		private string match = null;

		private string ignorecase = null;

		private string target = null;

		private string replace = null;
		public override void addProperty(string key, string value)
		{
			base.addProperty(key, value);
			switch (key)
			{
				case "source":
					source = value;
					break;
				case "match":
					match = value;
					break;
				case "ignorecase":
					ignorecase = value;
					break;
				case "replace":
					replace = value;
					break;
				case "target":
					target = value;
					break;
			}
		}

		public override int runCommand(IDictionary<string, object> parameters)
		{
			Replacer rp = new Replacer(match, replace, source, target, ignorecase);
			if (rp.isValid)
			{
				EventContext.instance.replacers.Add(rp);
			}
			return 0;
		}
	}
}
