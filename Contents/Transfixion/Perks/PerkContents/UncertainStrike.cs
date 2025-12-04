using Roguelike.Common.Utils;
using Terraria;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class UncertainStrike : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<UncertainStrike>();
		CanBeChoosen = false;
		CanBeStack = false;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .33f) {
			modifiers.SourceDamage += Main.rand.NextFloat(-.15f, .55f);
		}
		if (Main.rand.NextFloat() <= .15f) {
			modifiers.ArmorPenetration += 20 * (!Main.rand.NextBool(4)).ToDirectionInt();
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .33f) {
			modifiers.SourceDamage += Main.rand.NextFloat(-.15f, .55f);
		}
		if (Main.rand.NextFloat() <= .15f) {
			modifiers.ArmorPenetration += 20 * (!Main.rand.NextBool(4)).ToDirectionInt();
		}
	}
}
