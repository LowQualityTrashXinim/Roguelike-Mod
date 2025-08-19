using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.RelicItem.RelicSetContent;
internal class TitanicBlood_ModPlayer : ModPlayer {
	class TitanicBlood : RelicSet {
		public override void SetStaticDefaults() {
			Requirement = 5;
		}
	}
	public bool set => RelicSetSystem.Check_RelicSetRequirment(Player, RelicSet.GetRelicSetType<TitanicBlood>());
	public override void UpdateEquips() {
		if (!set) {
			return;
		}
		float damagereduction = 0;
		if (!Player.IsHealthAbovePercentage(.5f)) {
			damagereduction += .1f;
		}
		if (!Player.IsHealthAbovePercentage(.35f)) {
			damagereduction += .05f;
		}
		if (!Player.IsHealthAbovePercentage(.15f)) {
			damagereduction += .05f;
		}
		Player.endurance += damagereduction;
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		TitanBlock(ref modifiers);
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		TitanBlock(ref modifiers);
	}
	public void TitanBlock(ref Player.HurtModifiers hurtInfo) {
		if (Main.rand.NextBool(12) && set) {
			hurtInfo.SourceDamage *= 0;
			Player.AddImmuneTime(hurtInfo.CooldownCounter, 60);
		}
	}
}
