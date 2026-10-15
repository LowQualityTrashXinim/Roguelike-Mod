using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Roguelike.Common.General;
using Roguelike.Common.Global;
using Roguelike.Common.Global.Mechanic.OutroEffect;
using Roguelike.Common.Global.Mechanic.OutroEffect.Contents.ShortBurst;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.RoguelikeMode.ItemOverhaul.Accessories;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Weapon.MeleeSynergyWeapon.HeartBreakInstrument;
public static class HeartBreakInstrument_Tool {
	public static void SpawnDust(NPC target) {
		Vector2 ran = Main.rand.NextVector2Circular(target.width, target.height) * .75f;
		Dust dust = Dust.NewDustDirect(target.Center + ran, 0, 0, ModContent.DustType<HeartBreakInstrument_Cross>());
		dust.noGravity = true;
		dust.velocity = Vector2.Zero;
		dust.rotation = MathHelper.ToRadians(Main.rand.NextFloat(1, 90));
		dust.color = Color.DodgerBlue;
		dust.scale = Main.rand.NextFloat(1.1f, 1.3f);
		for (int i = 0; i < 5; i++) {
			dust = Dust.NewDustDirect(target.Center + ran, 0, 0, ModContent.DustType<HeartBreakInstrument_Cross>());
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2Circular(3, 3);
			dust.rotation = MathHelper.ToRadians(Main.rand.NextFloat(1, 90));
			dust.color = Color.DodgerBlue;
			dust.scale = Main.rand.NextFloat(.4f, .6f);
		}
		for (int i = 0; i < 15; i++) {
			dust = Dust.NewDustDirect(target.Center + ran, 0, 0, ModContent.DustType<HeartBreakInstrument_Extra>());
			dust.velocity = Main.rand.NextVector2Circular(5, 5);
			dust.rotation = dust.velocity.ToRotation();
			dust.scale = Main.rand.NextFloat(.4f, .8f);
			dust.color = Color.DodgerBlue;
			dust.noGravity = true;
		}
	}
	public static readonly SoundStyle[] Guitar = [
				SoundID.Item133,
				SoundID.Item134,
				SoundID.Item135,
				SoundID.Item136,
				SoundID.Item137,
				SoundID.Item138,
		];
}
public class HeartBreakInstrument : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultMeleeShootCustomProjectile(102, 102, 65, 4f, 24, 24, ItemUseStyleID.Swing, 1, 1f, true);
		Item.UseSound = SoundID.Item1 with { Pitch = -1f };
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul global)) {
			global.SwingType = BossRushUseStyle.SwipeDown;
			global.SwingDegree = 145;
			global.SwingStrength = 7f;
			global.Ignore_AttackSpeed = true;
			global.DistanceThrust = 120;
			global.OffsetThrust = 20;
		}
		Item.Set_InfoItem();
		Item.Set_ItemOutroEffect<OutroEffect_HeartBreakInstrument>();
	}
	int ComboChain_ResetTimer = 0;
	int ComboChain_Count = 0;
	int ComboChain_CoolDownAttack = 0;
	public override bool CanUseItem(Player player) {
		if (ComboChain_CoolDownAttack > 0) {
			return false;
		}
		if (!player.ItemAnimationActive) {
			var overhaul = Item.GetGlobalItem<MeleeWeaponOverhaul>();
			if (player.altFunctionUse == 2) {
				SetStyle(overhaul, 3);
				return base.CanUseItem(player);
			}
			else {
				ComboChain_ResetTimer = 120 + player.itemAnimationMax;
				ComboChain_Count++;
				SetStyle(overhaul, ComboChain_Count);
				if (ComboChain_Count >= 5) {
					ComboChain_Count = 0;
				}
			}
		}
		return base.CanUseItem(player);
	}
	private void SetStyle(MeleeWeaponOverhaul overhaul, int Style) {
		switch (Style) {
			case 1:
				Item.reuseDelay = 0;
				Item.noUseGraphic = false;
				overhaul.HideSwingVisual = false;
				overhaul.SwingType = BossRushUseStyle.SwipeDown;
				overhaul.SwingStrength = 9;
				break;
			case 2:
				overhaul.SwingType = BossRushUseStyle.SwipeUp;
				overhaul.SwingStrength = 9;
				break;
			case 3:
				overhaul.HideSwingVisual = true;
				overhaul.SwingType = BossRushUseStyle.Thrust_Style1;
				overhaul.SwingStrength = 15;
				overhaul.IframeDivision = 1;
				overhaul.DistanceThrust = 10;
				break;
			case 4:
				overhaul.HideSwingVisual = false;
				overhaul.SwingType = BossRushUseStyle.SwipeDown;
				overhaul.SwingStrength = 9;
				break;
			case 5:
				overhaul.SwingType = BossRushUseStyle.SwipeUp;
				overhaul.SwingStrength = 9;
				break;
		}
	}
	public override bool AltFunctionUse(Player player) => true;
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		CanShootItem = false;
		Projectile projectile;
		type = ModContent.ProjectileType<HeartBreakInstrument_Slash_Projectile>();
		HeartBreakInstrument_ModPlayer heartplayer = player.GetModPlayer<HeartBreakInstrument_ModPlayer>();
		if (heartplayer.IntroEffectActivate) {
			heartplayer.IntroEffectActivate = false;
			for (int i = 0; i < 20; i++) {
				Vector2 post = position.PositionOFFSET(velocity, Main.rand.Next(75, 250)) + Main.rand.NextVector2Circular(20, 20);
				projectile = Projectile.NewProjectileDirect(source, post, Main.rand.NextVector2CircularEdge(1, 1), type, damage, knockback, player.whoAmI, 3, 15, i * 2);
				if (projectile.ModProjectile is HeartBreakInstrument_Slash_Projectile proj) {
					proj.ScaleX = 10f;
					proj.ScaleY = .5f;
					projectile.scale = 2;
				}
			}
		}
		bool ClimaxOfTragedy = heartplayer.ClimaxOfTradegy;
		int count = ComboChain_Count;
		if (ClimaxOfTragedy) {
			int ghostType = ModContent.ProjectileType<HeartBreakInstrument_Ghost_Projectile>();
			if (player.ownedProjectileCounts[ghostType] < 4) {
				if (player.altFunctionUse != 2) {
					int whoAmI = Projectile.NewProjectile(source, Main.MouseWorld + Vector2.One.Vector2DistributeEvenlyPlus(5, 360, count) * 300, Vector2.Zero, ghostType, damage * 3, knockback, player.whoAmI);
					player.ModPlayerStats().WhoAmI_Projectile.Add(whoAmI);
				}
			}
			else {
				PlayerStatsHandle handler = player.ModPlayerStats();
				for (int i = 0; i < handler.WhoAmI_Projectile.Count; i++) {
					Projectile proj = Main.projectile[handler.WhoAmI_Projectile[i]];
					if (proj == null) {
						continue;
					}
					if (proj.ModProjectile is HeartBreakInstrument_Ghost_Projectile ghost) {
						ghost.ActivateAttack();
					}
				}
			}
			heartplayer.ClimaxOfTradegy_Count++;
		}
		if (player.altFunctionUse == 2) {
			if (!ClimaxOfTragedy && heartplayer.ClimaxOfTradegy_Count >= 30) {
				heartplayer.ClimaxOfTradegy = true;
				heartplayer.ClimaxOfTradegy_Count = 0;
				heartplayer.Effect = false;
				for (int i = 0; i < 4; i++) {
					Projectile.NewProjectile(source, position, Vector2.One, ModContent.ProjectileType<HeartBreakInstrument_StarProjectile>(), damage, knockback, player.whoAmI, 300, i);
				}
				type = ModContent.ProjectileType<HeartBreakerInstrument_Special>();
				Projectile.NewProjectileDirect(source, position.Add(-30, -30), Vector2.Zero, type, damage, knockback, player.whoAmI);
				Projectile.NewProjectileDirect(source, position.Add(-10, -50), Vector2.Zero, type, damage, knockback, player.whoAmI);
				Projectile.NewProjectileDirect(source, position.Add(10, -50), Vector2.Zero, type, damage, knockback, player.whoAmI);
				Projectile.NewProjectileDirect(source, position.Add(30, -30), Vector2.Zero, type, damage, knockback, player.whoAmI);
				player.AddBuff<HeartBreakInstrument_Special_CoolDown>(ModUtils.ToSecond(15));
				return;
			}
			else {
				if (!heartplayer.OnCoolDown) {
					if (ClimaxOfTragedy) {
						player.AddBuff<HeartBreakInstrument_Buff>(ModUtils.ToSecond(15));
						for (int i = 0; i < 4; i++) {
							Projectile.NewProjectile(source, position, Vector2.One, ModContent.ProjectileType<HeartBreakInstrument_StarProjectile>(), damage, knockback, player.whoAmI, 300, i);
						}
						type = ModContent.ProjectileType<HeartBreakerInstrument_Special>();
						Projectile.NewProjectileDirect(source, position.Add(-30, -30), Vector2.Zero, type, damage, knockback, player.whoAmI);
						Projectile.NewProjectileDirect(source, position.Add(-10, -50), Vector2.Zero, type, damage, knockback, player.whoAmI);
						Projectile.NewProjectileDirect(source, position.Add(10, -50), Vector2.Zero, type, damage, knockback, player.whoAmI);
						Projectile.NewProjectileDirect(source, position.Add(30, -30), Vector2.Zero, type, damage, knockback, player.whoAmI);
						player.AddBuff<HeartBreakInstrument_Special_CoolDown>(ModUtils.ToSecond(15));
					}
					else {
						heartplayer.DashDir = true;
						player.AddBuff<HeartBreakInstrument_Special_CoolDown>(ModUtils.ToSecond(1));
					}
					return;
				}
			}
		}
		var pos = Main.MouseWorld;
		if ((pos - player.Center).LengthSquared() > 5625) {
			pos = position.PositionOFFSET(velocity, 75f);
		}

		if (count == 3) {
			int amount = 5;
			if (ClimaxOfTragedy) {
				amount = 8;
				damage = (int)(damage * .65f);
			}
			else {
				damage = (int)(damage * .35f);
			}
			for (int i = 0; i < amount; i++) {
				Vector2 post = pos.PositionOFFSET(velocity, Main.rand.Next(0, 75)) + Main.rand.NextVector2Circular(20, 20);
				projectile = Projectile.NewProjectileDirect(source, post, velocity.Vector2RotateByRandom(10), type, damage, knockback, player.whoAmI, 10, 6, i * 4);
				if (projectile.ModProjectile is HeartBreakInstrument_Slash_Projectile proj) {
					proj.ScaleX = 5f;
					if (ClimaxOfTragedy) {
						proj.ScaleX += 2;
					}
					proj.ScaleY = .5f;
					proj.FollowPlayer = true;
					projectile.scale = 2;
				}
				if (ClimaxOfTragedy) {
					post = pos.PositionOFFSET(velocity, Main.rand.Next(0, 75)) + Main.rand.NextVector2Circular(50, 50);
					projectile = Projectile.NewProjectileDirect(source, post, velocity.Vector2RotateByRandom(30), type, damage, knockback, player.whoAmI, 20, 6, i * 4);
					if (projectile.ModProjectile is HeartBreakInstrument_Slash_Projectile proj3) {
						proj3.ScaleX = 7f;
						proj3.ScaleY = .5f;
						proj3.FollowPlayer = true;
						projectile.scale = 2;
					}
				}
			}
			Gclef proj2 = Get_ModProjectile(player);
			if (proj2 != null) {
				proj2.HitNote(player);
			}
		}
		else if (count == 4) {
			ComboChain_CoolDownAttack = ModUtils.ToSecond(1) + player.itemAnimationMax;
			ComboChain_ResetTimer += ComboChain_CoolDownAttack;
			Gclef proj = Get_ModProjectile(player);
			if (proj != null) {
				proj.MoveToCursorTime = 1200;
			}
		}
		else if (count == 0) {
			ComboChain_CoolDownAttack = ModUtils.ToSecond(1f) + player.itemAnimationMax;
			ComboChain_ResetTimer += ComboChain_CoolDownAttack;
			int amount = 15;
			if (ClimaxOfTragedy) {
				damage = (int)(damage * 2.2f);
				amount = 25;
			}
			else {
				damage = (int)(damage * .78f);
			}
			for (int i = 0; i < amount; i++) {
				Vector2 vel = velocity.RotatedBy(MathHelper.PiOver2).Vector2RotateByRandom(180);
				projectile = Projectile.NewProjectileDirect(source, pos.PositionOFFSET(velocity, 35f) + Main.rand.NextVector2Circular(15, 15), vel, type, damage + (int)(damage * .1f * i), knockback, player.whoAmI, 5 * Main.rand.NextBool().ToDirectionInt(), 9, 25 + i * 3);
				if (projectile.ModProjectile is HeartBreakInstrument_Slash_Projectile proj) {
					proj.ScaleX = 5f + Main.rand.NextFloat(-1, 1);
					proj.ScaleY = .5f;
					if (ClimaxOfTragedy) {
						proj.ScaleX += 5f;
						proj.ScaleY += 2;

					}
					projectile.scale = 2;
				}
			}
		}
		else {
			int amount = count;
			if (ClimaxOfTragedy) {
				amount += Main.rand.Next(10, 17);
				damage += (int)(damage * .32f);
				for (int i = 0; i < amount; i++) {
					Vector2 post = position.PositionOFFSET(velocity, 75 + 35 * i) + Main.rand.NextVector2Circular(20, 20);
					projectile = Projectile.NewProjectileDirect(source, post, Main.rand.NextVector2CircularEdge(1, 1), type, damage, knockback, player.whoAmI, 3, 15, i * 2);
					if (projectile.ModProjectile is HeartBreakInstrument_Slash_Projectile proj) {
						proj.ScaleX = 5f;
						proj.ScaleY = .5f;
						projectile.scale = 2;
					}
				}
			}
			else {
				for (int i = 0; i < amount; i++) {
					Vector2 vel = velocity.RotatedBy(MathHelper.PiOver2).Vector2RotateByRandom(60);
					projectile = Projectile.NewProjectileDirect(source, pos.PositionOFFSET(velocity, 35f) + Main.rand.NextVector2Circular(15, 15), vel, type, damage, knockback, player.whoAmI, 5 * Main.rand.NextBool().ToDirectionInt(), 12, i * 5);
					if (projectile.ModProjectile is HeartBreakInstrument_Slash_Projectile proj) {
						proj.ScaleX = 3f;
						proj.ScaleY = .5f;
						projectile.scale = 2;
					}
				}
			}
			Gclef proj2 = Get_ModProjectile(player);
			if (proj2 != null) {
				proj2.HitNote(player);
			}
		}
	}
	private Gclef Get_ModProjectile(Player player) {
		var musicPlayer = player.GetModPlayer<HeartBreakInstrument_ModPlayer>();
		if (musicPlayer.Projectile_WhoAmI <= -1 || musicPlayer.Projectile_WhoAmI > 1000) {
			return null;
		}
		return (Gclef)Main.projectile[musicPlayer.Projectile_WhoAmI].ModProjectile;
	}
	public override void SynergyUpdateInventory(Player player, PlayerSynergyItemHandle modplayer) {
		ComboChain_ResetTimer = ModUtils.CountDown(ComboChain_ResetTimer);
		ComboChain_CoolDownAttack = ModUtils.CountDown(ComboChain_CoolDownAttack);
		if (ComboChain_ResetTimer <= 0) {
			ComboChain_Count = 0;
		}
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) {
		HeartBreakInstrument_Tool.SpawnDust(target);
	}
	public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
		var musicPlayer = player.GetModPlayer<HeartBreakInstrument_ModPlayer>();
		if (OutroEffect_ModPlayer.Check_ValidForIntroEffect(player)) {
			OutroEffect_ModPlayer.Set_IntroEffect(player, Type, 1);
			musicPlayer.IntroEffectActivate = true;
		}
		if (player.ItemAnimationActive) {
			musicPlayer.IntroEffectActivate = false;
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.MagicalHarp)
			.AddIngredient(ItemID.Anchor)
			.AddIngredient(ItemID.BeamSword)
			.Register();
	}
}
public class HeartBreakInstrument_ModPlayer : ModPlayer {
	public int ClimaxOfTradegy_Count = 0;
	public bool ClimaxOfTradegy = false;
	public bool OnCoolDown = false;
	public bool Effect = false;
	public bool IntroEffectActivate = false;
	public int Projectile_WhoAmI = -1;
	private Gclef Get_ModProjectile() {
		if (Projectile_WhoAmI <= -1 || Projectile_WhoAmI > 1000) {
			return null;
		}
		return (Gclef)Main.projectile[Projectile_WhoAmI].ModProjectile;
	}
	public const int DashCooldown = 50;
	public const int DashDuration = 35;

