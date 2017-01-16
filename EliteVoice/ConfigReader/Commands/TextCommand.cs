using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeechLib;

namespace EliteVoice.ConfigReader.Commands
{
    class TextCommand : AbstractCommand
    {
		public override int runCommand(IDictionary<string, Object> parameters)
        {

            Speech sp = Speech.instance;
            string text = null;
            if (getProperties().ContainsKey("select"))
            {
                string parameter = getProperties()["select"];
                if (parameters.ContainsKey(parameter))
                {
                    text = string.Format("{0:#,##}",parameters[parameter]);
                }
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
