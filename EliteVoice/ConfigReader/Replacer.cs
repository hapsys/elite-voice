using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EliteVoice.ConfigReader
{
	class Replacer
	{
		TextLogger logger = TextLogger.instance;

		private Regex source = null;

		private Regex match = null;

		private string target = null;

		private string replace = null;

		public bool isValid { get; private set; } = false;
		public Replacer(string match, string replace, string source, string target, string ingnoreCase)
		{
			if (replace == null || match == null && source == null || match == null && source != null && target == null)
			{
				logger.log("Ignore one or more Replace commands... Not enough arguments!!!");
			} 
			else
			{
				isValid = true;

				this.target = target;
				this.replace = replace;

				if (source != null) { 
					try
					{
						this.source = new Regex(source, RegexOptions.IgnoreCase | RegexOptions.Compiled);
					}
					catch (ArgumentException e1)
					{
						this.source = null;
						isValid = false;
						logger.log("Error parsing regexp \"" + source + "\"");
					}
				}
				if (match != null)
				{
					try
					{
						RegexOptions options = !("false".Equals(ingnoreCase)) ? RegexOptions.IgnoreCase | RegexOptions.Compiled : RegexOptions.Compiled;
						this.match = new Regex(match, options);
					}
					catch (ArgumentException e1)
					{
						this.match = null;
						isValid = false;
						logger.log("Error parsing regexp \"" + source + "\"");
					}
				}
			}
		}


		public void Replace(IDictionary<string, object> parameters)
		{
			if (isValid) {
				List<string> keys = new List<string>(parameters.Keys);
				foreach (string key in keys)
				{
					string value = parameters[key].ToString();

					string restarget = (target == null) ? key : target;

					if (source == null || source.IsMatch(key))
					{
						if (match != null && match.IsMatch(value))
						{
							value = match.Replace(value, replace);
							parameters[restarget] = value;
						} else if (match == null)
						{
							parameters[restarget] = replace;
						}
					}
				}
			}
		}

		public string Replace(string text)
		{
			string result = text;
			if (isValid && match != null && source == null && target == null)
			{
				result = match.Replace(text, replace);
			}
			return result;
		}


	}
}
