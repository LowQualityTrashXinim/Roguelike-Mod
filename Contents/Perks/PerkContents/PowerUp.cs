using Roguelike.Common.Utils;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Perks.PerkContents;
public class PowerUp : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<PowerUp>();
		CanBeStack = true;
		StackLimit = 3;
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		damage += .25f * StackAmount(player);
	}
	public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
		crit += 10 * StackAmount(player);
	}
	public override void ModifyUseSpeed(Player player, Item item, ref float useSpeed) {
		useSpeed -= .25f + .1f * StackAmount(player);
	}
}
