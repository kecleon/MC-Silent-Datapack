using System;
using System.IO;

namespace WetDreamPack
{
	public class Program
	{
		private const string CheatDatapackName = "datapack.zip";
		private static string _watchPath;
		private static FileSystemWatcher _watcher;
		private static byte[] _datapack;

		public static void Main()
		{
			_datapack = File.ReadAllBytes("datapack.zip");
			_watchPath = File.ReadAllText("savespath.txt").Replace("\r", "").Replace("\n", "");
			foreach (var c in Path.GetInvalidPathChars())
			{
				if (_watchPath.Contains(c))
				{
					Console.WriteLine($"World saves path contains invalid character {c}\n{_watchPath}");
				}
			}

			_watcher = new(_watchPath);
			
			_watcher.NotifyFilter = NotifyFilters.DirectoryName;
			_watcher.Created += WatcherOnCreated;
			_watcher.IncludeSubdirectories = true;
			_watcher.EnableRaisingEvents = true;

			Console.WriteLine("Watching!");
			Console.ReadLine();
		}

		private static void WatcherOnCreated(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType != WatcherChangeTypes.Created)
			{
				return;
			}

			if (Directory.GetParent(e.FullPath)?.FullName != _watchPath)
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
				File.WriteAllBytes(cheatDatapackPath, _datapack);
			}

			Console.WriteLine($"Added datapack to world {Path.GetDirectoryName(e.FullPath)}");
		}
	}
}