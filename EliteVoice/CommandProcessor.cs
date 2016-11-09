using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EliteVoice.ConfigReader.Commands;
using EliteVoice.ConfigReader;
using System.Collections;
using Newtonsoft.Json;

namespace EliteVoice
{
    class CommandProcessor
    {
        //private JavaScriptSerializer json = new JavaScriptSerializer();

        TextLogger logger = TextLogger.instance;
        EliteVoice.ConfigReader.ConfigReader config = null;

        public CommandProcessor(EliteVoice.ConfigReader.ConfigReader config)
        {
            this.config = config;
			init();
        }

		public void init()
		{
			config.init.runCommand(new Dictionary<string, Object>());
		}
        public void process(string jsonStr)
        {
            logger.log("Receive json: " + jsonStr);
            IDictionary<string, Object> values = JsonConvert.DeserializeObject<Dictionary<string, Object>>(jsonStr);
            if (values.ContainsKey("event")) {
                string eventName = (string)values["event"];
                ICommand command = config.getEvent(eventName);
                if (command != null)
                {
					
					logger.log("Command successfully found for event: " + eventName);
					try {
						foreach (Replacer rp in EventContext.instance.replacers)
						{
							rp.Replace(values);
						}
					} catch (Exception e)
					{
						logger.log("Replace error result: " + e.Message);
					}
					command.runCommand(values);
                } else
                {
                    logger.log("No command found for event: " + eventName);
                }
        }
            else
            {
                logger.log("No event found!!!");
            }
        }
    }
}
