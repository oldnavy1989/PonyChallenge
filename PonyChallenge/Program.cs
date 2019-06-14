using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PonyChallenge
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

			var configuration = builder.Build();

			var mazeSettings = new MazeSettings(configuration.GetSection(nameof(MazeSettings)));

			var ponyChallenge = new PonyChallenge(mazeSettings);

			try
			{
				await ponyChallenge.StartAsync();
			}
			catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine(e.Message);
				Console.ReadLine();
			}
		}
	}
}
