using System;
using Microsoft.Extensions.Configuration;

namespace PonyChallenge
{
	public class MazeSettings
	{
		public MazeSettings(IConfigurationSection configurationSection)
		{
			Difficulty = Convert.ToInt32(configurationSection[nameof(Difficulty)]);
			Width = Convert.ToInt32(configurationSection[nameof(Width)]);
			Height = Convert.ToInt32(configurationSection[nameof(Height)]);
			PonyName = configurationSection[nameof(PonyName)];
		}

		public int Difficulty { get;  }
		public int Width { get;  }
		public int Height { get;  }
		public string PonyName { get;  }
	}
}
