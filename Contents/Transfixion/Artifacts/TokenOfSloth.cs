using System;
using Terraria;
using Terraria.ID;
using Roguelike.Texture;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using System.Collections.Generic;
using Roguelike.Common.Systems.Achievement;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Contents.Items.Weapon;
using Roguelike.Contents.Items.Weapon.ItemVariant;
using Roguelike.Contents.Transfixion.Perks;

namespace Roguelike.Contents.Transfixion.Artifacts;
internal class TokenOfSlothArtifact : Artifact {
	public override IEnumerable<Item> AddStartingItems(Player player) {
		WorldVaultSystem.Set_Variant = ModVariant.GetVariantType<BreakerBlade_Var1>();
		yield return new Item(ItemID.BreakerBlade);
		yield return new Item(ItemID.EndurancePotion, 5);
	}
	public override string TexturePath => ModTexture.Get_MissingTexture("Artifact");
	public override Color DisplayNameColor => Color.LimeGreen;
}
public class TokenOfSlothPlayer : ModPlayer {
	bool TokenOfSloth = false;
	public int SlothMeter = 0;
	public int Counter_Sloth = 0;
	public const int ThreeSecond = 180;
	public int SlumberCounter = 0;
	public bool SlumberState = false;
	public override void ResetEffects() {
		TokenOfSloth = Player.HasArtifact<TokenOfSlothArtifact>();
		if (SlothMeter <= 0 || Player.velocity == Vector2.Zero || !TokenOfSloth) {
			return;
		}
	}
	public override void UpdateEquips() {
		if (TokenOfSloth) {
			for (int i = 0; i < SlothMeter; i++) {
				Vector2 pos = Player.Center +
						Vector2.One.Vector2DistributeEvenly(SlothMeter, 360, i)
						.RotatedBy(MathHelper.ToRadians(Player.GetModPlayer<ModUtilsPlayer>().counterToFullPi)) * 30 - Vector2.One;
				int dust = Dust.NewDust(pos, 0, 0, DustID.GemTopaz);
				Main.dust[dust].velocity = Vector2.Zero;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].Dust_BelongTo(Player);
				Main.dust[dust].Dust_GetDust().FollowEntity = true;
			}
			if (!Player.ItemAnimationActive) {
				if (++Counter_Sloth >= ThreeSecond) {
					SlothMeter = Math.Clamp(SlothMeter + 1, 0, 2);
					Counter_Sloth = 0;
				}
			}
			else {
				Counter_Sloth = 0;
			}
			PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, .65f);
			modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, .9f);
			modplayer.AddStatsToPlayer(PlayerStats.Defense, 1.1f);
			modplayer.AddStatsToPlayer(PlayerStats.MaxHP, 1.15f);
		}
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (SlothMeter > 0) {
			modifiers.SetMaxDamage(1);
			SlothMeter--;
			Counter_Sloth = 0;
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (SlothMeter > 0) {
			modifiers.SetMaxDamage(1);
			SlothMeter--;
			Counter_Sloth = 0;
		}
	}
}
public class SlothVessle : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
	}
	public override bool SelectChoosing() {
		return Artifact.PlayerCurrentArtifact<TokenOfSlothArtifact>() || AchievementSystem.IsAchieved("TokenOfSloth");
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.Iframe, 1 + .1f * StackAmount(player));
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (player.immune || player.immuneTime > 0 && Main.rand.NextFloat() <= .05f * StackAmount(player)) {
			player.Heal((int)(item.damage * .1f));
		}
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (player.immune || player.immuneTime > 0 && Main.rand.NextFloat() <= .05f * StackAmount(player)) {
			player.Heal((int)(proj.damage * .1f));
		}
	}
	public override void ModifyHitByNPC(Player player, NPC npc, ref Player.HurtModifiers modifiers) {
		if (Main.rand.NextFloat() <= .1f * StackAmount(player)) {
			modifiers.SetMaxDamage(1);
		}
	}
	public override void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) {
		if (Main.rand.NextFloat() <= .1f * StackAmount(player)) {
			modifiers.SetMaxDamage(1);
		}
	}
}
