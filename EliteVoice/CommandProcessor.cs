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
using Saxon.Api;

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
			config.init.runCommand(null);
		}
        public void process(string jsonStr)
        {
            this.doNextLine = false;
            log.Debug(jsonStr);
            logger.log("Receive json: " + jsonStr);
            // Some test
            XmlDocument docXML = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonStr, "root");
            XdmNode doc = XMLContext.instance.processor.NewDocumentBuilder().Wrap(docXML);
            if (doc != null)
            {
                log.Debug(doc.OuterXml);

                XdmNode eventElement = (XdmNode)XMLContext.instance.xpath.EvaluateSingle("/*/event", doc);
                if (eventElement != null)
                {
                    string eventName = eventElement.GetStringValue();
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
                        command.runCommand(doc);
                    }
                    else
                    {
                        Boolean find = false;
                        foreach (string eventXpath in config.events.Keys)
                        {
                            XPathExpression exp = XMLContext.instance.getXPathExpression(eventXpath);
                            if (XMLContext.instance.xpath.EvaluateSingle(eventXpath, doc) != null)
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
                                    command.runCommand(doc);
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
