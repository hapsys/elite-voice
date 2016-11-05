using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
namespace EliteVoice.ConfigReader.Commands
{
    class PlaySoundCommand : AbstractCommand
    {
        private bool isOpen;
        static WMPLib.WindowsMediaPlayer wplayer;
        public override int runCommand(IDictionary<string, object> parameters)
        {
            bool async = false;
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
                    isOpen = true;
                    wplayer = new WMPLib.WindowsMediaPlayer();
                    int lastVolume = wplayer.settings.volume;
                    wplayer.PlayStateChange += Player_PlayStateChange;
                    wplayer.URL = filename;
                    if (volume > -1)
                    {
                        wplayer.settings.volume = volume;
                    }
                    wplayer.controls.play();

                    while (!async && isOpen)
                    {
                        Thread.Sleep(500);
                    }
                    //wplayer.settings.volume = lastVolume;
                    wplayer.settings.volume = 100;
                }
                else
                {
                    logger.log("File NOT Foud: \"" + filename + "\"!");
                }
            }
            return 0;
        }

        private void Player_PlayStateChange(int NewState)
        {
            if ((WMPLib.WMPPlayState)NewState == WMPLib.WMPPlayState.wmppsStopped)
            {
                isOpen = false;
            }
        }
    }
}
