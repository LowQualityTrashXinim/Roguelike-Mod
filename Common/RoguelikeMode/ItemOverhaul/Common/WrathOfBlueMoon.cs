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
internal class Roguelike_WrathOfBlueMoon : GlobalItem {
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		if (item.type == ItemID.Muramasa) {
			target.AddBuff<WrathOfBlueMoon>(ModUtils.ToSecond(3));
		}
	}
}
public class Roguelike_WrathOfBlueMoon_Projectile : GlobalProjectile {
	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.WrathOfBlueMoon].Contains(projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType)) {
			target.AddBuff<WrathOfBlueMoon>(ModUtils.ToSecond(5));
		}
	}
}
public class Roguelike_WrathOfBlueMoon_GlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int WrathOfBlueMoon = 0;
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		if (npc.HasBuff<WrathOfBlueMoon>()) {
			modifiers.SourceDamage += .01f * WrathOfBlueMoon;
		}
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		if (npc.HasBuff<WrathOfBlueMoon>()) {
			modifiers.SourceDamage += .01f * WrathOfBlueMoon;
		}
	}
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
		if (npc.HasBuff<WrathOfBlueMoon>()) {
			if (++WrathOfBlueMoon >= 20) {
				WrathOfBlueMoon = 20;
				if (Main.rand.NextBool(10)) {
					Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(item), npc.Center, Main.rand.NextVector2CircularEdge(1, 1), ModContent.ProjectileType<SimplePiercingProjectile2>(), 30 + (int)(npc.life * .01f), 0, player.whoAmI, 2, 30);
					if (proj.ModProjectile is SimplePiercingProjectile2 modproj) {
						modproj.ProjectileColor = Color.Blue;
						modproj.ScaleX = 5;
					}
				}
			}
		}
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		if (projectile.owner != Main.myPlayer) {
			return;
		}
		Player player = Main.player[projectile.owner];
		if (npc.HasBuff<WrathOfBlueMoon>()) {
			if (++WrathOfBlueMoon >= 20) {
				WrathOfBlueMoon = 20;
				if (Main.rand.NextBool(10)) {
					Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), npc.Center, Main.rand.NextVector2CircularEdge(1, 1), ModContent.ProjectileType<SimplePiercingProjectile2>(), 30 + (int)(npc.life * .01f), 0, player.whoAmI, 2, 30);
					if (proj.ModProjectile is SimplePiercingProjectile2 modproj) {
						modproj.ProjectileColor = Color.Blue;
						modproj.ScaleX = 5;
					}
				}
			}
		}
	}
}
internal class WrathOfBlueMoon : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.lifeRegen -= 15;
		if (npc.buffTime[buffIndex] <= 0) {
			npc.GetGlobalNPC<Roguelike_WrathOfBlueMoon_GlobalNPC>().WrathOfBlueMoon = 0;
		}
	}
}
