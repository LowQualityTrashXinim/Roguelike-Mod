using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.RelicItem.RelicSetContent;
internal class RelicSet_HealingStrike_ModPlayer : ModPlayer {
	class RelicSet_HealingStrike : RelicSet {
		public override void SetStaticDefaults() {
			Requirement = 2;
		}
	}
	public bool HealingStrike => RelicSetSystem.Check_RelicSetRequirment(Player, RelicSet.GetRelicSetType<RelicSet_HealingStrike>());
	public int Counter = 0;
	public override void ResetEffects() {
		if (--Counter <= 0) {
			Counter = 0;
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!HealingStrike) {
			return;
		}
		if (Counter <= 0 && Main.rand.NextBool()) {
			Player.Heal(Main.rand.Next(1, 51));
			Counter = 120;
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!HealingStrike || proj.minion) {
			return;
		}
		if (Counter <= 0 && Main.rand.NextBool()) {
			Player.Heal(Main.rand.Next(1, 51));
			Counter = 120;
		}
	}
}
