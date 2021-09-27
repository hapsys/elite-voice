using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace EliteVoice
{
    class FileProcessor
    {
        private static string homeDir = Environment.ExpandEnvironmentVariables("%USERPROFILE%") + "\\Saved Games\\Frontier Developments\\Elite Dangerous\\";
        private List<FileDescription> files = null;
        private StreamReader reader = null;
        private CommandProcessor processor = null;
        TextLogger logger = TextLogger.instance;

        public FileProcessor(List<FileDescription> files, CommandProcessor processor)
        {
            this.files = files;
            this.processor = processor;
        }

        public void directoryRead()
        {
            try { 
				Regex reg = new Regex("^.*/([^/]+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
				int lastCount = 0;
				while(true) {
					string[] filenames = Directory.GetFiles(homeDir, "*.log");
					if (filenames.Length != lastCount)
					{
						files.Clear();
						List<FileDescription> filesTmp = new List<FileDescription>();
						foreach (string filename in filenames)
						{
							filesTmp.Add(new FileDescription(reg.Replace(filename.Replace('\\', '/'), "$1"), File.GetCreationTime(filename).ToString()));
						}
						lastCount = filenames.Length;
						filesTmp.Sort(compareByCreateTime);
						filesTmp.ForEach(delegate (FileDescription fd) {
							files.Add(fd);
						});
						/*
						 * get reader
						 */
						bool skip = reader == null;
						reader = null;
						string fn = homeDir + files[files.Count - 1].name;
						logger.log("Using journal file: " + fn);
						FileStream fs = new FileStream(fn, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
						reader = new StreamReader(fs, System.Text.Encoding.UTF8);
						if (skip)
						{
							reader.ReadToEnd();
						}

					}
					Thread.Sleep(1000);
				}
            }
            catch (Exception e1)
            {
            }
        }

        private static int compareByCreateTime(FileDescription a, FileDescription b)
        {
            return File.GetCreationTime(homeDir + a.name).CompareTo(File.GetCreationTime(homeDir + b.name));
            //return a.createDate.CompareTo(b.createDate);
        }

        public void processCurrentFile()
        {
            try
            {
                while (true)
            {
                while (reader == null)
                {
                    Thread.Sleep(500);
                }

                string line = null;

                while((line = reader.ReadLine()) != null)
                {
                    logger.log("Read event line from journal file.");
                    processor.process(line);
                }

                while (!processor.doNextLine)
                    {
                        Thread.Sleep(500);
                    }
                }
            }
            catch (Exception e1)
            {
            }

        }
    }
}
