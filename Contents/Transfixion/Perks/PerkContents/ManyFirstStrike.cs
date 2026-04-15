using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class ManyFirstStrike : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
		textureString = ModUtils.GetTheSameTextureAsEntity<ManyFirstStrike>();
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateFullHPDamage *= .33f;
		handler.HitCountIgnore += StackAmount(player);
	}
}
