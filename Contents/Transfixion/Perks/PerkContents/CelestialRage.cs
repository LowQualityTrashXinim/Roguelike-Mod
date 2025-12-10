using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class CelestialRage : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<CelestialRage>();
		CanBeStack = false;
	}
	public override void OnChoose(Player player) {
		player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<CelestialWrath>());
	}
	public override void UpdateEquip(Player player) {
		var modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 2f);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 5);
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, Multiplicative: 1.1f);
	}
}