	public const float DashVelocity = 7.5f;

	public bool DashDir = false;

	public int DashDelay = 0;
	public int DashTimer = 0;
	public Vector2 ReappliedMovement = Vector2.Zero;
	public override void PreUpdateMovement() {
		if (DashDir && DashDelay == 0) {
			DashTimer = 10;
			DashDelay = DashTimer * 3 + DashTimer;
			var vel = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.Zero) * 45;
			ReappliedMovement = vel;
			Player.velocity = vel;
			Player.direction = Player.velocity.X > 0 ? 1 : -1;
		}

		if (DashDelay > 0)
			DashDelay--;

		if (DashTimer > 0) {
			int type = ModContent.ProjectileType<HeartBreakInstrument_Slash_Projectile>();
			int damage = Player.GetWeaponDamage(Player.HeldItem);
			Vector2 post = Player.Center + Main.rand.NextVector2Circular(20, 20);
			Projectile projectile = Projectile.NewProjectileDirect(Player.GetSource_ItemUse(Player.HeldItem), post, Main.rand.NextVector2CircularEdge(1, 1), type, damage, 1, Player.whoAmI, 3, 10, 0);
			if (projectile.ModProjectile is HeartBreakInstrument_Slash_Projectile proj) {
				proj.ScaleX = 4f;
				proj.ScaleY = .25f;
				projectile.scale = 1;
			}
			Player.velocity = ReappliedMovement;
			if (DashTimer == 1) {
				Player.velocity = Player.velocity * .25f;
			}
			for (int i = 0; i < 3; i++) {
				Dust dust = Dust.NewDustDirect(Player.Center + Main.rand.NextVector2Circular(20, 20), 0, 0, ModContent.DustType<HeartBreakInstrument_Extra>());
				dust.velocity = -Vector2.UnitY;
				dust.rotation = dust.velocity.ToRotation();
				dust.scale = Main.rand.NextFloat(.7f, 1.3f);
				dust.color = Color.DodgerBlue;
				dust.noGravity = true;
			}
			DashTimer--;
		}
		else {
			if (DashDir) {
				int type = ModContent.ProjectileType<HeartBreakInstrument_Slash_Projectile>();
				int damage = Player.GetWeaponDamage(Player.HeldItem);
				for (int i = 0; i < 10; i++) {
					Vector2 post = Player.Center.PositionOFFSET(Vector2.UnitX * Player.direction, Main.rand.Next(75, 250)) + Main.rand.NextVector2Circular(20, 20);
					Projectile projectile = Projectile.NewProjectileDirect(Player.GetSource_ItemUse(Player.HeldItem), post, Main.rand.NextVector2CircularEdge(1, 1), type, damage, 1, Player.whoAmI, 3, 15, 10 + i);
					if (projectile.ModProjectile is HeartBreakInstrument_Slash_Projectile proj) {
						proj.ScaleX = 7f;
						proj.ScaleY = .25f;
						projectile.scale = 2;
					}
				}
			}
			DashDir = false;
			ReappliedMovement = Vector2.Zero;
		}
	}
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		if (!Player.IsHeldingModItem<HeartBreakInstrument>()) {
			Gclef proj = Get_ModProjectile();
			if (proj != null) {
				proj.Set_KillState();
			}
			Projectile_WhoAmI = -1;
			ClimaxOfTradegy_Count = 0;
			ClimaxOfTradegy = false;
			Effect = false;
		}
		else {
			if (Player.altFunctionUse == 2 && Player.ItemAnimationActive) {
				Player.ModPlayerStats().CurrentDashType = "HeartBreak";
			}

			if (DashTimer > 0) {
				Player.immune = true;
				Player.immuneTime = 2;
				Player.immuneNoBlink = true;
			}
			int type = ModContent.ProjectileType<Gclef>();
			if (Player.ownedProjectileCounts[type] < 1) {
				if (Projectile_WhoAmI == -1) {
					Projectile_WhoAmI = Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), Player.Center, Vector2.Zero, type, (int)(Player.GetWeaponDamage(Player.HeldItem) * .55f), 0, Player.whoAmI);
				}
				else {
					if (!Main.projectile[Projectile_WhoAmI].active) {
						Projectile_WhoAmI = -1;
					}
				}
			}
		}
		OnCoolDown = false;
		if (ClimaxOfTradegy_Count >= 30 && !ClimaxOfTradegy) {
			if (!Effect) {
				Effect = true;
				VisualEffect();
			}
		}
		else if (ClimaxOfTradegy_Count >= 30 && ClimaxOfTradegy) {
			if (!Effect) {
				Effect = true;
				VisualEffect();
			}
			if (!Player.ItemAnimationActive) {
				if (Main.mouseLeft) {
					ClimaxOfTradegy_Count = 0;
					ClimaxOfTradegy = false;
					Effect = false;
				}
				else if (Main.mouseRight) {
					ClimaxOfTradegy_Count = 0;
					ClimaxOfTradegy = false;
					Effect = false;
				}
			}
		}
		if (ClimaxOfTradegy) {
			Dust dust = Dust.NewDustDirect(Player.Center + Main.rand.NextVector2Circular(20, 20), 0, 0, ModContent.DustType<HeartBreakInstrument_Extra>());
			dust.velocity = -Vector2.UnitY;
			dust.rotation = dust.velocity.ToRotation();
			dust.scale = Main.rand.NextFloat(.7f, 1.3f);
			dust.color = Color.DodgerBlue;
			dust.noGravity = true;
		}
	}
	private void VisualEffect() {
		ModUtils.DustStar(Player.Center, ModContent.DustType<HeartBreakInstrument_Extra>(), Color.DodgerBlue);
		for (int i = 0; i < 50; i++) {
			Vector2 pos = Player.Center + Main.rand.NextVector2CircularEdge(100, 100);
			Dust dust = Dust.NewDustDirect(pos, 0, 0, ModContent.DustType<HeartBreakInstrument_Extra>());
			dust.velocity = (Player.Center - pos).SafeNormalize(Vector2.Zero);
			dust.rotation = dust.velocity.ToRotation();
			dust.scale = Main.rand.NextFloat(1.4f, 1.8f);
			dust.color = Color.DodgerBlue;
			dust.noGravity = true;
		}
	}
}
public class HeartBreakInstrument_StarProjectile : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.PiercingStarlight);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 30;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 200;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		NPCtoTarget = -1;
		Index = -1;
	}
	int DelayBeforeDecaySound = 0;
	public float Index = 0;
	public int Counter = 0;
	public int NPCtoTarget = -1;
	public override void OnSpawn(IEntitySource source) {
		if (Projectile.ai[0] != 0) {
			Projectile.timeLeft = (int)Projectile.ai[0];
		}
		Index = Projectile.ai[1];
		Projectile.ai[1] = 0;
		if (Projectile.Center.LookForHostileNPC(out NPC npc, 750)) {
			NPCtoTarget = npc.whoAmI;
		}
	}
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		float percentage = ModUtils.InOutExpo(1 - Projectile.timeLeft / (float)Projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().InitialTimeLeft);
		Projectile.Center = player.Center + Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, percentage) + MathHelper.PiOver2 * Index) * 125 * percentage;

		Projectile.velocity *= .97f;

		Attack();

		if (Projectile.scale > 1) {
			Projectile.scale -= .025f;
		}
		else {
			Projectile.scale = 1f;
		}
		if (--DelayBeforeDecaySound <= 0) {
			DelayBeforeDecaySound = 0;
			if (Projectile.ai[2] > -1) {
				Projectile.ai[2] -= .01f;
			}
		}
	}
	public void Attack() {
		if (Projectile.timeLeft <= 30) {
			return;
		}
		SoundStyle sound = SoundID.Item35;
		int CD = 20;
		int damage = Projectile.damage;
		if (++Projectile.ai[1] < CD) {
			return;
		}
		Projectile.ai[1] = 0;
		SpawnVisualTracker();
		Projectile.scale += .2f;
		Projectile.ai[2] = Math.Clamp(Projectile.ai[2] + .15f, -1, 1);
		DelayBeforeDecaySound = 10;
		SoundEngine.PlaySound(SoundID.Item35 with { Pitch = Projectile.ai[2] }, Projectile.Center);
	}
	public override bool? CanDamage() {
		return false;
	}
	List<SpriteTracker> tracker = new();
	public override void OnKill(int timeLeft) {
		Player player = Main.player[Projectile.owner];
		Projectile.Center.LookForHostileNPC(out List<NPC> listnpc, 500);
		foreach (var npc in listnpc) {
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo(Projectile.damage * 15, ModUtils.DirectionFromEntityAToEntityB(npc.Center.X, Projectile.Center.X)));
			float rotate = MathHelper.ToRadians(Main.rand.NextFloat(360));
			ModUtils.DustStar(npc.Center, DustID.GemDiamond, Color.White, rotationOff: rotate, speed: 10);
			ModUtils.DustStar(npc.Center, DustID.GemDiamond, Color.DodgerBlue, thiccness: 30, rotationOff: rotate, thiccness2: 12);
		}

		player.Heal((int)(player.statLifeMax2 * .05f));
		for (int i = 0; i < 3; i++) {
			Vector2 ran = Main.rand.NextVector2Circular(Projectile.width, Projectile.height) * .75f;
			Dust star = Dust.NewDustDirect(Projectile.Center + ran, 0, 0, ModContent.DustType<HeartBreakInstrument_Cross>());
			star.noGravity = true;
			star.velocity = Vector2.Zero;
			star.rotation = MathHelper.ToRadians(Main.rand.NextFloat(1, 90));
			star.color = Color.DodgerBlue;
			star.scale = Main.rand.NextFloat(3.1f, 4.3f);
		}
		for (int i = 0; i < 15; i++) {
			Vector2 ran = Main.rand.NextVector2Circular(Projectile.width, Projectile.height) * .75f;
			Dust dust = Dust.NewDustDirect(Projectile.Center + ran, 0, 0, ModContent.DustType<HeartBreakInstrument_Cross>());
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2Circular(15, 15);
			dust.rotation = MathHelper.ToRadians(Main.rand.NextFloat(1, 90));
			dust.color = Color.DodgerBlue;
			dust.scale = Main.rand.NextFloat(1.4f, 1.6f);
		}
		for (int i = 0; i < 50; i++) {
			Vector2 ran = Main.rand.NextVector2Circular(Projectile.width, Projectile.height) * .75f;
			Dust dust = Dust.NewDustDirect(Projectile.Center + ran, 0, 0, ModContent.DustType<HeartBreakInstrument_Extra>());
			dust.velocity = Main.rand.NextVector2Circular(20, 20);
			dust.rotation = dust.velocity.ToRotation();
			dust.scale = Main.rand.NextFloat(1.4f, 1.8f);
			dust.color = Color.DodgerBlue;
			dust.noGravity = true;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		if (Projectile.timeLeft <= 100) {
			lightColor *= Projectile.timeLeft / 100f;
		}

		Main.instance.LoadItem(Type);
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;
		ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);
		Main.EntitySpriteDraw(texture, drawpos, null, Color.DodgerBlue, MathHelper.PiOver2, origin, Projectile.scale, SpriteEffects.None);
		Main.EntitySpriteDraw(texture, drawpos, null, Color.White, MathHelper.PiOver2, origin, Projectile.scale * .5f, SpriteEffects.None);

		Main.EntitySpriteDraw(texture, drawpos, null, Color.Cyan, 0, origin, Projectile.scale * .75f, SpriteEffects.None);
		Main.EntitySpriteDraw(texture, drawpos, null, Color.White, 0, origin, Projectile.scale * .25f, SpriteEffects.None);
		ModUtils.Draw_ResetToNormal(Main.spriteBatch);

		TrackerHandler(Color.DodgerBlue, Color.White, origin);
		return false;
	}
	private void SpawnVisualTracker() {
		SpriteTracker track = new(Vector2.Zero, 0, 20);
		track.position = Projectile.Center;
		track.scale = 1;
		tracker.Add(track);
	}
	private void TrackerHandler(Color color, Color color2, Vector2 origin) {
		if (tracker == null) {
			tracker = new();
		}
		Texture2D texture = ModContent.Request<Texture2D>(ModTexture.OuterInnerGlow).Value;
		Vector2 origin2 = texture.Size() * .5f;
		for (int i = tracker.Count - 1; i >= 0; i--) {
			SpriteTracker tr = tracker[i];
			tr.position = Projectile.Center;
			Color baseOnScale = color * (tr.TimeLeft / 20f);
			Color baseOnScale2 = color2 * (tr.TimeLeft / 20f);
			ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);
			Main.spriteBatch.Draw(texture, tr.position - Main.screenPosition, null, Color.Black * (tr.TimeLeft / 60f), tr.rotation, origin2, tr.scale, SpriteEffects.None, 0);
			ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);
			Main.spriteBatch.Draw(texture, tr.position - Main.screenPosition, null, baseOnScale * .5f, tr.rotation, origin2, tr.scale, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(texture, tr.position - Main.screenPosition, null, baseOnScale2 * .5f, tr.rotation, origin2, tr.scale, SpriteEffects.None, 0);
			ModUtils.Draw_ResetToNormal(Main.spriteBatch);
			if (tr.TimeLeft >= 15) {
				tr.scale += .2f;
			}
			else if (tr.TimeLeft >= 10) {
				tr.scale += .1f;
			}
			else {
				tr.scale += .01f;
			}
			if (--tr.TimeLeft <= 0) {
				tracker.RemoveAt(i);
			}
			else {
				tracker[i] = tr;
			}
		}
	}
}
public class HeartBreakInstrument_Buff : ModBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateDefenseBase.Base += 15;
		handler.UpdateHPRegen.Base += 10;
		handler.UpdateCritDamage += .4f;
		handler.UpdateMovement += .3f;
		player.GetCritChance(DamageClass.Generic) += 10;
		player.GetDamage(DamageClass.Generic) += .2f;
	}
}
public class HeartBreakInstrument_Note_Projectile : ModProjectile {
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 9000;
		Projectile.extraUpdates = 10;
	}
	float TextureIDSelf = -1;
	public override void OnSpawn(IEntitySource source) {
		TextureIDSelf = Projectile.ai[0];
	}
	float VelocityLength = 0;
	public override void AI() {
		if (++Projectile.ai[1] < 600) {
			return;
		}
		if (Projectile.Center.LookForHostileNPC(out NPC npc, 750)) {
			if (VelocityLength == 0) {
				VelocityLength = Projectile.velocity.Length();
			}
			if (VelocityLength >= 2) {
				VelocityLength *= .995f;
			}
			else {
				VelocityLength += .0001f;
			}
			Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * VelocityLength;
		}
		else {
			Projectile.velocity *= .99f;
			if (Projectile.ai[2] <= 100) {
				Projectile.velocity += Vector2.UnitY * .0025f;
			}
			else if (Projectile.ai[2] > 100 && Projectile.ai[2] <= 200) {
				Projectile.velocity += -Vector2.UnitY * .0025f;
			}
			else {
				Projectile.ai[2] = -1;
			}
			if (Projectile.velocity.IsLimitReached(.5f)) {
				Projectile.velocity = Projectile.velocity.LimitedVelocity(.5f);
			}
			Projectile.ai[2]++;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		if (TextureIDSelf <= 0 || TextureIDSelf > ItemID.Count) {
			return false;
		}
		Texture2D texture;
		switch (TextureIDSelf) {
			case 1:
				texture = ModContent.Request<Texture2D>(ModUtils.GetTheSameTextureAsEntity<Crotchet>()).Value;
				break;
			case 2:
				texture = ModContent.Request<Texture2D>(ModUtils.GetTheSameTextureAsEntity<Minim>()).Value;
				break;
			case 3:
				texture = ModContent.Request<Texture2D>(ModUtils.GetTheSameTextureAsEntity<Quaver>()).Value;
				break;
			default:
				texture = ModContent.Request<Texture2D>(ModUtils.GetTheSameTextureAsEntity<SemiQuaver>()).Value;
				break;
		}

		Vector2 origin = texture.Size() * .5f;
		Vector2 drawPos = Projectile.Center - Main.screenPosition;
		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

		Texture2D line = ModContent.Request<Texture2D>(ModTexture.WHITEDOT).Value;
		Vector2 originLine = line.Size() * .5f;
		float len = Projectile.oldPos.Length;
		for (int k = 0; k < len; k++) {
			Vector2 drawPosDot = Projectile.oldPos[k] - Main.screenPosition + origin - Vector2.UnitX * 5;
			Color color2 = lightColor * ((Projectile.oldPos.Length - k) / len);
			Main.EntitySpriteDraw(line, drawPosDot, null, color2, 0, originLine, 1f, SpriteEffects.None, 0);
		}
		return false;
	}
}
public class HeartBreakInstrument_Ghost_Projectile : ModProjectile {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<HeartBreakInstrument>();
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 18000;
		Projectile.scale = .75f;
		Projectile.extraUpdates = 5;
		Projectile.light = .5f;
	}
	public override bool? CanDamage() => Projectile.ai[0] >= 1;
	public override void AI() {
		if (!Main.player[Projectile.owner].GetModPlayer<HeartBreakInstrument_ModPlayer>().ClimaxOfTradegy) {
			ActivateAttack();
		}
	}
	public void ActivateAttack() {
		if (Projectile.ai[0] == 0) {
			Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 20;
			Projectile.timeLeft = 900;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.ai[0] = 10;
			for (int i = 0; i < 30; i++) {
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<HeartBreakInstrument_Extra>());
				dust.velocity = -Projectile.velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.5f, 1.1f);
				dust.rotation = dust.velocity.ToRotation();
				dust.scale = 1 + Main.rand.NextFloat(.2f, .8f);
				dust.color = Color.DodgerBlue;
				dust.noGravity = true;
			}
			for (int i = 0; i < 120; i++) {
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<HeartBreakInstrument_Extra>());
				dust.position += Main.rand.NextVector2Circular(1000, 50).RotatedBy(Projectile.velocity.ToRotation());
				dust.velocity = Projectile.velocity * Main.rand.NextFloat(.5f, 1.1f);
				dust.rotation = dust.velocity.ToRotation();
				dust.scale = 1 + Main.rand.NextFloat(.2f, .8f);
				dust.color = Color.DodgerBlue;
				dust.noGravity = true;
			}
			Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<SimplePiercingProjectile2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 15, 30);
			if (proj.ModProjectile is SimplePiercingProjectile2 modproj) {
				modproj.ScaleX = 30;
				modproj.ScaleY = 1f;
				modproj.ProjectileColor = Color.DodgerBlue;
			}
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center, Main.rand.NextVector2CircularEdge(1, 1), ModContent.ProjectileType<HeartBreakInstrument_Slash_Projectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 4, 6);
		if (Projectile.ai[0] > 1)
			Projectile.ai[0]--;
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;
		Vector2 origin = texture.Size() * .5f;
		Texture2D texture2 = TextureAssets.Projectile[ProjectileID.PiercingStarlight].Value;
		float rotation = Projectile.rotation - MathHelper.PiOver2;
		ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);
		Main.EntitySpriteDraw(texture2, drawpos, null, Color.DodgerBlue, rotation, texture2.Size() * .5f, new Vector2(Projectile.scale * 5, Projectile.scale), SpriteEffects.None);
		Main.EntitySpriteDraw(texture2, drawpos, null, Color.White, rotation, texture2.Size() * .5f, new Vector2(Projectile.scale * 3, Projectile.scale), SpriteEffects.None);
		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		Main.EntitySpriteDraw(texture, drawpos, null, lightColor, rotation + MathHelper.PiOver4, origin, Projectile.scale, SpriteEffects.None);
		return false;
	}
}
public class HeartBreakInstrument_Special_CoolDown : ModBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<HeartBreakInstrument_ModPlayer>().OnCoolDown = true;
	}
}
public class HeartBreakerInstrument_Special : ModProjectile {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<HeartBreakInstrument>();
	public override void SetDefaults() {
		Projectile.width = 10;
		Projectile.height = 84;
		Projectile.extraUpdates = 10;
		Projectile.timeLeft = 7600;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.scale = .5f;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 10;
	}
	Vector2 OriginalPosition = Vector2.Zero;
	public override void OnSpawn(IEntitySource source) {
		OriginalPosition = Main.player[Projectile.owner].Center - Projectile.Center;
		Projectile.Center = Projectile.Center.Add(0, -100);
		Projectile.ai[0] = (Main.player[Projectile.owner].itemAnimationMax + 10) * Projectile.extraUpdates;
	}
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		if (Projectile.ai[1] == 0) {
			if (player.ItemAnimationActive && --Projectile.ai[0] < 0) {
				Projectile.ai[1] = 1;
				Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 5;
				Projectile.timeLeft = 300;
				return;
			}
			Vector2 dis = player.Center + OriginalPosition - Projectile.Center;
			Projectile.velocity *= .09f;
			if (!Projectile.Center.IsCloseToPosition(player.Center + OriginalPosition, 30)) {
				Projectile.velocity = dis.SafeNormalize(Vector2.Zero) * dis.Length() / 80f;
			}
			else {
				Projectile.velocity *= .098f;
			}
			Projectile.rotation = (Main.MouseWorld - Projectile.Center).ToRotation() + MathHelper.PiOver2;
		}
		else {
			if (Main.rand.NextBool(5)) {
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<HeartBreakInstrument_Extra>());
				dust.velocity = -Projectile.velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.5f, 1.1f);
				dust.rotation = dust.velocity.ToRotation();
				dust.scale = 1 + Main.rand.NextFloat(.2f, .8f);
				dust.color = Color.DodgerBlue;
				dust.noGravity = true;
			}
		}
	}
	public override bool? CanDamage() => Projectile.ai[1] == 1;
	public override bool PreDraw(ref Color lightColor) {
		if (Projectile.timeLeft >= 1000) {
			Projectile.ai[2] = Math.Clamp(Projectile.ai[2] + .1f, 0, 1);

		}
		else {
			Projectile.ai[2] = Math.Clamp(Projectile.timeLeft / 1000f, 0, 1);
		}
		lightColor *= Projectile.ai[2];
		DrawSelf(lightColor);
		return false;
	}
	private void DrawSelf(Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;
		Vector2 origin = texture.Size() * .5f;
		Texture2D texture2 = TextureAssets.Projectile[ProjectileID.PiercingStarlight].Value;
		float rotation = Projectile.rotation - MathHelper.PiOver2;
		ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);
		Main.EntitySpriteDraw(texture2, drawpos, null, Color.DodgerBlue, rotation, texture2.Size() * .5f, new Vector2(Projectile.scale * 5, Projectile.scale), SpriteEffects.None);
		Main.EntitySpriteDraw(texture2, drawpos, null, Color.White, rotation, texture2.Size() * .5f, new Vector2(Projectile.scale * 3, Projectile.scale), SpriteEffects.None);
		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		Main.EntitySpriteDraw(texture, drawpos, null, lightColor, rotation + MathHelper.PiOver4, origin, Projectile.scale, SpriteEffects.None);
	}
}
public abstract class HeartBreakInstrument_CommonDust : ModDust {
	public override bool Update(Dust dust) {
		dust.velocity *= .98f;
		dust.scale -= .001f;
		if (dust.scale <= 0) {
			dust.active = false;
		}
		else {
			dust.position += dust.velocity;
		}
		return base.Update(dust);
	}
	public override bool PreDraw(Dust dust) {
		Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
		Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, null, dust.color, dust.rotation, texture.Size() * .5f, dust.scale, SpriteEffects.None, 0);

		ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);
		Texture2D glow = ModContent.Request<Texture2D>(ModTexture.Glow_Big).Value;
		dust.color.A = 50;
		Main.spriteBatch.Draw(glow, dust.position - Main.screenPosition, null, dust.color, dust.rotation, glow.Size() * .5f, dust.scale, SpriteEffects.None, 0);
		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		return false;
	}
}
public class Crotchet : HeartBreakInstrument_CommonDust {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<Crotchet>();
}
public class Minim : HeartBreakInstrument_CommonDust {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<Minim>();
}
public class Quaver : HeartBreakInstrument_CommonDust {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<Quaver>();
}
public class SemiQuaver : HeartBreakInstrument_CommonDust {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<SemiQuaver>();
}
public class HeartBreakInstrument_Cross : ModDust {
	public override string Texture => ModTexture.CrossGlow;
	public override bool Update(Dust dust) {
		dust.velocity *= .98f;
		dust.scale -= .001f;
		if (dust.scale <= 0) {
			dust.active = false;
		}
		else {
			dust.position += dust.velocity;
		}
		return base.Update(dust);
	}
	public override bool PreDraw(Dust dust) {
		Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

		ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);
		Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, null, dust.color, dust.rotation, texture.Size() * .5f, dust.scale, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, null, Color.White, dust.rotation, texture.Size() * .5f, dust.scale * .5f, SpriteEffects.None, 0);
		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		return false;
	}
}
public class HeartBreakInstrument_Extra : ModDust {
	public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.PiercingStarlight);
	public override bool Update(Dust dust) {
		dust.velocity *= .98f;
		dust.scale -= .05f;
		if (dust.scale <= 0) {
			dust.active = false;
		}
		else {
			dust.position += dust.velocity;
		}
		return false;
	}
	public override bool PreDraw(Dust dust) {
		Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

		ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);
		Vector2 scaler = new Vector2(dust.scale, dust.scale * .5f);
		Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, null, dust.color, dust.rotation, texture.Size() * .5f, scaler, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, null, Color.White, dust.rotation, texture.Size() * .5f, scaler * .5f, SpriteEffects.None, 0);
		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		return false;
	}
}
public class Gclef : ModProjectile {
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 200;
		ProjectileID.Sets.TrailingMode[Type] = 2;
	}
	public override void SetDefaults() {
		Projectile.width = 26;
		Projectile.height = 62;
		Projectile.timeLeft = 1200;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.extraUpdates = 10;
		Projectile.light = .5f;
	}
	public override void OnSpawn(IEntitySource source) {
		tracker.Clear();
		Projectile.ai[2] = -1;
	}
	readonly int[] Notes = [
		ModContent.DustType<Crotchet>(),
		ModContent.DustType<Minim>(),
		ModContent.DustType<Quaver>(),
		ModContent.DustType<SemiQuaver>()
	];
	public void Set_KillState() {
		CanDecay = true;
		Projectile.timeLeft = 300;
	}
	bool CanDecay = false;
	public int MoveToCursorTime = 0;
	int DelayBeforeDecaySound = 0;
	Vector2 MousePos = Vector2.Zero;
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		if (player.dead || !player.active) {
			Projectile.Kill();
		}
		Projectile.velocity *= .99f;

		MovementHandler(player);

		if (!CanDecay) {
			Projectile.timeLeft = 1200;
		}

		if (MoveToCursorTime % 101 == 100) {
			HitNote(player);
		}

		if (Projectile.scale > 1) {
			Projectile.scale -= .005f;
		}
		else {
			Projectile.scale = 1f;
		}
		if (--DelayBeforeDecaySound <= 0) {
			DelayBeforeDecaySound = 0;
			if (Projectile.ai[2] > -1) {
				Projectile.ai[2] -= .01f;
			}
		}
	}
	private void MovementHandler(Player player) {
		Projectile.velocity *= .98f;
		Vector2 positionToBeAt = player.Center + Vector2.UnitX * 50 * -player.direction;
		if (!Projectile.Center.IsCloseToPosition(player.Center, 2000)) {
			Projectile.Center = player.Center;
		}
		if (--MoveToCursorTime > 0) {
			if (MousePos == Vector2.Zero) {
				MousePos = Main.MouseWorld;
			}
			if (!Projectile.Center.IsCloseToPosition(MousePos, 200)) {
				Projectile.velocity += (MousePos - Projectile.Center).SafeNormalize(Vector2.Zero) / 32f;
			}
			else if (!Projectile.Center.IsCloseToPosition(MousePos, 100)) {
				Projectile.velocity += (MousePos - Projectile.Center).SafeNormalize(Vector2.Zero) / 64f;
			}
			else {
				if (Projectile.Center.IsCloseToPosition(MousePos, 50)) {
					Projectile.velocity = (MousePos - Projectile.Center) + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(Projectile.ai[1])) * 48;
					Projectile.ai[1] += 1 * Math.Clamp(MoveToCursorTime / 400f, 0, 1f);
				}
				else {
					Projectile.ai[1] = MathHelper.ToDegrees((Projectile.Center - MousePos).ToRotation());
					Projectile.velocity += (MousePos - Projectile.Center).SafeNormalize(Vector2.Zero) / 128f;
				}
			}
		}
		else {
			MousePos = Vector2.Zero;
			MoveToCursorTime = 0;
			if (!Projectile.Center.IsCloseToPosition(positionToBeAt, 200)) {
				Projectile.velocity += (positionToBeAt - Projectile.Center).SafeNormalize(Vector2.Zero) / 32f;
			}
			else if (!Projectile.Center.IsCloseToPosition(positionToBeAt, 100)) {
				Projectile.velocity += (positionToBeAt - Projectile.Center).SafeNormalize(Vector2.Zero) / 64f;
			}
			else {
				Projectile.velocity += (positionToBeAt - Projectile.Center).SafeNormalize(Vector2.Zero) / 128f;
			}
		}
	}
	public void HitNote(Player player) {
		var modplayer = player.GetModPlayer<HeartBreakInstrument_ModPlayer>();
		if (!modplayer.ClimaxOfTradegy) {
			player.GetModPlayer<HeartBreakInstrument_ModPlayer>().ClimaxOfTradegy_Count++;
		}
		else {
			for (int i = 0; i < 3; i++) {
				Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(1, 1), ModContent.ProjectileType<HeartBreakInstrument_Slash_Projectile>(), (int)(Projectile.damage * .45f), 0, player.whoAmI, 5 * Main.rand.NextBool().ToDirectionInt(), 12, 5 * i);
				if (projectile.ModProjectile is HeartBreakInstrument_Slash_Projectile proj) {
					proj.ScaleX = 5f;
					proj.ScaleY = .35f;
					projectile.scale = 2;
				}
			}
		}
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, -Projectile.velocity.SafeNormalize(Vector2.Zero) * .25f, ModContent.ProjectileType<HeartBreakInstrument_Note_Projectile>(), (int)(Projectile.damage * .37f), Projectile.knockBack, Projectile.owner, Main.rand.Next(1, 5));
		Projectile.scale = 1.35f;
		Lighting.AddLight(Projectile.Center, Color.AliceBlue.ToVector3());
		Projectile.ai[2] = Math.Clamp(Projectile.ai[2] + .15f, -1, 1);
		DelayBeforeDecaySound = 120;
		SoundEngine.PlaySound(SoundID.Item35 with { Pitch = Projectile.ai[2] }, Projectile.Center);
		SpawnVisualTracker();
		Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 300);
		Dust note = Dust.NewDustDirect(Projectile.Center, 0, 0, Main.rand.Next(Notes));
		note.noGravity = true;
		note.velocity = Main.rand.NextVector2CircularEdge(1, 1);
		note.scale = Main.rand.NextFloat(.6f, 1);
		note.color = Color.White;

		foreach (NPC npc in npclist) {
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo(Projectile.damage, 1));
			HeartBreakInstrument_Tool.SpawnDust(npc);
		}
	}
	List<SpriteTracker> tracker = new();
	public override bool PreDraw(ref Color lightColor) {
		Color blue = Color.DodgerBlue;
		Color black = Color.Black;
		if (Projectile.timeLeft <= 300) {
			float per = Projectile.timeLeft / 300f;
			lightColor *= per;
			blue *= per;
			black *= per;
		}
		Main.instance.LoadProjectile(Type);
		Texture2D Projectile_texture = TextureAssets.Projectile[Type].Value;
		Vector2 Projectile_Origin = Projectile_texture.Size() * .5f;
		Vector2 Projectile_DrawPos = Projectile.position - Main.screenPosition + Projectile_Origin;

		ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);
		Texture2D glow = ModContent.Request<Texture2D>(ModTexture.Glow_Big).Value;
		Color blue2 = blue with { A = (byte)(blue.A * .7f) };
		Main.spriteBatch.Draw(glow, Projectile_DrawPos, null, blue2, Projectile.rotation, glow.Size() * .5f, 2, SpriteEffects.None, 0);

		ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);
		Texture2D Gclef_glow = ModContent.Request<Texture2D>(ModUtils.GetTheSameTextureAs<Gclef>("Gclef_Glow")).Value;
		Main.EntitySpriteDraw(Gclef_glow, Projectile_DrawPos, null, blue, Projectile.rotation, Gclef_glow.Size() * .5f, Projectile.scale, SpriteEffects.None, 0);

		Texture2D texture = ModContent.Request<Texture2D>(ModTexture.OuterInnerGlow).Value;
		Vector2 origin2 = texture.Size() * .5f;
		Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition + Projectile_Origin, null, blue, 0, origin2, 2f, SpriteEffects.None, 0);

		ModUtils.Draw_ResetToNormal(Main.spriteBatch);

		TrackerHandler(blue, lightColor, Projectile_Origin);

		Texture2D line = ModContent.Request<Texture2D>(ModUtils.GetTheSameTextureAs<Gclef>("StraightWhiteLine")).Value;
		Vector2 originLine = line.Size() * .5f;
		float len = Projectile.oldPos.Length;
		for (int k = 0; k < len; k++) {
			Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + Projectile_Origin;
			Color color2 = Color.Lerp(lightColor, blue, 1 - k / len) * ((Projectile.oldPos.Length - k) / len);
			Main.EntitySpriteDraw(line, drawPos, null, color2 * .35f, Projectile.rotation, originLine, Projectile.scale, SpriteEffects.None, 0);
		}
		Main.EntitySpriteDraw(Projectile_texture, Projectile_DrawPos, null, black, Projectile.rotation, Projectile_Origin, Projectile.scale, SpriteEffects.None, 0);

		return false;
	}
	private void SpawnVisualTracker() {
		SpriteTracker track = new(Vector2.Zero, 0, 30);
		track.position = Projectile.position;
		track.scale = 2;
		tracker.Add(track);
	}
	private void TrackerHandler(Color color, Color color2, Vector2 origin) {
		if (tracker == null) {
			tracker = new();
		}
		Texture2D texture = ModContent.Request<Texture2D>(ModTexture.OuterInnerGlow).Value;
		Vector2 origin2 = texture.Size() * .5f;
		for (int i = tracker.Count - 1; i >= 0; i--) {
			SpriteTracker tr = tracker[i];
			tr.position = Projectile.position;
			Color baseOnScale = color * (tr.TimeLeft / 20f);
			Color baseOnScale2 = color2 * (tr.TimeLeft / 20f);
			ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);
			Main.spriteBatch.Draw(texture, tr.position - Main.screenPosition + origin.Add(5, 5), null, Color.Black * (tr.TimeLeft / 60f), tr.rotation, origin2, tr.scale, SpriteEffects.None, 0);
			ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);
			Main.spriteBatch.Draw(texture, tr.position - Main.screenPosition + origin, null, baseOnScale * .5f, tr.rotation, origin2, tr.scale, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(texture, tr.position - Main.screenPosition + origin, null, baseOnScale2 * .5f, tr.rotation, origin2, tr.scale, SpriteEffects.None, 0);
			ModUtils.Draw_ResetToNormal(Main.spriteBatch);
			if (tr.TimeLeft >= 20) {
				tr.scale += .2f;
			}
			else if (tr.TimeLeft >= 10) {
				tr.scale += .1f;
			}
			else {
				tr.scale += .01f;
			}
			if (--tr.TimeLeft <= 0) {
				tracker.RemoveAt(i);
			}
			else {
				tracker[i] = tr;
			}
		}
	}
}
/// <summary>
/// Ai0 : shoot velocity<br/>
/// Ai1 : time left of a AI, recommend setting it above 0<br/>
/// </summary>
public class HeartBreakInstrument_Slash_Projectile : ModProjectile {
	public Color ProjectileColor = Color.DodgerBlue;
	public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.PiercingStarlight);
	float InitialScaleXValue = 0f;
	float InitialScaleYValue = 0f;
	public float ScaleX = 3f;
	public float ScaleY = 1f;
	public int TimeBeforeActive = 0;
	Vector2 CenterBefore = Vector2.Zero;
	float OffSetFromPlayer = 0;
	public bool FollowPlayer = false;
	Vector2 PlayerCenterOrigin = Vector2.Zero;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 36;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 60;
		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.idStaticNPCHitCooldown = 10;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
	}
	public override void OnSpawn(IEntitySource source) {
		if (Projectile.ai[0] <= 0) {
			Projectile.ai[0] = 1;
		}
		Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.ai[0];
		Projectile.rotation = Projectile.velocity.ToRotation();
		if (Projectile.ai[1] <= 0) {
			Projectile.ai[1] = 15;
		}
		TimeBeforeActive = (int)Projectile.ai[2];
		Projectile.timeLeft = (int)(Projectile.ai[1] + TimeBeforeActive);
		CenterBefore = Projectile.Center;
		OffSetFromPlayer = (CenterBefore - Main.player[Projectile.owner].Center).Length();
		PlayerCenterOrigin = Main.player[Projectile.owner].Center;
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		if (Projectile.ai[2] >= 0) {
			return false;
		}
		var pointEdgeOfProjectile = Projectile.Center.IgnoreTilePositionOFFSET(Projectile.rotation.ToRotationVector2(), 18 * ScaleX * Projectile.scale);
		var pointEdgeOfProjectile2 = Projectile.Center.IgnoreTilePositionOFFSET((Projectile.rotation + MathHelper.Pi).ToRotationVector2(), 18 * ScaleX * Projectile.scale);
		return ModUtils.Collision_PointAB_EntityCollide(targetHitbox, pointEdgeOfProjectile, pointEdgeOfProjectile2);
	}
	public override Color? GetAlpha(Color lightColor) {
		ProjectileColor.A = 0;
		return ProjectileColor * (Projectile.timeLeft / Projectile.ai[1]);
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		HeartBreakInstrument_Tool.SpawnDust(target);
	}
	public override void AI() {
		if (--Projectile.ai[2] >= 0) {
			Projectile.Center = CenterBefore;
			return;
		}
		if (FollowPlayer) {
			Player player = Main.player[Projectile.owner];
			Projectile.Center = player.Center.PositionOFFSET(Projectile.velocity, OffSetFromPlayer) + CenterBefore - PlayerCenterOrigin;
			OffSetFromPlayer += Projectile.velocity.Length();
		}
		float timeleft = Projectile.Get_ProjectileTimeInitial() - TimeBeforeActive;
		if (Projectile.timeLeft == timeleft) {
			InitialScaleXValue = ScaleX;
			InitialScaleYValue = ScaleY;
			float extraScaleX = ScaleX * .5f;
			//for (int i = 0; i < 40; i++) {
			//	var dust = Dust.NewDustDirect(ModUtils.NextPointOn2Vector2(Projectile.Center.PositionOFFSET(Projectile.velocity, 36 * extraScaleX), Projectile.Center.PositionOFFSET(Projectile.velocity, -36 * extraScaleX)), 0, 0, ModContent.DustType<AkaiHanbunNoHasami_Dust2>());
			//	dust.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver2 * Main.rand.NextBool().ToDirectionInt()).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(3, 5);
			//	dust.scale = Main.rand.NextFloat(.2f, .35f);
			//	dust.color = Color.Black with { A = 150 };
			//	dust.rotation += Main.rand.NextFloat();
			//}
		}
		ScaleX = InitialScaleXValue * (Projectile.timeLeft / timeleft);
		ScaleY = InitialScaleYValue * (Projectile.timeLeft / timeleft);
	}
	public override bool PreDraw(ref Color lightColor) {
		if (Projectile.ai[2] < 0) {
			Main.instance.LoadProjectile(ProjectileID.PiercingStarlight);
			var texture = TextureAssets.Projectile[ProjectileID.PiercingStarlight].Value;
			var origin = texture.Size() * .5f;
			var drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			Main.EntitySpriteDraw(texture, drawPos, null, Color.DodgerBlue with { A = 100 }, Projectile.rotation, origin, new Vector2(ScaleX, ScaleY) * Projectile.scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, drawPos, null, Color.White with { A = 200 }, Projectile.rotation, origin, new Vector2(ScaleX, ScaleY) * Projectile.scale * .5f, SpriteEffects.None, 0);
		}
		return false;
	}
}
