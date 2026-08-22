using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class ScatterShot : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<ScatterShot>();
		CanBeStack = true;
		StackLimit = 3;
	}
	public override string ModifyToolTip() {
		if(StackAmount(Main.LocalPlayer) >= 1) {
			return DescriptionIndex(1);
		}
		return base.ModifyToolTip();
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().ScatterShot += 2 + StackAmount(player) - 1;
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.minion || proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().IsFromMinion) {
			return;
		}
		modifiers.SourceDamage -= .33f;
	}
}
