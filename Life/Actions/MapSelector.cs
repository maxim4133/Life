﻿using Life.Infrastructure;
using Life.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Life.Actions
{
	public class MapSelector
	{
		public MapSelector(
			AppSettings appSettings,
			IFileProvider fileProvider,
			IUserInterface userInterface)
		{
			_directories = appSettings?.DirectoriesWithMaps?.ToArray()
				?? throw new ArgumentException("Directories with maps cannot be skipped", nameof(appSettings));
			_fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
			_userInterface = userInterface ?? throw new ArgumentNullException(nameof(userInterface));
		}

		public string[] Select()
		{
			var files = GetFiles(_directories);
			var fileName = AskUserToSelectFile(files);
			return _fileProvider.ReadFile(fileName);
		}

		private readonly string[] _directories;
		private readonly IFileProvider _fileProvider;
		private readonly IUserInterface _userInterface;

		private FileInfo[] GetFiles(IEnumerable<string> directories)
		{
			return directories.SelectMany(directoryPath =>
					_fileProvider.GetFiles(directoryPath, "*.txt", true))
				.Select((filePath, index) => new FileInfo
				{
					Index = index,
					DirectoryName = _fileProvider.GetDirectoryName(filePath),
					FullPath = filePath
				})
				.ToArray();
		}

		private string AskUserToSelectFile(FileInfo[] files)
		{
			var directories = files.GroupBy(file => file.DirectoryName);

			_userInterface.ClearScreen();
			foreach (var directory in directories)
			{
				_userInterface.Output.WriteLine(directory.Key);
				foreach (var file in directory)
					_userInterface.Output.WriteLine(
						$"{file.Index}. {_fileProvider.GetFileName(file.FullPath)}");
			}

			_userInterface.Output.Write("Select number of file: ");
			var number = int.Parse(_userInterface.Input.ReadLine());

			return files
				.Where(file => file.Index == number)
				.First().FullPath;
		}

		private struct FileInfo
		{
			public int Index;
			public string DirectoryName;
			public string FullPath;
		}
	}
}
