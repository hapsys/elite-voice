﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
		public override int runCommand(XmlElement node)
		{
			if (selectParam != null && selectParam.Length > 0)
			{
				/*
				select = "";
				if (parameters.ContainsKey(selectParam))
				{
					select = (parameters[selectParam] != null)? parameters[selectParam].ToString() : "";
				}
				runChilds(parameters);
				*/
				XmlElement selectNode = (XmlElement)node.SelectSingleNode(selectParam);
				if (selectNode != null)
                {
					runChilds(selectNode);
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
