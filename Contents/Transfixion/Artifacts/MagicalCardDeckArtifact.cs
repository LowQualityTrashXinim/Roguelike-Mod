using Microsoft.Xna.Framework;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Artifacts
{
	internal class MagicalCardDeckArtifact : Artifact {
		public override int Frames => 10;
		public override Color DisplayNameColor => Color.DeepSkyBlue;
		public override bool CanBeSelected(Player player) {
			return false;
		}
	}

	class MagicalCardDeckPlayer : ModPlayer {
		public bool MagicalCardDeck = false;
		public override void ResetEffects() {
			MagicalCardDeck = Player.HasArtifact<MagicalCardDeckArtifact>();
		}
		public override void UpdateEquips() {
			if (MagicalCardDeck) {
			}
		}
	}
}
