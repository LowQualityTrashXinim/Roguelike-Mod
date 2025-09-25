using Terraria.ModLoader;

namespace Roguelike.Contents.Items.RelicItem.RelicSetContent;
internal class DamageUp_ModPlayer : ModPlayer {
	public class DamageUp : RelicSet {
		public override void SetStaticDefaults() {
			Requirement = 1;
		}
	}
	public bool set => RelicSetSystem.Check_RelicSetRequirment(Player, RelicSet.GetRelicSetType<DamageUp>());
	public override void UpdateEquips() {
		if (set) {
			Player.GetDamage(DamageClass.Generic) += .12f;
		}
	}
}
