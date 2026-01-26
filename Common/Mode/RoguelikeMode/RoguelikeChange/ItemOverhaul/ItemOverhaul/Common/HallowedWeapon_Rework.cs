using Roguelike.Common.Global;
using Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.Mechanic.OutroEffect;
using Roguelike.Common.Utils;
using Roguelike.Contents.BuffAndDebuff;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Common;
internal class Roguelike_HallowedWeapon : GlobalItem {
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		if (item.type == ItemID.Excalibur || item.type == ItemID.TrueExcalibur) {
			target.AddBuff<HallowedGaze>(ModUtils.ToSecond(3));
		}
	}
}
public class Roguelike_HallowedWeapon_Projectile : GlobalProjectile {
	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.HallowedGaze].Contains(projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType)) {
			target.AddBuff<HallowedGaze>(ModUtils.ToSecond(5));
		}
	}
}
