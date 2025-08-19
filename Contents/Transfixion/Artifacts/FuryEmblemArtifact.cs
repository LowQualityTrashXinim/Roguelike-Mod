using Terraria;
using Roguelike.Texture;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Systems.ArtifactSystem;

namespace Roguelike.Contents.Transfixion.Artifacts;
internal class FuryEmblemArtifact : Artifact {
	public override Color DisplayNameColor => Color.LightGoldenrodYellow;
	public override string TexturePath => ModTexture.Get_MissingTexture("Artifact");
}
class FuryEmblemPlayer : ModPlayer {
	public bool Furious = false;
	public override void ResetEffects() {
		Furious = Player.HasArtifact<FuryEmblemArtifact>();
	}
	public override void UpdateEquips() {
		var modplayer = Player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.05f);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 5);
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (Furious && !Player.HasBuff<FuriousCoolDown>()) Player.AddBuff(ModContent.BuffType<Furious>(), 600);
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (Furious && !Player.HasBuff<FuriousCoolDown>()) Player.AddBuff(ModContent.BuffType<Furious>(), 600);
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
		modplayer.AddStatsToPlayer(PlayerStats.Defense, -.5f);
		modplayer.AddStatsToPlayer(PlayerStats.RegenHP, -.25f);
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.25f);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 25);
		if (player.buffTime[buffIndex] == 0) {
			player.AddBuff(ModContent.BuffType<FuriousCoolDown>(), 420);
		}
	}
}
public class FuriousCoolDown : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
		Main.buffNoSave[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex) {
	}
}
