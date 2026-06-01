using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Global.Mechanic.OutroEffect;
using Roguelike.Common.Utils;
using Roguelike.Contents.BuffAndDebuff;
using Roguelike.Contents.Projectiles;
using Roguelike.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Common;
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
public class Roguelike_HallowedGaze_GlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int HallowedGaze_Count = 0;
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		if (npc.HasBuff<HallowedGaze>()) {
			modifiers.SourceDamage += .05f * HallowedGaze_Count;
		}
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		if (npc.HasBuff<HallowedGaze>()) {
			modifiers.SourceDamage += .05f * HallowedGaze_Count;
		}
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		if (projectile.owner != Main.myPlayer) {
			return;
		}
		Player player = Main.player[projectile.owner];
		if (npc.HasBuff<HallowedGaze>()) {
			if (HallowedGaze_Count >= 12) {
				Vector2 playerPos = player.Center;
				Vector2 pos = new Vector2(npc.Center.X + Main.rand.Next(-100, 100), playerPos.Y - 800);
				Projectile.NewProjectile(projectile.GetSource_FromAI(), pos, (npc.Center - pos), ModContent.ProjectileType<HitScanShotv2>(), 1, 0, player.whoAmI);
			}
		}
	}
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
		if (npc.HasBuff<HallowedGaze>()) {
			if (HallowedGaze_Count >= 12) {
				Vector2 playerPos = player.Center;
				Vector2 pos = new Vector2(npc.Center.X + Main.rand.Next(-100, 100), playerPos.Y - 800);
				Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, (npc.Center - pos), ModContent.ProjectileType<HitScanShotv2>(), 1, 0, player.whoAmI);
			}
		}
	}
}
internal class HallowedGaze : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override bool ReApply(NPC npc, int time, int buffIndex) {
		npc.buffTime[buffIndex] = time;
		npc.GetGlobalNPC<Roguelike_HallowedGaze_GlobalNPC>().HallowedGaze_Count++;
		return base.ReApply(npc, time, buffIndex);
	}
}
