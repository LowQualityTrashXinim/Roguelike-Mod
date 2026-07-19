using Roguelike.Common.Global;
using Roguelike.Common.Global.Mechanic.Revive;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Weapon.MeleeSynergyWeapon.RelentlessAbomination;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.RoguelikeMode.ItemOverhaul.Specific;
internal class ZombieArm_Rework : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.ZombieArm;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 33;
	}
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		player.Heal(Main.rand.Next(1, 4));
		target.AddBuff<RA_Rotting>(ModUtils.ToSecond(3));
	}
}
public class ZombieArm_Rework_Revive : ModRevive {
	public override bool ReviveCondition(Player player) {
		return player.HeldItem.type == ItemID.ZombieArm;
	}
	public override void OnRevive(Player player, double damage, int hitDirection, bool pvp, ref PlayerDeathReason damageSource) {
		player.Heal(player.statLifeMax2);
		player.AddImmuneTime(-1, 120);
		player.immune = true;
		if (player.HeldItem.type == ItemID.ZombieArm) {
			player.HeldItem.TurnToAir();
		}
	}
}
public class ZombieArm_Rework_ModPlayer : ModPlayer {
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (Player.HeldItem.type == ItemID.ZombieArm) {
			Player.StrikeNPCDirect(npc, npc.CalculateHitInfo(Player.GetWeaponDamage(Player.HeldItem), 1));
		}
	}
}
