using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EliteVoice.ConfigReader.Commands
{
	class StopCommand : AbstractCommand
	{
		private string name = null;
		private int fade = 0;
		public override void addProperty(string key, string value)
		{
			base.addProperty(key, value);
			switch (key)
			{
				case "name":
					name = value;
					break;
				case "fade":
					fade = Int32.Parse(value);
					if (fade < 0)
					{
						fade = 0;
					}
					break;
			}
		}
		public override int runCommand(IDictionary<string, object> parameters)
		{
			Thread thread = new Thread(new ThreadStart(this.runThreads));
			thread.Start();
			return 0;
		}

		private void runThreads()
		{
			List<PlayCommand> players = EventContext.instance.getPlayersByName(name);

			List<Thread> threads = new List<Thread>();

			foreach (PlayCommand player in players)
			{
				player.fadeMills = fade;
				Thread thread = new Thread(new ThreadStart(player.fade));
				threads.Add(thread);
				thread.Start();
			}

			bool allStoped = false;
			while (!allStoped)
			{
				allStoped = true;
				foreach (Thread thread in threads)
				{
					if (thread.IsAlive)
					{
						allStoped = false;
						break;
					}
				}
				if (!allStoped)
				{
					Thread.Sleep(100);
				}
			}

			logger.log("Stop Play Threads");

		}
	}
}
