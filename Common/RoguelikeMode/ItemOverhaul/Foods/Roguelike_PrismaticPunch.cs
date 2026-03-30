using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Roguelike.Common.Global;
using Microsoft.Xna.Framework;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Foods;
internal class Roguelike_PrismaticPunch : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.PrismaticPunch;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(6);
	public override int ManaAmount() => 305;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(3.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_PrismaticPunch_ModBuff>(), ModUtils.ToMinute(24));
	}
	public override void OnConsumeFood(Item item, Player player) {
		Player_FoodPlayer(player).SetFoodBuff(item.type, 1);
	}
}
public class Roguelike_PrismaticPunch_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.PrismaticPunch;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.Iframe.Base += ModUtils.ToSecond(.43f);
		handler.UpdateDefenseBase.Base += 9;
		player.GetDamage(DamageClass.Magic) += .18f;
		player.manaCost -= .27f;
		player.GetModPlayer<Roguelike_PrismaticPunch_ModPlayer>().PrismaticPunch = true;
	}
}
public class Roguelike_PrismaticPunch_ModPlayer : ModPlayer {
	public bool PrismaticPunch = false;
	public int CoolDown = 0;
	public override void ResetEffects() {
		PrismaticPunch = false;
		CoolDown = ModUtils.CountDown(CoolDown);
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (PrismaticPunch && CoolDown <= 0) {
			modifiers.SourceDamage -= .6f;
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (PrismaticPunch && CoolDown <= 0) {
			CoolDown = ModUtils.ToSecond(5);
			for (int i = 0; i < 5; i++) {
				Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center,
					Vector2.One.Vector2DistributeEvenlyPlus(5, 360, i) * 5,
					ProjectileID.NebulaBlaze2, 150 + (int)(hurtInfo.Damage * .1f), 5, Player.whoAmI);
			}
			Player.StrikeNPCDirect(npc, npc.CalculateHitInfo(hurtInfo.Damage, hurtInfo.HitDirection * -1));
		}
	}
}
