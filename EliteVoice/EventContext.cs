using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EliteVoice.ConfigReader.Commands;

namespace EliteVoice
{
	class EventContext
	{
		public static EventContext instance { get; } = new EventContext();

		private List<PlayCommand> players = new List<PlayCommand>();

		public List<EliteVoice.ConfigReader.Replacer> replacers { get; } = new List<ConfigReader.Replacer>();

		public List<PlayCommand> getPlayersByName(string name = null)
		{
			List<PlayCommand> result = null;
			if (name != null)
			{
				result = new List<PlayCommand>();
				name = name.ToLower();
				foreach (PlayCommand player in players)
				{
					if (name.Equals(player.name))
					{
						result.Add(player);
					}
				}
			}
			else
			{
				result = players;
			}
			
			return result;
		}

		public void addPlayer(PlayCommand player)
		{
			players.Add(player);
		}
	}
}
