using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeechLib;

namespace EliteVoice
{
    class Speech
    {

        public static Speech instance { get; } = new Speech();
        public SpVoice speech { get; } = new SpVoice();

        private ISpeechObjectTokens voices;

        private Speech()
        {
            voices = speech.GetVoices();
        }
        private void setDefaults()
        {
            //speech.Voice = 
        }

        public ISpeechObjectToken getVoice(string name)
        {
            ISpeechObjectToken result = null;

            foreach (ISpeechObjectToken voice in voices)
            {
                if (voice.GetDescription().Equals(name))
                {
                    result = voice;
                    break;
                }
            }

            return result;
        }

        public void speak(string text)
        {
            speech.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            speech.WaitUntilDone(-1);
        }
    }
}
