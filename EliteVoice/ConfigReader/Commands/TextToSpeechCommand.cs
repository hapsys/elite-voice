using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SpeechLib;
using System.Speech;
using System.Speech.Synthesis;


namespace EliteVoice.ConfigReader.Commands
{
    class TextToSpeechCommand : AbstractCommand
    {
        public override int runCommand(XmlElement node)
        {
            Speech sp = Speech.instance;
            //SpeechSynthesizer sp = new SpeechSynthesizer();

            /*
			 * Store values
			 */
            SpObjectToken prevVoice = sp.speech.Voice;
			int prevVolume = sp.speech.Volume;
			int prevRate = sp.speech.Rate;


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
            runChilds(node);
			if (commands.Count > 0)
			{
				sp.speech.Voice = prevVoice;
				sp.speech.Volume = prevVolume;
				sp.speech.Rate = prevRate;
			}
			return 0;
        }
    }
}
