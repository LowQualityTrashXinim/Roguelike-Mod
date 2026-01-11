using Terraria;
using Terraria.ModLoader;
using Roguelike.Common.Utils;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.Mechanic.OutroEffect.Contents;
internal class OutroEffect_LesserDamage : WeaponEffect {
	public override void SetStaticDefaults() {
		Duration = ModUtils.ToSecond(20);
	}
	public override void WeaponDamage(Player player, Item item, ref StatModifier damage) {
		damage += .075f;
	}
}
