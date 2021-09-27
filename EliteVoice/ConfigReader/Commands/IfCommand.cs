using System;
using System.Collections.Generic;
using System.Linq;
using Saxon.Api;


namespace EliteVoice.ConfigReader.Commands
{
    class IfCommand : AbstractCommand
    {
		public override int runCommand(XdmNode node)
        {

            if (getProperties().ContainsKey("select"))
            {
                string exp = getProperties()["select"];
                XdmItem child = XMLContext.instance.xpath.EvaluateSingle(exp, node);
                if (child.IsAtomic() && child.Matches(XdmAtomicType.BOOLEAN))
                {
                    if (((XdmAtomicValue)child).GetBooleanValue())
                    {
                        runChilds(node);
                    }
                    return 0;
                }
                else
                {
                    logger.log("Expression \"" + exp + "\" not a boolean!");
                    return -1;
                }
                //
            }
            else 
            {
                logger.log("Check \"test\" parameter at If command!");
                return -1;
            }

        }
    }
}
