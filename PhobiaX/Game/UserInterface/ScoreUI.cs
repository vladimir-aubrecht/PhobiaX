using PhobiaX.Game.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Game.UserInterface
{
	public class ScoreUI
	{
		private readonly IList<TextGameObject> playerScores;
		private readonly IList<TextGameObject> playerEnergies;

		public ScoreUI(IList<TextGameObject> playerScores, IList<TextGameObject> playerEnergies)
		{
			this.playerScores = playerScores ?? throw new ArgumentNullException(nameof(playerScores));
			this.playerEnergies = playerEnergies ?? throw new ArgumentNullException(nameof(playerEnergies));
		}

		public void SetPlayerScore(int index, int score)
		{
			playerScores[index].SetText($"P{index + 1} {score}");
		}

		public void SetPlayerLife(int index, int life)
		{
			playerEnergies[index].SetText($"P{index + 1} {life}");
		}
	}
}
