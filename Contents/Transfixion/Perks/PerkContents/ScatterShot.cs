using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks;
using Terraria;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class ScatterShot : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<ScatterShot>();
		CanBeStack = false;
	}
	public override void ResetEffect(Player player) {
		player.GetModPlayer<PerkPlayer>().perk_ScatterShot = true;
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.minion || proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().IsFromMinion) {
			return;
		}
		modifiers.SourceDamage -= .33f;
	}
}
