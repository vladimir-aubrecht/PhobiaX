using PhobiaX.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.UserInterface
{
	public class GameUI
	{
		private readonly StaticGameObject map;
		private readonly TextGameObject player1Score;
		private readonly TextGameObject player2Score;
		private readonly TextGameObject player1Energy;
		private readonly TextGameObject player2Energy;

		private readonly IList<IGameObject> gameObjects;

		public GameUI(StaticGameObject map, TextGameObject player1Score, TextGameObject player2Score, TextGameObject player1Energy, TextGameObject player2Energy, params IGameObject[] otherGraphics)
		{
			this.map = map ?? throw new ArgumentNullException(nameof(map));
			this.player1Score = player1Score ?? throw new ArgumentNullException(nameof(player1Score));
			this.player2Score = player2Score ?? throw new ArgumentNullException(nameof(player2Score));
			this.player1Energy = player1Energy ?? throw new ArgumentNullException(nameof(player1Energy));
			this.player2Energy = player2Energy ?? throw new ArgumentNullException(nameof(player2Energy));
			_ = otherGraphics ?? throw new ArgumentNullException(nameof(otherGraphics));

			gameObjects = new List<IGameObject>();
			gameObjects.Add(map);
			gameObjects.Add(player1Score);
			gameObjects.Add(player2Score);
			gameObjects.Add(player1Energy);
			gameObjects.Add(player2Energy);

			foreach (var graphic in otherGraphics)
			{
				gameObjects.Add(graphic);
			}
		}

		public void SetPlayer1Score(int score)
		{
			player1Score.SetText($"P1 {score}");
		}

		public void SetPlayer2Score(int score)
		{
			player2Score.SetText($"P2 {score}");
		}

		public void SetPlayer1Life(int life)
		{
			player1Energy.SetText($"P1 {life}");
		}

		public void SetPlayer2Life(int life)
		{
			player2Energy.SetText($"P2 {life}");
		}

		public IGameObject GetMapGameObject()
		{
			return this.map;
		}

		public IList<IGameObject> GetGameObjects()
		{
			return gameObjects;
		}
	}
}
