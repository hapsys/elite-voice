using System;
using System.Collections.Generic;
using System.Linq;
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


namespace EliteVoice
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Speech speech;
        private ISpeechObjectTokens audios;
        private Thread tProcessor;
        private Thread lProcessor;
        public MainWindow()
        {
            InitializeComponent();

            TextLogger logger = TextLogger.instance;
            logger.output = logTextBox;

            speech = Speech.instance;
            audios = speech.speech.GetAudioOutputs();
            int idx = 0, i = 0;
            foreach (SpObjectToken audio in audios)
            {
                audioDevices.Items.Add(audio.GetDescription());
                if (audio.Equals(speech.speech.AudioOutput))
                {
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
            /*
                * 
                */
            EliteVoice.ConfigReader.ConfigReader config = new EliteVoice.ConfigReader.ConfigReader("config/config.xml");
            config.parse();
            /*
             * 
             */
            CommandProcessor commands = new CommandProcessor(config);
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
