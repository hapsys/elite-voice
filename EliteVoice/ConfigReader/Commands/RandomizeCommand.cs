using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EliteVoice.ConfigReader.Commands
{
    class RandomizeCommand : AbstractCommand
    {
        public override int runCommand(XmlElement node)
        {
            LinkedList<ICommand> childs = getChilds();
            LinkedList<int> priority = new LinkedList<int>();
            int scale = 0;
            foreach (ICommand child in childs)
            {
                if (child.getProperties().ContainsKey("priority"))
                {
                    int pr = Int32.Parse(child.getProperties()["priority"]);
                    if (pr < 1)
                    {
                        pr = 1;
                    }
                    scale += pr;
                }
                else
                {
                    scale += 1;
                }
                priority.AddLast(scale);
            }

            Random rand = new Random();

            int check = rand.Next(scale);
            int idx = 0;
            foreach (int pr in priority)
            {
                if (pr > check)
                {
                    break;
                }
                idx++;
            }

            childs.ElementAt(idx).runCommand(node);

            return 0;
        }
    }
}
