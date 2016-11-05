using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;


namespace EliteVoice
{
    class TextLogger
    {
        public static TextLogger instance { get; } = new TextLogger();

        public TextBoxBase output { set; get; } = null;

        private TextLogger()
        {

        }

        public void log(string value)
        {
            if (output != null)
            {

                if (!output.CheckAccess())
                {
                    output.Dispatcher.Invoke(new Action<string>(log), value + "\n");
                }
                else
                {
                    output.AppendText(value);
                    output.AppendText("\n");
                }
            }
        }

    }
}
