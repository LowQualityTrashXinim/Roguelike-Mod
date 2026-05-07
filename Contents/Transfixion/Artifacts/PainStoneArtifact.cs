using Microsoft.Xna.Framework;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Artifacts;
internal class PainStoneArtifact : Artifact {
	public override Color DisplayNameColor => Color.Gray;
}
public class PainStonePlayer : ModPlayer {
	public bool PainStone = false;
	public override void ResetEffects() {
		PainStone = Player.HasArtifact<PainStoneArtifact>();
	}
	public override void UpdateEquips() {
		if (PainStone) {
			Player.ModPlayerStats().PercentageDamage += .01f;
			Player.ModPlayerStats().CappedHealthAmount = 50;
			if (Player.HeldItem.IsAWeapon()) {
				Player.HeldItem.useTime = Player.HeldItem.useAnimation = 120;
			}
		}
	}
}
