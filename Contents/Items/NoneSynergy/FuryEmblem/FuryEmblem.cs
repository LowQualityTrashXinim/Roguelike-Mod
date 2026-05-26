using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.NoneSynergy.FuryEmblem;
internal class FuryEmblem : ModItem {
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<FuryEmblemPlayer>().Furious = true;
		var modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.05f);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 5);
	}
}
class FuryEmblemPlayer : ModPlayer {
	public bool Furious = false;
	public override void ResetEffects() {
		Furious = false;
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (Furious) Player.AddBuff(ModContent.BuffType<Furious>(), 600);
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (Furious) Player.AddBuff(ModContent.BuffType<Furious>(), 600);
	}
}
public class Furious : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = true; //Add this so the nurse doesn't remove the buff when healing
		Main.buffNoSave[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.Defense, -.15f);
		modplayer.AddStatsToPlayer(PlayerStats.RegenHP, -.05f);
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.25f);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 15);
	}
}
