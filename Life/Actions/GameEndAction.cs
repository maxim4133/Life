﻿using Life.Infrastructure;
using Life.Infrastructure.Common;
using System;

namespace Life.Actions
{
	public class GameEndAction : IUserAction
	{
		public string Description => "End the current game";
		public string Name => "End game";
		public string Command => "end";

		public GameEndAction(GameSettings gameSettings)
		{
			_gameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
		}

		public void Perform(CommandContext context)
		{
			_gameSettings.CurrentGame.InProgress = false;
		}

		private readonly GameSettings _gameSettings;
	}
}
