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
		public string name { get; private set; } = null;
		IWavePlayer waveOutDevice = new WaveOut();
        AudioFileReader audioFileReader = null;
        bool async = false;
        public bool isOpen { get; private set; } = false;

		public int fadeMills { set; private get; } = 0;

        float volume = 0.5f;

        private void initializeParameters()
        {

			if (getProperties().ContainsKey("name"))
			{
				name = getProperties()["name"];
			}


			if (getProperties().ContainsKey("async"))
            {
                async = "true".Equals(getProperties()["async"]);
            }

            int volume = -1;
            if (getProperties().ContainsKey("volume"))
            {
                volume = Int32.Parse(getProperties()["volume"]);
                if (volume < 0 || volume > 100)
                {
                    volume = -1;
                }
            }
            if (getProperties().ContainsKey("file"))
            {
                string filename = getProperties()["file"];
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
                    logger.log("File NOT Found: \"" + filename + "\"!");
                }
            }

			EventContext.instance.addPlayer(this);

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
					//logger.log("Continue Playing");
				}
				//isOpen = false;
			}
			return 0;

        }

		public void fade()
		{

			//logger.log("Open state " + isOpen);

			if (isOpen)
			{
				float oldVolume = audioFileReader.Volume;
				if (fadeMills > 0)
				{
					int steps = 10;
					float volStep = oldVolume / steps;
					int sleep = 1 + fadeMills / steps;
					for (int i = 0; i < steps; i++)
					{
						audioFileReader.Volume -= volStep;
						Thread.Sleep(sleep);
					}
				}
				waveOutDevice.Stop();
				isOpen = false;
				audioFileReader.Volume = oldVolume;
			}
		}
    }
}
