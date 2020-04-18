using PhobiaX.Game.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Game.UserInterface
{
	public class GameUI
	{
		private readonly TextGameObject player1Score;
		private readonly TextGameObject player2Score;
		private readonly TextGameObject player1Energy;
		private readonly TextGameObject player2Energy;

		private readonly IList<IGameObject> gameObjects;

		public GameUI(TextGameObject player1Score, TextGameObject player2Score, TextGameObject player1Energy, TextGameObject player2Energy, params IGameObject[] otherGraphics)
		{
			this.player1Score = player1Score ?? throw new ArgumentNullException(nameof(player1Score));
			this.player2Score = player2Score ?? throw new ArgumentNullException(nameof(player2Score));
			this.player1Energy = player1Energy ?? throw new ArgumentNullException(nameof(player1Energy));
			this.player2Energy = player2Energy ?? throw new ArgumentNullException(nameof(player2Energy));
			_ = otherGraphics ?? throw new ArgumentNullException(nameof(otherGraphics));

			gameObjects = new List<IGameObject>();
			gameObjects.Add(player1Score);
			gameObjects.Add(player2Score);
			gameObjects.Add(player1Energy);
			gameObjects.Add(player2Energy);

			foreach (var graphic in otherGraphics)
			{
				gameObjects.Add(graphic);
			}
		}

		public void SetPlayerScore(int index, int score)
		{
			if (index == 0)
			{
				player1Score.SetText($"P{index + 1} {score}");
			}
			else if (index == 1)
			{
				player2Score.SetText($"P{index + 1} {score}");
			}
		}

		public void SetPlayerLife(int index, int life)
		{
			if (index == 0)
			{
				player1Energy.SetText($"P{index + 1} {life}");
			}
			else if (index == 1)
			{
				player2Energy.SetText($"P{index + 1} {life}");
			}
		}

		public IList<IGameObject> GetGameObjects()
		{
			return gameObjects;
		}
	}
}
