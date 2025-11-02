using System;
using Terraria;
using Humanizer;
using Terraria.ID;
using Roguelike.Texture;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Roguelike.Common.Global;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Roguelike.Common.Systems.Achievement;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Contents.Transfixion.Perks;

namespace Roguelike.Contents.Transfixion.Artifacts;
internal class TokenOfWrathArtifact : Artifact {
	public override IEnumerable<Item> AddStartingItems(Player player) {
		yield return new(ItemID.BladedGlove);
		yield return new(ItemID.WrathPotion, 5);
	}
	public override string TexturePath => ModTexture.Get_MissingTexture("Artifact");
	public override Color DisplayNameColor => Color.LimeGreen;
}
public class TokenOfWrathPlayer : ModPlayer {
	bool TokenOfWrath = false;
	public override void ResetEffects() {
		TokenOfWrath = Player.HasArtifact<TokenOfWrathArtifact>();
	}
	public override void UpdateEquips() {
		if (!TokenOfWrath) {
			return;
		}
		PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.1f);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, .25f);
		modplayer.NonCriticalDamage += .5f;
	}
}
public class StrikeOfFury : Perk {
	public override bool SelectChoosing() {
		return Artifact.PlayerCurrentArtifact<TokenOfWrathArtifact>() || AchievementSystem.IsAchieved("TokenOfWrath");
	}
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
		list_category.Add(PerkCategory.ArtifactExclusive);
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().NonCriticalDamage += .25f * StackAmount(player);
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (player.HasBuff<FuryStrike>()) {
			modifiers.ArmorPenetration += 10 * StackAmount(player);
		}
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (player.HasBuff<FuryStrike>()) {
			modifiers.ArmorPenetration += 10 * StackAmount(player);
		}
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		CriticalEffect(player, target, hit);
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		CriticalEffect(player, target, hit);
	}
	private void CriticalEffect(Player player, NPC target, NPC.HitInfo hit) {
		if (!player.HasBuff<FuryStrike>()) {
			if (Main.rand.NextFloat() <= .1f + .1f * StackAmount(player)) {
				player.AddBuff(ModContent.BuffType<FuryStrike>(), ModUtils.ToSecond(Main.rand.Next(3, 8)));
			}
		}
		else {
			if (Main.rand.NextFloat() <= Math.Clamp(.01f * StackAmount(player), 0, .25f)) {
				player.StrikeNPCDirect(target, hit);
			}
		}
		if (Main.rand.NextFloat() <= .05f * StackAmount(player)) {
			NPC.HitInfo newinfo = new NPC.HitInfo();
			newinfo.Damage = 45;
			newinfo.Crit = true;
			newinfo.DamageType = DamageClass.Default;
			player.StrikeNPCDirect(target, newinfo);
		}
	}
	class FuryStrike : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<PlayerStatsHandle>().NonCriticalDamage += .2f;
		}
		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
			tip = tip.FormatWith(1);
			if (Main.LocalPlayer.TryGetModPlayer(out PerkPlayer perkplayer)) {
				if (perkplayer.perks.ContainsKey(GetPerkType<StrikeOfFury>()))
					tip = tip.FormatWith(perkplayer.perks[GetPerkType<StrikeOfFury>()]);
			}
		}
	}
}
public class RuthlessRage : Perk {
	public override bool SelectChoosing() {
		return Artifact.PlayerCurrentArtifact<TokenOfWrathArtifact>() || AchievementSystem.IsAchieved("TokenOfWrath");
	}
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
		list_category.Add(PerkCategory.ArtifactExclusive);
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.NonCriticalDamage += .35f * StackAmount(player);
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .05f * StackAmount(player)) {
			modifiers.FinalDamage *= 2;
		}
		if (Main.rand.NextFloat() <= .01f) {
			modifiers.FinalDamage *= 4;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .05f * StackAmount(player)) {
			modifiers.FinalDamage *= 2;
		}
		if (Main.rand.NextFloat() <= .01f) {
			modifiers.FinalDamage *= 4;
		}
	}
}
