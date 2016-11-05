using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteVoice
{
    class FileDescription
    {
        public string name { set; get; }
        public string createDate { set; get; }

        public FileDescription(string name = "", string createDate = "")
        {
            this.name = name;
            this.createDate = createDate;
        }
    }
}
