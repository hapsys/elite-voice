using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EliteVoice.ConfigReader.Commands;
using EliteVoice.ConfigReader;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using Newtonsoft.Json;
using log4net;

namespace EliteVoice
{
    class CommandProcessor
    {
        //private JavaScriptSerializer json = new JavaScriptSerializer();
        private static readonly ILog log = LogManager.GetLogger(typeof(CommandProcessor));
        private Regex regEvent = new Regex("^.*\"event\"\\:\"([^\"]+)\".*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);


        public bool doNextLine { get; set; } = true;

        TextLogger logger = TextLogger.instance;
        EliteVoice.ConfigReader.ConfigReader config = null;

        public CommandProcessor(EliteVoice.ConfigReader.ConfigReader config)
        {
            this.config = config;
			init();
        }

		public void init()
		{
			config.init.runCommand(new XmlDocument().DocumentElement);
		}
        public void processOld(string jsonStr)
        {
            log.Debug(jsonStr);
            // Some test
            XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonStr, "root");
            if (doc != null)
            {
                XMLContext.instance.setDocument(doc);
            }
            log.Debug(doc.OuterXml);
            //
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
					//command.runCommand(values);
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
        public void process(string jsonStr)
        {
            this.doNextLine = false;
            log.Debug(jsonStr);
            logger.log("Receive json: " + jsonStr);
            // Some test
            XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonStr, "root");
            if (doc != null)
            {
                log.Debug(doc.OuterXml);

                XmlElement eventElement = (XmlElement)doc.DocumentElement.SelectSingleNode("event");
                if (eventElement != null)
                {
                    string eventName = eventElement.InnerText;
                    ICommand command = config.getEvent(eventName);
                    if (command != null)
                    {

                        logger.log("Command successfully found for event: " + eventName);
                        try
                        {
                            foreach (Replacer rp in EventContext.instance.replacers)
                            {
                                //rp.Replace(values);
                            }
                        }
                        catch (Exception e)
                        {
                            logger.log("Replace error result: " + e.Message);
                        }
                        command.runCommand(doc.DocumentElement);
                    }
                    else
                    {
                        XPathNavigator navigator = doc.DocumentElement.CreateNavigator();
                        Boolean find = false;
                        foreach (string eventXpath in config.events.Keys)
                        {
                            XPathExpression exp = XMLContext.instance.getXPathExpression(eventXpath);
                            if (XMLContext.instance.EvaluateBoolean(exp, navigator))
                            {

                                command = config.getEvent(eventXpath);
                                if (command != null)
                                {
                                    logger.log("Command successfully found for event: " + eventName);
                                    try
                                    {
                                        foreach (Replacer rp in EventContext.instance.replacers)
                                        {
                                            //rp.Replace(values);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        logger.log("Replace error result: " + e.Message);
                                    }
                                    command.runCommand(doc.DocumentElement);
                                }
                                find = true;
                                break;
                            }
                        }
                        if (!find)
                        {
                            logger.log("No command found for event: " + eventName);
                        }
                    }
                }
                else
                {
                    log.Error("Error parsing to XML!!!");
                }
            }
            this.doNextLine = true;
        }
    }
}
