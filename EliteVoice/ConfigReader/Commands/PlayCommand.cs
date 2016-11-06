using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using NAudio;
using NAudio.Wave;

namespace EliteVoice.ConfigReader.Commands
{
    class PlayCommand : AbstractCommand
    {
        IWavePlayer waveOutDevice = new WaveOut();
        AudioFileReader audioFileReader = null;
        bool async = false;
        bool isOpen = false;

        float volume = 0.5f;

        private void initializeParameters()
        {
            if (getProperties().ContainsKey("async"))
            {
                async = "true".Equals((string)getProperties()["async"]);
            }

            int volume = -1;
            if (getProperties().ContainsKey("volume"))
            {
                volume = Int32.Parse((string)getProperties()["volume"]);
                if (volume < 0 || volume > 100)
                {
                    volume = -1;
                }
            }
            if (getProperties().ContainsKey("file"))
            {
                string filename = (string)getProperties()["file"];
                logger.log("Try to play file: \"" + filename + "\"");
                if (File.Exists(filename))
                {
                    audioFileReader = new AudioFileReader(filename);
                    if (volume > -1)
                    {
                        this.volume = (float)(volume / 100.0f);
                        audioFileReader.Volume = this.volume;
                    }
                    waveOutDevice.Init(audioFileReader);
                    waveOutDevice.PlaybackStopped += new EventHandler<StoppedEventArgs>(Player_PlayStateChange);
                }
                else
                {
                    logger.log("File NOT Foud: \"" + filename + "\"!");
                }
            }

        }

        private void Player_PlayStateChange(object sender, StoppedEventArgs e)
        {
            //logger.log("Stop Playing");
            isOpen = false;
        }

        public override int runCommand(IDictionary<string, object> parameters)
        {

            if (audioFileReader == null)
            {
                initializeParameters();
            }
			else
            {
                audioFileReader.Position = 0;
            }

			if (audioFileReader != null) { 

				isOpen = true;
				waveOutDevice.Play();

				while (!async && isOpen)
				{
					Thread.Sleep(500);
					//logger.log("Continute Playing");
				}
				isOpen = false;
			}
			return 0;

        }
    }
}
