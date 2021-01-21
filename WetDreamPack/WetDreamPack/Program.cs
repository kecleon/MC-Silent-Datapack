using System;
using System.IO;

namespace WetDreamPack
{
	public class Program
	{
		private const string CheatDatapackName = "datapack.zip";
		private const string WatchPath = @"C:\Users\YourName\Documents\MultiMC\instances\1.16.1 cheat\.minecraft\saves";
		private static readonly FileSystemWatcher Watcher = new(WatchPath);

		public static void Main()
		{
			Watcher.NotifyFilter = NotifyFilters.DirectoryName;
			Watcher.Created += WatcherOnCreated;
			Watcher.IncludeSubdirectories = true;
			Watcher.EnableRaisingEvents = true;

			Console.WriteLine("Watching");
			Console.ReadLine();
		}

		private static void WatcherOnCreated(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType != WatcherChangeTypes.Created)
			{
				return;
			}

			Console.WriteLine($"new dir {e.FullPath}");

			if (Directory.GetParent(e.FullPath)?.FullName != WatchPath)
			{
				return;
			}

			var datapackDir = Path.Combine(e.FullPath, "datapacks");
			if (!Directory.Exists(datapackDir))
			{
				Directory.CreateDirectory(datapackDir);
			}

			var cheatDatapackPath = Path.Combine(e.FullPath, "datapacks", CheatDatapackName);
			if (!File.Exists(cheatDatapackPath))
			{
				File.Copy(CheatDatapackName, cheatDatapackPath);
			}
			
			Console.WriteLine("forsenCD DATAPACK.EXE ENABLED");
		}
	}
}