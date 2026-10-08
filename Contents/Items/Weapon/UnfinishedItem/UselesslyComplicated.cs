using Roguelike.Common.Global;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Utils;
using Roguelike.Contents.BuffAndDebuff;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Weapon.UnfinishedItem;
internal class UselesslyComplicated : SynergyModItem {
	public override void Synergy_SetStaticDefaults() {
		SynergyBonus_System.Add_SynergyBonus(Type, ItemID.ZapinatorOrange, $"[i:{ItemID.ZapinatorOrange}] All of the effects is double");
	}
	public override void SetDefaults() {
		Item.BossRushSetDefault(62, 62, 10, 1f, 60, 60, ItemUseStyleID.Swing, true);
		Item.DamageType = DamageClass.Melee;
		Item.value = Item.buyPrice(gold: 50);
		Item.UseSound = SoundID.Item1;
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul global)) {
			global.SwingType = BossRushUseStyle.Swipe;
		}
	}
	public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
		SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.ZapinatorOrange);
	}
	public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
		player.GetModPlayer<UselesslyComplicated_ModPlayer>().useless++;
		if(SynergyBonus_System.Check_SynergyBonus(Type, ItemID.ZapinatorOrange)) {
			player.GetModPlayer<UselesslyComplicated_ModPlayer>().useless++;
		}
		
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.EnchantedSword)
			.AddIngredient(ItemID.ZapinatorGray)
			.Register();
	}
}
public class UselesslyComplicated_ModPlayer : ModPlayer {
	public float DamageDealtIncreases = 0;
	public int useless = 0;
	public override void ResetEffects() {
		useless = 0;
		DamageDealtIncreases = 0;
	}
	int timer = 0;
	public override void UpdateEquips() {
		if (useless < 1) {
			return;
		}
		PlayerStatsHandle handler = Player.ModPlayerStats();
		handler.Chance_ToInstantKill += .01f * useless;
		handler.UpdateDefenseBase.Base += 1 * useless;
		handler.UpdateHPRegen.Base += 1 * useless;
		if (timer >= 60 && Main.rand.NextBool(90)) {
			timer = 0;
			Player.AddBuff<UselesslyOverlyComplicated_Buff8>(ModUtils.ToSecond(5));
		}
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		ModifyHitBy(ref modifiers);
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		ModifyHitBy(ref modifiers);
	}
	private void ModifyHitBy(ref Player.HurtModifiers modifiers) {
		if (useless < 1) {
			return;
		}
		modifiers.KnockbackImmunityEffectiveness *= useless + 1;
		for (int i = 0; i < useless; i++) {
			modifiers.SourceDamage -= Main.rand.NextFloat();
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (OnHit(hurtInfo)) {
			for (int i = 0; i < useless; i++) {
				int damage = 50;
				if (Main.rand.NextBool(30)) {
					damage += 1000;
				}
				Player.StrikeNPCDirect(npc, npc.CalculateHitInfo(damage, 1));
			}
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		OnHit(hurtInfo);
	}
	private bool OnHit(Player.HurtInfo hurtInfo) {
		if (useless < 1) {
			return false;
		}
		for (int i = 0; i < useless; i++) {
			if (Main.rand.NextBool(76)) {
				Player.Heal(Main.rand.Next(1, 501));
			}
		}
		return true;
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (useless < 1) {
			return;
		}
		for (int i = 0; i < useless; i++) {
			if (Main.rand.NextFloat() <= .01f) {
				modifiers.SourceDamage.Base += 1000;
			}
			modifiers.SourceDamage += DamageDealtIncreases;
			modifiers.SourceDamage += Main.rand.NextFloat();
			if (Main.rand.NextBool(101)) {
				modifiers.SetCrit();
			}
			if (Main.rand.NextBool(101)) {
				modifiers.FinalDamage += 5f;
				modifiers.SetCrit();
			}
			if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3) || target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2) || target.HasBuff(BuffID.CursedInferno) || target.HasBuff(BuffID.ShadowFlame)) {
				modifiers.SourceDamage.Base += Math.Clamp(target.life * .01f, 1f, 100f);
			}
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (useless < 1) {
			return;
		}
		for (int i = 0; i < useless; i++) {
			if (Main.rand.NextBool(150)) {
				Player.Heal(Main.rand.Next(1, 501));
			}
			if (Main.rand.NextBool(75)) {
				int[] buffs = [BuffID.OnFire3, BuffID.Frostburn2, BuffID.ShadowFlame, BuffID.CursedInferno, BuffID.Electrified, BuffID.Poisoned, BuffID.Venom, BuffID.Frozen, BuffID.Bleeding, BuffID.BrokenArmor, ModContent.BuffType<HolyFlame>()];
				target.AddBuff(Main.rand.Next(buffs), ModUtils.ToSecond(Main.rand.Next(5, 8)));
			}
			if (Main.rand.NextBool(25)) {
				int[] buffs = [ModContent.BuffType<UselesslyOverlyComplicated_Buff1>(), ModContent.BuffType<UselesslyOverlyComplicated_Buff2>(), ModContent.BuffType<UselesslyOverlyComplicated_Buff3>(), ModContent.BuffType<UselesslyOverlyComplicated_Buff4>(), ModContent.BuffType<UselesslyOverlyComplicated_Buff5>(), ModContent.BuffType<UselesslyOverlyComplicated_Buff6>(), ModContent.BuffType<UselesslyOverlyComplicated_Buff7>()];
				Player.AddBuff(Main.rand.Next(buffs), 180);
			}
			if (Main.rand.NextBool(10)) {
				Player.StrikeNPCDirect(target, hit);
			}
		}
	}
}
public class UselesslyOverlyComplicated_Buff1 : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<UselesslyComplicated_ModPlayer>().DamageDealtIncreases += .1f;
	}
}
public class UselesslyOverlyComplicated_Buff2 : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<UselesslyComplicated_ModPlayer>().DamageDealtIncreases += .25f;
	}
}
public class UselesslyOverlyComplicated_Buff3 : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<UselesslyComplicated_ModPlayer>().DamageDealtIncreases += .5f;
	}
}
public class UselesslyOverlyComplicated_Buff4 : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<UselesslyComplicated_ModPlayer>().DamageDealtIncreases += 1;
	}
}
public class UselesslyOverlyComplicated_Buff5 : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<UselesslyComplicated_ModPlayer>().DamageDealtIncreases += 2;
	}
}
public class UselesslyOverlyComplicated_Buff6 : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<UselesslyComplicated_ModPlayer>().DamageDealtIncreases += 5;
	}
}
public class UselesslyOverlyComplicated_Buff7 : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<UselesslyComplicated_ModPlayer>().DamageDealtIncreases += 10;
	}
}
public class UselesslyOverlyComplicated_Buff8 : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<UselesslyComplicated_ModPlayer>().DamageDealtIncreases += .5f;
	}
}
