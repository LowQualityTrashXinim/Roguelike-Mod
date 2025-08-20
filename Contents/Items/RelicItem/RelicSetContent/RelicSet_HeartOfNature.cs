using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.RelicItem.RelicSetContent;
public class HeartOfNature_ModPlayer : ModPlayer {
	class HeartOfNature : RelicSet {
		public override void SetStaticDefaults() {
			Requirement = 2;
		}
	}
	public bool set => RelicSetSystem.Check_RelicSetRequirment(Player, RelicSet.GetRelicSetType<HeartOfNature>());
	public override void UpdateEquips() {
		if(!set) {
			return;
		}
		Player.ModPlayerStats().UpdateHPMax.Base += 40;
		Player.ModPlayerStats().UpdateManaMax.Base += 40;
	}
	public void HealBaseOnDamage(int damage) {
		if (!set) {
			return;
		}
		int damageReal = (int)(damage * .25f);
		Player.Heal(damageReal);
		Player.statMana += Main.rand.Next(3, 10);
		if (Player.statMana > Player.statManaMax2) {
			Player.statMana = Player.statManaMax2;
		}
		Player.ManaEffect(damageReal);
	}
}
