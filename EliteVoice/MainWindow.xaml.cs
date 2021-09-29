using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SpeechLib;
using EliteVoice.ConfigReader;
using NAudio;
using log4net;
using log4net.Config;


namespace EliteVoice
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Speech speech;
        private ISpeechObjectTokens audios;
        private CommandProcessor commands;
        private Thread tProcessor;
        private Thread lProcessor;
        public MainWindow()
        {
            InitializeComponent();


            TextLogger logger = TextLogger.instance;
            logger.output = logTextBox;

            /**
             * Initialize log4net
             */
            string log4netPath = "log4net/log4net.config";
            if (File.Exists(log4netPath))
            {
                XmlConfigurator.Configure(new FileInfo(log4netPath));
            } else if (File.Exists(Environment.ExpandEnvironmentVariables("%APPDATA%") + "/EliteVoice/" + log4netPath))
            {
                XmlConfigurator.Configure(new FileInfo(Environment.ExpandEnvironmentVariables("%APPDATA%") + "/EliteVoice/" + log4netPath));
            } else
            {
                logger.log("Not found log4net configuration. See documentation.");
            }

            var enumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();
            var dev = enumerator.GetDefaultAudioEndpoint(NAudio.CoreAudioApi.DataFlow.Render, NAudio.CoreAudioApi.Role.Console);
            //logger.log(dev.DeviceFriendlyName);
            //logger.log(dev.FriendlyName);

            speech = Speech.instance;
            //speech.speech.
            audios = speech.speech.GetAudioOutputs();
            int idx = 0, i = 0;
            foreach (SpObjectToken audio in audios)
            {
                audioDevices.Items.Add(audio.GetDescription());
                //if (audio.GetDescription().Equals(speech.speech.AudioOutput.GetDescription()))
                if (audio.GetDescription().Equals(dev.FriendlyName))
                {
                    speech.speech.AudioOutput = audio;
                    idx = i;
                }
                i++;
            }
            audioDevices.SelectedIndex = idx;
            /*
             * 
             */
            logger.log("Found voices:");
            foreach (ISpeechObjectToken voice in speech.speech.GetVoices())
            {
                logger.log(voice.GetDescription());
            }
            /**
             * 
            */
            EliteVoice.ConfigReader.ConfigReader config = null;
            string configPath = "config/config.xml";
            if (File.Exists(configPath))
            {
                config = new EliteVoice.ConfigReader.ConfigReader(configPath);
            } else if (File.Exists(Environment.ExpandEnvironmentVariables("%APPDATA%") + "/EliteVoice/" + configPath))
            {
                config = new EliteVoice.ConfigReader.ConfigReader(Environment.ExpandEnvironmentVariables("%APPDATA%") + "/EliteVoice/" + configPath);
            } else
            {
                logger.log("Not found configuration!!!");
                return;
            }

            config.parse();
            /*
             * 
             */
            commands = new CommandProcessor(config);
            List<FileDescription> files = new List<FileDescription>();
            fileGrid.ItemsSource = files;
            FileProcessor processor = new FileProcessor(files, commands);
            tProcessor = new Thread(new ThreadStart(processor.directoryRead));
            tProcessor.Start();
            lProcessor = new Thread(new ThreadStart(processor.processCurrentFile));
            lProcessor.Start();
        }
        private void audioDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (audioDevices.SelectedIndex > -1)
            {
                speech.speech.AudioOutput = audios.Item(audioDevices.SelectedIndex);
            }
        }


        private void fileGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            fileGrid.SelectedIndex = fileGrid.Items.Count - 1;
            logTextBox.AppendText("Here\n");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            commands.doNextLine = false;
            try
            {
                tProcessor.Interrupt();
            }
            catch (Exception e1) {
            }
            try
            {
                lProcessor.Interrupt();
            }
            catch (Exception e2)
            {
            }
        }

        private void logTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            logTextBox.ScrollToEnd();
        }
    }

}
