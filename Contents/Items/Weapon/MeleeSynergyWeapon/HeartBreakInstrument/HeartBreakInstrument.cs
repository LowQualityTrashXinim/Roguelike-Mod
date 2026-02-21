using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Roguelike.Common.Global;
using Roguelike.Common.Global.Mechanic.OutroEffect;
using Roguelike.Common.Global.Mechanic.OutroEffect.Contents;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Utils;
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
		Item.BossRushDefaultMeleeShootCustomProjectile(102, 102, 55, 4f, 24, 24, ItemUseStyleID.Swing, 1, 1f, true);
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
			if (player.altFunctionUse == 2) {
				return base.CanUseItem(player);
			}
			else {
				var overhaul = Item.GetGlobalItem<MeleeWeaponOverhaul>();
				ComboChain_ResetTimer = 120 + player.itemAnimationMax;
				switch (++ComboChain_Count) {
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
				if (ComboChain_Count >= 5) {
					ComboChain_Count = 0;
				}
			}
		}
		return base.CanUseItem(player);
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
		if (ClimaxOfTragedy) {
			heartplayer.ClimaxOfTradegy_Count++;
		}
		if (player.altFunctionUse == 2) {
			if (!ClimaxOfTragedy && heartplayer.ClimaxOfTradegy_Count >= 30) {
				heartplayer.ClimaxOfTradegy = true;
				heartplayer.ClimaxOfTradegy_Count = 0;
				heartplayer.Effect = false;
				int type_Instrument = ModContent.ProjectileType<HeartBreakInstrument_InstrumentProjectile>();
				if (heartplayer.Instrument1_WhoAmI == -1) {
					heartplayer.Instrument1_WhoAmI = Projectile.NewProjectile(source, position, velocity, type_Instrument, damage, knockback, player.whoAmI, ItemID.IvyGuitar, 1);
				}
				if (heartplayer.Instrument2_WhoAmI == -1) {
					heartplayer.Instrument2_WhoAmI = Projectile.NewProjectile(source, position, velocity, type_Instrument, damage, knockback, player.whoAmI, ItemID.Piano, 2);
				}
				if (heartplayer.Instrument3_WhoAmI == -1) {
					heartplayer.Instrument3_WhoAmI = Projectile.NewProjectile(source, position, velocity, type_Instrument, damage, knockback, player.whoAmI, ItemID.Harp, 3);
				}
				if (heartplayer.Instrument4_WhoAmI == -1) {
					heartplayer.Instrument4_WhoAmI = Projectile.NewProjectile(source, position, velocity, type_Instrument, damage, knockback, player.whoAmI, ItemID.Bell, 4);
				}
				return;
			}
			else {
				ComboChain_Count = 1;
				if (!player.GetModPlayer<HeartBreakInstrument_ModPlayer>().OnCoolDown) {
					type = ModContent.ProjectileType<HeartBreakerInstrument_Special>();
					Projectile.NewProjectileDirect(source, position.Add(-30, -30), Vector2.Zero, type, damage, knockback, player.whoAmI, 1);
					Projectile.NewProjectileDirect(source, position.Add(-10, -50), Vector2.Zero, type, damage, knockback, player.whoAmI, 2);
					Projectile.NewProjectileDirect(source, position.Add(10, -50), Vector2.Zero, type, damage, knockback, player.whoAmI, 3);
					Projectile.NewProjectileDirect(source, position.Add(30, -30), Vector2.Zero, type, damage, knockback, player.whoAmI, 4);
					player.AddBuff<HeartBreakInstrument_Special_CoolDown>(ModUtils.ToSecond(15));
					return;
				}
			}
		}
		var pos = Main.MouseWorld;
		if ((pos - player.Center).LengthSquared() > 5625) {
			pos = position.PositionOFFSET(velocity, 75f);
		}
		int count = ComboChain_Count;

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
				projectile = Projectile.NewProjectileDirect(source, post, velocity.Vector2RotateByRandom(10), type, damage, knockback, player.whoAmI, 20, 15, i * 4);
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
					projectile = Projectile.NewProjectileDirect(source, post, velocity.Vector2RotateByRandom(30), type, damage, knockback, player.whoAmI, 20, 15, i * 4);
					if (projectile.ModProjectile is HeartBreakInstrument_Slash_Projectile proj3) {
						proj3.ScaleX = 7f;
						proj3.ScaleY = .5f;
						proj3.FollowPlayer = true;
						projectile.scale = 2;
					}
				}
			}
			Gclef proj2 = Get_ModProjectile();
			if (proj2 != null) {
				proj2.HitNote(player);
			}
		}
		else if (count == 4) {
			ComboChain_CoolDownAttack = ModUtils.ToSecond(1) + player.itemAnimationMax;
			ComboChain_ResetTimer += ComboChain_CoolDownAttack;
			Gclef proj = Get_ModProjectile();
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
				projectile = Projectile.NewProjectileDirect(source, pos.PositionOFFSET(velocity, 35f) + Main.rand.NextVector2Circular(15, 15), vel, type, damage, knockback, player.whoAmI, 5 * Main.rand.NextBool().ToDirectionInt(), 9, 25 + i * 3);
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
			Gclef proj2 = Get_ModProjectile();
			if (proj2 != null) {
				proj2.HitNote(player);
			}
		}
	}
	private Gclef Get_ModProjectile() {
		if (Projectile_WhoAmI <= -1 || Projectile_WhoAmI > 1000) {
			return null;
		}
		return (Gclef)Main.projectile[Projectile_WhoAmI].ModProjectile;
	}
	public override void SynergyUpdateInventory(Player player, PlayerSynergyItemHandle modplayer) {
		ComboChain_ResetTimer = ModUtils.CountDown(ComboChain_ResetTimer);
		ComboChain_CoolDownAttack = ModUtils.CountDown(ComboChain_CoolDownAttack);
		if (ComboChain_ResetTimer <= 0) {
			ComboChain_Count = 0;
		}
		if (!player.IsHeldingModItem<HeartBreakInstrument>()) {
			Gclef proj = Get_ModProjectile();
			if (proj != null) {
				proj.Set_KillState();
			}
			Projectile_WhoAmI = -1;
			player.GetModPlayer<HeartBreakInstrument_ModPlayer>().ClimaxOfTradegy_Count = 0;
			player.GetModPlayer<HeartBreakInstrument_ModPlayer>().ClimaxOfTradegy = false;
			player.GetModPlayer<HeartBreakInstrument_ModPlayer>().Effect = false;
			player.GetModPlayer<HeartBreakInstrument_ModPlayer>().KillProjectile();

		}
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) {
		HeartBreakInstrument_Tool.SpawnDust(target);
	}
	int Projectile_WhoAmI = -1;
	public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
		if (WeaponEffect_ModPlayer.Check_ValidForIntroEffect(player) && player.Check_SwitchedWeapon(Type)) {
			WeaponEffect_ModPlayer.Set_IntroEffect(player, Type, 1);
			player.GetModPlayer<HeartBreakInstrument_ModPlayer>().IntroEffectActivate = true;
		}
		if (player.ItemAnimationActive) {
			player.GetModPlayer<HeartBreakInstrument_ModPlayer>().IntroEffectActivate = false;
		}

		int type = ModContent.ProjectileType<Gclef>();
		if (player.ownedProjectileCounts[ModContent.ProjectileType<Gclef>()] < 1) {
			if (Projectile_WhoAmI == -1) {
				Projectile_WhoAmI = Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, type, (int)(player.GetWeaponDamage(Item) * .55f), 0, player.whoAmI);
			}
			else {
				if (!Main.projectile[Projectile_WhoAmI].active) {
					Projectile_WhoAmI = -1;
				}
			}
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
	public int Instrument1_WhoAmI = -1;
	public int Instrument2_WhoAmI = -1;
	public int Instrument3_WhoAmI = -1;
	public int Instrument4_WhoAmI = -1;
	public bool IntroEffectActivate = false;
	public override void ResetEffects() {
		OnCoolDown = false;
		CheckProjectileActive();
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
				var Proj1 = GetInstrument1();
				var Proj2 = GetInstrument2();
				var Proj3 = GetInstrument3();
				var Proj4 = GetInstrument4();
				if (Main.mouseLeft) {
					if (Proj1 != null) {
						Proj1.DoSkillAttack = true;
					}
					if (Proj2 != null) {
						Proj2.DoSkillAttack = true;
					}
					if (Proj3 != null) {
						Proj3.DoSkillAttack = true;
					}
					if (Proj4 != null) {
						Proj4.DoSkillAttack = true;
					}
					ClimaxOfTradegy_Count = 0;
					ClimaxOfTradegy = false;
					Effect = false;
					Instrument1_WhoAmI = -1;
					Instrument2_WhoAmI = -1;
					Instrument3_WhoAmI = -1;
					Instrument4_WhoAmI = -1;
				}
				else if (Main.mouseRight) {
					if (Proj1 != null) {
						Proj1.DoHealAttack = true;
					}
					if (Proj2 != null) {
						Proj2.DoHealAttack = true;
					}
					if (Proj3 != null) {
						Proj3.DoHealAttack = true;
					}
					if (Proj4 != null) {
						Proj4.DoHealAttack = true;
					}
					ClimaxOfTradegy_Count = 0;
					ClimaxOfTradegy = false;
					Effect = false;
					Instrument1_WhoAmI = -1;
					Instrument2_WhoAmI = -1;
					Instrument3_WhoAmI = -1;
					Instrument4_WhoAmI = -1;
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
	public void KillProjectile() {
		var Proj1 = GetInstrument1();
		var Proj2 = GetInstrument2();
		var Proj3 = GetInstrument3();
		var Proj4 = GetInstrument4();
		if (Proj1 != null) {
			Proj1.Projectile.Kill();
		}
		if (Proj2 != null) {
			Proj2.Projectile.Kill();
		}
		if (Proj3 != null) {
			Proj3.Projectile.Kill();
		}
		if (Proj4 != null) {
			Proj4.Projectile.Kill();
		}
		Instrument1_WhoAmI = -1;
		Instrument2_WhoAmI = -1;
		Instrument3_WhoAmI = -1;
		Instrument4_WhoAmI = -1;
	}

	private HeartBreakInstrument_InstrumentProjectile GetInstrument1() {
		if (Instrument1_WhoAmI >= 0 && Instrument1_WhoAmI < 1000) {
			return (HeartBreakInstrument_InstrumentProjectile)Main.projectile[Instrument1_WhoAmI].ModProjectile;
		}
		return null;
	}
	private HeartBreakInstrument_InstrumentProjectile GetInstrument2() {
		if (Instrument2_WhoAmI >= 0 && Instrument2_WhoAmI < 1000) {
			return (HeartBreakInstrument_InstrumentProjectile)Main.projectile[Instrument2_WhoAmI].ModProjectile;
		}
		return null;
	}
	private HeartBreakInstrument_InstrumentProjectile GetInstrument3() {
		if (Instrument3_WhoAmI >= 0 && Instrument3_WhoAmI < 1000) {
			return (HeartBreakInstrument_InstrumentProjectile)Main.projectile[Instrument3_WhoAmI].ModProjectile;
		}
		return null;
	}
	private HeartBreakInstrument_InstrumentProjectile GetInstrument4() {
		if (Instrument4_WhoAmI >= 0 && Instrument4_WhoAmI < 1000) {
			return (HeartBreakInstrument_InstrumentProjectile)Main.projectile[Instrument4_WhoAmI].ModProjectile;
		}
		return null;
	}
	private void CheckProjectileActive() {
		if (Instrument1_WhoAmI >= 0 && Instrument1_WhoAmI < 1000) {
			if (!Main.projectile[Instrument1_WhoAmI].active) {
				Instrument1_WhoAmI = -1;
			}
		}
		if (Instrument2_WhoAmI >= 0 && Instrument2_WhoAmI < 1000) {
			if (!Main.projectile[Instrument2_WhoAmI].active) {
				Instrument2_WhoAmI = -1;
			}
		}
		if (Instrument3_WhoAmI >= 0 && Instrument3_WhoAmI < 1000) {
			if (!Main.projectile[Instrument3_WhoAmI].active) {
				Instrument3_WhoAmI = -1;
			}
		}
		if (Instrument4_WhoAmI >= 0 && Instrument4_WhoAmI < 1000) {
			if (!Main.projectile[Instrument4_WhoAmI].active) {
				Instrument4_WhoAmI = -1;
			}
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
public class HeartBreakInstrument_InstrumentProjectile : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 30;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 6000;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
	}
	int TextureItemID = -1;
	int DelayBeforeDecaySound = 0;
	public bool DoSkillAttack = false;
	public bool DoHealAttack = false;
	public float Index = 0;
	public override void OnSpawn(IEntitySource source) {
		TextureItemID = (int)Projectile.ai[0];
		Projectile.ai[0] = 0;
		Index = Projectile.ai[1];
		Projectile.ai[1] = 0;
	}
	public override void AI() {
		Projectile.velocity *= .98f;

		if (DoSkillAttack) {
			if (++Projectile.ai[1] < 100) {
				Projectile.velocity *= .97f;
				Projectile.timeLeft = 300;
			}
			else {
				Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * (Projectile.ai[1] - 100) * .1f;
				if (Projectile.ai[1] >= 250) {
					Projectile.ai[1] = 250;
				}
				if (Projectile.Center.IsCloseToPosition(Main.MouseWorld, 50)) {
					Projectile.Kill();
				}
			}
			return;
		}
		if (DoHealAttack) {
			if (++Projectile.ai[1] < 100) {
				Projectile.velocity *= .97f;
				Projectile.timeLeft = 120;
			}
			else {
				Player player = Main.player[Projectile.owner];
				Vector2 dis = player.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(Projectile.ai[1]) * MathHelper.PiOver4 * Index) * 75 - Projectile.Center;
				Projectile.velocity = dis.SafeNormalize(Vector2.Zero) * dis.Length() / 32f;
			}
			return;
		}
		Vector2 distance = Main.player[Projectile.owner].Center + Vector2.One.RotatedBy(Index * MathHelper.PiOver4 + MathHelper.PiOver2 + MathHelper.PiOver4 * .5f) * 150 - Projectile.Center;
		Projectile.velocity = distance.SafeNormalize(Vector2.Zero) * distance.Length() / 32f;

		Attack();

		if (Projectile.scale > 1) {
			Projectile.scale -= .01f;
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
		if (Projectile.timeLeft <= 100) {
			return;
		}
		SoundStyle sound;
		int CD = 0;
		int damage = Projectile.damage;
		if (TextureItemID == ItemID.Harp) {
			sound = SoundID.Item26;
			CD = 60;
			damage = (int)(damage * 1.4f);
		}
		else if (TextureItemID == ItemID.Bell) {
			sound = SoundID.Item35;
			CD = 45;
		}
		else if (TextureItemID == ItemID.Piano) {
			sound = SoundID.Item35;
			CD = 30;
			damage = (int)(damage * .9f);
		}
		else if (TextureItemID == ItemID.IvyGuitar) {
			sound = Main.rand.Next(HeartBreakInstrument_Tool.Guitar);
			CD = 25;
			damage = (int)(damage * .85f);
		}
		if (++Projectile.ai[1] < CD) {
			return;
		}
		Projectile.ai[1] = 0;
		SpawnVisualTracker();
		Projectile.scale += .2f;
		Projectile.ai[2] = Math.Clamp(Projectile.ai[2] + .15f, -1, 1);
		DelayBeforeDecaySound = 120;
		SoundEngine.PlaySound(SoundID.Item35 with { Pitch = Projectile.ai[2] }, Projectile.Center);
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero), ModContent.ProjectileType<HeartBreakInstrument_Note_Projectile>(), damage, Projectile.knockBack, Projectile.owner, Main.rand.Next(1, 5));

	}
	public override bool? CanDamage() {
		return false;
	}
	List<SpriteTracker> tracker = new();
	public override void OnKill(int timeLeft) {
		Player player = Main.player[Projectile.owner];
		if (DoSkillAttack) {
			Projectile.Center.LookForHostileNPC(out List<NPC> listnpc, 500);
			foreach (var npc in listnpc) {
				player.StrikeNPCDirect(npc, npc.CalculateHitInfo(Projectile.damage * 15, ModUtils.DirectionFromEntityAToEntityB(npc.Center.X, Projectile.Center.X)));
			}
		}
		if (DoHealAttack) {
			player.Heal(100 + (int)(player.statLifeMax2 * .05f));
			player.AddBuff<HeartBreakInstrument_Buff>(ModUtils.ToSecond(15));
		}
		Vector2 ran = Main.rand.NextVector2Circular(Projectile.width, Projectile.height) * .75f;
		Dust dust = Dust.NewDustDirect(Projectile.Center + ran, 0, 0, ModContent.DustType<HeartBreakInstrument_Cross>());
		dust.noGravity = true;
		dust.velocity = Vector2.Zero;
		dust.rotation = MathHelper.ToRadians(Main.rand.NextFloat(1, 90));
		dust.color = Color.DodgerBlue;
		dust.scale = Main.rand.NextFloat(3.1f, 4.3f);
		for (int i = 0; i < 15; i++) {
			dust = Dust.NewDustDirect(Projectile.Center + ran, 0, 0, ModContent.DustType<HeartBreakInstrument_Cross>());
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2Circular(15, 15);
			dust.rotation = MathHelper.ToRadians(Main.rand.NextFloat(1, 90));
			dust.color = Color.DodgerBlue;
			dust.scale = Main.rand.NextFloat(1.4f, 1.6f);
		}
		for (int i = 0; i < 50; i++) {
			dust = Dust.NewDustDirect(Projectile.Center + ran, 0, 0, ModContent.DustType<HeartBreakInstrument_Extra>());
			dust.velocity = Main.rand.NextVector2Circular(20, 20);
			dust.rotation = dust.velocity.ToRotation();
			dust.scale = Main.rand.NextFloat(1.4f, 1.8f);
			dust.color = Color.DodgerBlue;
			dust.noGravity = true;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		if (TextureItemID <= 0 || TextureItemID > ItemID.Count) {
			return false;
		}

		if (Projectile.timeLeft <= 100) {
			lightColor *= Projectile.timeLeft / 100f;
		}

		Main.instance.LoadItem(TextureItemID);
		Texture2D texture = TextureAssets.Item[TextureItemID].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;

		TrackerHandler(lightColor, lightColor, origin);

		Main.EntitySpriteDraw(texture, drawpos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
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
public class HeartBreakInstrument_Buff : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
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
		Projectile.timeLeft = 900;
		Projectile.extraUpdates = 10;
	}
	float TextureIDSelf = -1;
	public override void OnSpawn(IEntitySource source) {
		TextureIDSelf = Projectile.ai[0];
	}
	float VelocityLength = 0;
	public override void AI() {
		if (Projectile.Center.LookForHostileNPC(out NPC npc, 750)) {
			if (VelocityLength == 0) {
				VelocityLength = Projectile.velocity.Length();
			}
			if (VelocityLength >= 2) {
				VelocityLength *= .995f;
			}
			else {
				VelocityLength += .001f;
			}
			Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * VelocityLength;
		}
		else {
			Projectile.velocity *= .995f;
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
			Vector2 drawPosDot = Projectile.oldPos[k] - Main.screenPosition + origin;
			Color color2 = lightColor * ((Projectile.oldPos.Length - k) / len);
			Main.EntitySpriteDraw(line, drawPosDot, null, color2, 0, originLine, 1f, SpriteEffects.None, 0);
		}
		return false;
	}
}
public class HeartBreakInstrument_Special_CoolDown : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<HeartBreakInstrument_ModPlayer>().OnCoolDown = true;
	}
}
public class HeartBreakerInstrument_Special : ModProjectile {
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = 10;
		Projectile.height = 84;
		Projectile.extraUpdates = 10;
		Projectile.timeLeft = 7600;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.scale = .75f;
	}
	Vector2 OriginalPosition = Vector2.Zero;
	float index = 0;
	public override void OnSpawn(IEntitySource source) {
		OriginalPosition = Main.player[Projectile.owner].Center - Projectile.Center;
		Projectile.Center = Projectile.Center.Add(0, -300);
		index = Projectile.ai[0];
		Projectile.ai[0] = 0;
	}
	public override void AI() {
		Projectile.localAI[0] = 0;
		Player player = Main.player[Projectile.owner];
		Vector2 dis = player.Center + OriginalPosition - Projectile.Center;
		Projectile.velocity *= .09f;
		if (!Projectile.Center.IsCloseToPosition(player.Center + OriginalPosition, 30)) {
			Projectile.velocity = dis.SafeNormalize(Vector2.Zero) * dis.Length() / 80f;
		}
		else {
			Projectile.velocity *= .098f;
		}
		Projectile.rotation = MathHelper.Pi;
		if (--Projectile.ai[1] <= 0 && player.ItemAnimationActive) {
			Projectile.ai[1] = (ModUtils.ToSecond(1) + Main.rand.Next(1, 30)) * 10;
			Vector2 vel = Main.rand.NextVector2Circular(15, 15);
			Vector2 pos = Main.MouseWorld + vel * Main.rand.NextFloat(2, 3);
			if (!player.Center.IsCloseToPosition(pos, 150)) {
				pos = player.Center.IgnoreTilePositionOFFSET((Main.MouseWorld - player.Center), 150) + vel * Main.rand.NextFloat(2, 3);
			}
			Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), pos, -vel, ModContent.ProjectileType<HeartBreakInstrument_Slash_Projectile>(), (int)(Projectile.damage * .45f), 0, player.whoAmI, 5 * Main.rand.NextBool().ToDirectionInt(), 12, index * 5);
			if (projectile.ModProjectile is HeartBreakInstrument_Slash_Projectile proj) {
				proj.ScaleX = 2f;
				proj.ScaleY = .25f;
				projectile.scale = 2;
			}
		}

	}
	public override bool? CanDamage() => false;
	public override bool PreDraw(ref Color lightColor) {
		Player player = Main.player[Projectile.owner];

		if (Projectile.timeLeft >= 1000) {
			Projectile.ai[2] = Math.Clamp(Projectile.ai[2] + .1f, 0, 1);

		}
		else {
			Projectile.ai[2] = Math.Clamp(Projectile.timeLeft / 1000f, 0, 1);
		}
		lightColor *= Projectile.ai[2];

		ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);

		Main.instance.LoadProjectile(Type);
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawPos = Projectile.Center - Main.screenPosition;
		SpriteEffects effect = SpriteEffects.None;
		if (player.ItemAnimationActive) {
			lightColor *= .2f;
			for (int i = 0; i < 5; i++) {
				Main.EntitySpriteDraw(texture, drawPos + Main.rand.NextVector2Circular(10, 10), null, lightColor, Projectile.rotation, origin, Projectile.scale, effect);
			}
		}
		else {
			Main.EntitySpriteDraw(texture, drawPos.Add(2, 0), null, lightColor * .6f, Projectile.rotation, origin, Projectile.scale, effect);
			Main.EntitySpriteDraw(texture, drawPos.Add(-2, 0), null, lightColor * .6f, Projectile.rotation, origin, Projectile.scale, effect);
		}
		Texture2D line = ModContent.Request<Texture2D>(ModTexture.WHITEDOT).Value;
		Vector2 originLine = line.Size() * .5f;
		float len = Projectile.oldPos.Length;
		for (int k = 0; k < len; k++) {
			Vector2 drawPosDot = Projectile.oldPos[k].Add(0, Projectile.height * -.3f * Projectile.scale) - Main.screenPosition + origin;
			Color color2 = lightColor * ((Projectile.oldPos.Length - k) / len);
			Main.EntitySpriteDraw(line, drawPosDot, null, color2, 0, originLine, 1f, SpriteEffects.None, 0);
		}
		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		return false;
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
			Projectile.Center = Main.player[Projectile.owner].Center.PositionOFFSET(Projectile.velocity, OffSetFromPlayer);
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
