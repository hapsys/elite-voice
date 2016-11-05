using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeechLib;


namespace EliteVoice.ConfigReader.Commands
{
    class TextToSpeechCommand : AbstractCommand
    {
        public override int runCommand(IDictionary<string, Object> parameters)
        {
            Speech sp = Speech.instance;
            foreach (string key in getProperties().Keys)
            {
                string value = getProperties()[key];
                switch(key)
                {
                    case "voice":
                        ISpeechObjectToken voice = sp.getVoice(value);
                        if (voice != null)
                        {
                            sp.speech.Voice = (SpObjectToken)voice;
                        }
                        break;
                    case "volume":
                        int volume = Int32.Parse(value);
                        if (volume > -1 && volume < 101)
                        {
                            sp.speech.Volume = volume;
                        }
                        break;
                    case "rate":
                        int rate = Int32.Parse(value);
                        if (rate > -11 && rate < 11)
                        {
                            sp.speech.Rate = rate;
                        }
                        break;
                }
            }
            runChilds(parameters);
            return 0;
        }
    }
}
