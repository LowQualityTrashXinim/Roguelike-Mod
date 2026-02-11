using Roguelike.Common.Global;
using Roguelike.Common.Global.Mechanic.OutroEffect;
using Roguelike.Common.Utils;
using Roguelike.Contents.BuffAndDebuff;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Common;
internal class Roguelike_FuryOfTheSun: GlobalItem {
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		if (item.type == ItemID.FieryGreatsword) {
			target.AddBuff<FuryOfTheSun>(ModUtils.ToSecond(3));
		}
	}
}
public class Roguelike_FuryOfTheSun_Projectile : GlobalProjectile {
	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.FuryOfTheSun].Contains(projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType)) {
			target.AddBuff<FuryOfTheSun>(ModUtils.ToSecond(5));
		}
	}
}
