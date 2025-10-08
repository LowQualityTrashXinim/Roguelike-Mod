using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent;
using Roguelike.Contents.Items;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Chest;
using Roguelike.Texture;
using Roguelike.Common.Global;
using Roguelike.Contents.NPCs.LootBoxLord.HostileProjectile;

namespace Roguelike.Contents.NPCs.LootBoxLord; 
internal class LootBoxLord : ModNPC {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<WoodenLootBox>();
	public override void SetStaticDefaults() {
		NPCID.Sets.DontDoHardmodeScaling[Type] = true;
		NPCID.Sets.NeedsExpertScaling[Type] = false;
	}
	public override void SetDefaults() {
		NPC.lifeMax = 54000;
		NPC.damage = 150;
		NPC.defense = 50;
		NPC.width = 38;
		NPC.height = 30;
		NPC.HitSound = SoundID.NPCHit4;
		NPC.DeathSound = SoundID.NPCDeath1;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.netAlways = true;
		NPC.knockBackResist = 0f;
		NPC.boss = true;
		NPC.dontTakeDamage = true;
		NPC.strengthMultiplier = 1;
		NPC.despawnEncouraged = false;
		NPC.friendly = false;
		NPC.dontTakeDamageFromHostiles = true;
		NPC.ScaleStats_UseStrengthMultiplier(1);
		NPC.GetGlobalNPC<RoguelikeGlobalNPC>().NPC_SpecialException = true;
	}
	public override void ModifyNPCLoot(NPCLoot npcLoot) {
		for (int i = 0; i < TerrariaArrayID.MeleePreBoss.Length; i++) {
			npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MeleePreBoss[i], 10));
		}
		for (int i = 0; i < TerrariaArrayID.MeleePreEoC.Length; i++) {
			npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MeleePreEoC[i], 10));
		}
		for (int i = 0; i < TerrariaArrayID.MeleeEvilBoss.Length; i++) {
			npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MeleeEvilBoss[i], 10));
		}
		for (int i = 0; i < TerrariaArrayID.RangePreBoss.Length; i++) {
			npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.RangePreBoss[i], 10));
		}
		for (int i = 0; i < TerrariaArrayID.RangePreEoC.Length; i++) {
			npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.RangePreEoC[i], 10));
		}
		for (int i = 0; i < TerrariaArrayID.RangeSkele.Length; i++) {
			npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.RangeSkele[i], 10));
		}
		for (int i = 0; i < TerrariaArrayID.MagicPreBoss.Length; i++) {
			npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MagicPreBoss[i], 10));
		}
		for (int i = 0; i < TerrariaArrayID.MagicPreEoC.Length; i++) {
			npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MagicPreEoC[i], 10));
		}
		for (int i = 0; i < TerrariaArrayID.MagicSkele.Length; i++) {
			npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MagicSkele[i], 10));
		}
		for (int i = 0; i < TerrariaArrayID.SummonPreBoss.Length; i++) {
			npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.SummonPreBoss[i], 10));
		}
		for (int i = 0; i < TerrariaArrayID.SummonerPreEoC.Length; i++) {
			npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.SummonerPreEoC[i], 10));
		}
		for (int i = 0; i < TerrariaArrayID.SummonSkele.Length; i++) {
			npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.SummonSkele[i], 10));
		}
		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PowerEnergy>()));
	}
	public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment) {
		NPC.lifeMax = 54000;
		NPC.life = NPC.lifeMax;
		NPC.damage = 100;
		NPC.defense = 50;
	}
	public override bool? CanBeHitByProjectile(Projectile projectile) {
		if (projectile.hostile) {
			return false;
		}
		return !IsTeleporting;
	}
	public override bool? CanBeHitByItem(Player player, Item item) {
		return !IsTeleporting;
	}
	Vector2 TeleportPosition = Vector2.Zero;
	Vector2 ChoosenPosition = Vector2.Zero;
	public int TeleportTime = 0;
	public const int TeleportDuration = 60;
	/// <summary>
	/// Return true when this NPC are teleporting
	/// </summary>
	public bool IsTeleporting => TeleportTime != 0;
	public bool Teleporting(Vector2 point) {
		TeleportPosition = point;
		if (++TeleportTime <= TeleportDuration) {
			return false;
		}
		NPC.TeleportCommon(TeleportPosition);
		ChoosenPosition = Vector2.Zero;
		TeleportPosition = Vector2.Zero;
		TeleportTime = 0;
		return true;
	}
	public int BossDamagePercentage(float percentage) => (int)Math.Ceiling(NPC.damage * percentage);
	//Use NPC.ai[0] to delay attack
	//Use NPC.ai[1] to switch attack
	//Use NPC.ai[2] for calculation
	//Use NPC.ai[3] to do movement
	bool AlreadySaidThat = false;
	int dialogNumber = 0;
	int dialogCD = 0;
	bool BeforeAttack = true;
	public void Dialog() {
		if (--dialogCD > 0) {
			return;
		}
		string dialog = "";
		var color = Color.White;
		switch (dialogNumber) {
			case 0:
				dialog = "I recognize you";
				break;
			case 1:
				dialog = "... I see";
				break;
			case 2:
				dialog = "Very well, I will entertain you";
				break;
			case 3:
				dialog = "Don't die too soon";
				color = Color.Red;
				BeforeAttack = false;
				NPC.dontTakeDamage = false;
				UniversalAttackCoolDown = 60;
				break;
		}
		dialogCD = 120;
		dialogNumber++;
		ModUtils.CombatTextRevamp(NPC.Hitbox, color, dialog);
	}

	public override void AI() {
		if (BeforeAttack) {
			Dialog();
			return;
		}
		Lighting.AddLight(NPC.Center, 1, 1, 1);
		var player = Main.player[NPC.target];
		if (player.dead || !player.active) {
			NPC.FindClosestPlayer();
			NPC.TargetClosest();
			player = Main.player[NPC.target];
			if (player.dead || !player.active) {
				NPC.active = false;
			}
		}
		if (NPC.CountNPCS(Type) > 1) {
			if (!AlreadySaidThat) {
				ModUtils.CombatTextRevamp(NPC.Hitbox, Color.Red, "You think so lowly of me ...");
				AlreadySaidThat = true;
				if (!NPC.AnyNPCs(ModContent.NPCType<ElderGuardian>()))
					NPC.NewNPC(NPC.GetSource_FromAI(), NPC.Hitbox.X, NPC.Hitbox.Y, ModContent.NPCType<ElderGuardian>());
			}
		}
		if (UniversalAttackCoolDown > 0) {
			UniversalAttackCoolDown--;
			return;
		}
		//TODO : change phase when boss hp is below 50%
		//Move above the player
		switch (CurrentAttack) {
			case 0:
				Move(player);
				break;
			case 1:
				ShootShortSword();
				break;
			case 2:
				ShootShortSword2(player);
				break;
			case 3:
				ShootBroadSword(player);
				break;
			case 4:
				ShootBroadSword2(player);
				break;
			case 5:
				ShootWoodBow(player);
				break;
			case 6:
				ShootWoodBow2();
				break;
			case 7:
				ShootStaff(player);
				break;
			case 8:
				ShootStaff2(player);
				break;
			case 9:
				ShootOreBow1(player);
				break;
			case 10:
				ShootOreBow2(player);
				break;
			case 11:
				ShootGun(player);
				break;
			case 12:
				ShootGun2(player);
				break;
		}
	}
	int lifeCounter = 0;
	public override void PostAI() {
		var player = Main.player[NPC.target];
		if (!ModUtils.CompareSquareFloatValue(NPC.Center, player.Center, 1000 * 1000)) {
			if (++lifeCounter < 120) {
				return;
			}
			NPC.life = Math.Clamp(NPC.life + 1, 0, NPC.lifeMax);
			if (Main.rand.NextBool(5)) {
				int dust = Dust.NewDust(NPC.Center + Main.rand.NextVector2Circular(30, 30), 0, 0, DustID.HealingPlus, Scale: Main.rand.NextFloat(1, 1.5f));
				Main.dust[dust].velocity = Vector2.UnitY * Main.rand.NextFloat(1, 3);
			}
		}
		else {
			lifeCounter = 0;
		}
	}
	private void Move(Player player) {
		var positionAbovePlayer = new Vector2(player.Center.X, player.Center.Y - 200);
		if (NPC.NPCMoveToPosition(positionAbovePlayer, 30f)) {
			CurrentAttack++;
		}
	}
	public float AttackTimer { get => NPC.ai[0]; set => NPC.ai[0] = value; }
	public float CurrentAttack { get => NPC.ai[1]; set => NPC.ai[1] = value; }
	public float AttackCounter { get => NPC.ai[2]; set => NPC.ai[2] = value; }
	public int UniversalAttackCoolDown = 0;
	public bool CanTeleport = true;
	private void ShootShortSword() {
		if (AttackCounter >= TerrariaArrayID.AllOreShortSword.Length) {
			CurrentAttack++;
			AttackCounter = 0;
			AttackTimer = 0;
			UniversalAttackCoolDown = 30;
			CanTeleport = true;
			return;
		}
		if (++AttackTimer < 10) {
			return;
		}
		AttackTimer = 0;
		var vec = -Vector2.UnitY.Vector2DistributeEvenly(8, 120, (int)NPC.ai[2]) * 15f;
		int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<ShortSwordAttackOne>(),
			BossDamagePercentage(.75f), 2, NPC.target);
		if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
			projectile.ItemIDtextureValue = TerrariaArrayID.AllOreShortSword[(int)NPC.ai[2]];
		AttackCounter++;
	}
	private void ShootShortSword2(Player player) {
		//Custom boss movement oooohhh
		ChoosenPosition = player.Center.Add(0, 100);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
			}
			return;
		}
		if (AttackCounter >= TerrariaArrayID.AllOreShortSword.Length - 1) {
			CurrentAttack++;
			AttackCounter = 0;
			UniversalAttackCoolDown = 90;
			CanTeleport = true;
			return;
		}
		if (++AttackTimer < 20) {
			return;
		}
		AttackTimer = 0;
		var vec = Vector2.UnitX * 20 * Main.rand.NextBool(2).ToDirectionInt();
		int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<ShortSwordAttackTwo>(), BossDamagePercentage(.75f), 2, NPC.target);
		if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
			projectile.ItemIDtextureValue = TerrariaArrayID.AllOreShortSword[(int)AttackCounter];
		Main.projectile[proj].ai[1] = -20;
		Main.projectile[proj].ai[0] = 2;
		Main.projectile[proj].rotation = Main.projectile[proj].velocity.ToRotation() + MathHelper.PiOver4;
		AttackCounter++;
	}
	private void ShootBroadSword(Player player) {
		//ahh movment, after the attack counter reach
		if (AttackCounter >= TerrariaArrayID.AllOreBroadSword.Length) {
			if (NPC.velocity.IsLimitReached(1f)) {
				NPC.velocity *= .8f;
			}
			else {
				UniversalAttackCoolDown = 35;
				AttackTimer = 0;
				AttackCounter = 0;
				CurrentAttack++;
				NPC.velocity = Vector2.Zero;
			}
			return;
		}//Before attack counter reach
		else {
			if (AttackTimer == 0 && AttackCounter == 0) {
				lastPlayerPosition = player.Center;
				var distance2 = player.Center.Add(0, 300) - NPC.Center;
				if (distance2.LengthSquared() > 100 * 100) {
					NPC.velocity = distance2.SafeNormalize(Vector2.Zero) * distance2.Length() / 4f;
					return;
				}
			}
			var distance = lastPlayerPosition - NPC.Center;
			NPC.velocity = distance.SafeNormalize(Vector2.Zero) * 20;
			NPC.velocity = NPC.velocity.RotatedBy(MathHelper.ToRadians(90));
		}
		if (++AttackTimer < 10) {
			return;
		}
		AttackTimer = 0;
		var vec = (NPC.Center - player.Center).SafeNormalize(Vector2.Zero) * 10;
		int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadAttackOne>(), BossDamagePercentage(.85f), 2, NPC.target);
		if (Main.projectile[proj].ModProjectile is SwordBroadAttackOne swordProj) {
			swordProj.SetNPCOwner(NPC.whoAmI);
			swordProj.ItemIDtextureValue = TerrariaArrayID.AllOreBroadSword[(int)AttackCounter];
		}
		AttackCounter++;
	}
	private void ShootBroadSword2(Player player) {
		ChoosenPosition = player.Center.Add(0, 100);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
			}
			return;
		}
		if (++AttackTimer < 30) {
			return;
		}
		if (Main.rand.NextBool()) {
			for (int i = 0; i < TerrariaArrayID.AllOreBroadSword.Length; i++) {
				var vec = -Vector2.UnitY.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllOreBroadSword.Length, 180, i) * 50f;
				vec.Y = 5;
				int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadAttackTwo>(), BossDamagePercentage(.85f), 2, NPC.target);
				Main.projectile[proj].ai[1] = 35;
				if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
					projectile.ItemIDtextureValue = TerrariaArrayID.AllOreBroadSword[i];
			}
		}
		else {
			for (int i = 0; i < TerrariaArrayID.AllOreBroadSword.Length; i++) {
				var vec = -Vector2.UnitY.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllOreBroadSword.Length, 90, i) * 20f;
				int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadAttackTwo>(), BossDamagePercentage(.85f), 2, NPC.target);
				Main.projectile[proj].ai[1] = 50;
				if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
					projectile.ItemIDtextureValue = TerrariaArrayID.AllOreBroadSword[i];
			}
		}
		UniversalAttackCoolDown = 60;
		CurrentAttack++;
		AttackCounter = 0;
		AttackTimer = 0;
		CanTeleport = true;
	}
	private void ShootWoodBow(Player player) {
		if (AttackCounter >= TerrariaArrayID.AllWoodBowPHM.Length) {
			CurrentAttack++;
			AttackCounter = 0;
			AttackTimer = 0;
			UniversalAttackCoolDown = 45;
			return;
		}
		if (++AttackTimer <= 12) {
			return;
		}
		AttackTimer = 0;
		int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.One.InverseVector2DistributeEvenly(TerrariaArrayID.AllWoodBowPHM.Length, 360, AttackCounter) * 3, ModContent.ProjectileType<WoodBowAttackOne>(), BossDamagePercentage(.65f), 2, NPC.target);
		if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile) {
			projectile.ItemIDtextureValue = TerrariaArrayID.AllWoodBowPHM[(int)AttackCounter];
			Main.projectile[proj].ai[0] = -10;
			Main.projectile[proj].ai[2] = 5;
			Main.projectile[proj].timeLeft = 120 + 12 * (int)(TerrariaArrayID.AllWoodBowPHM.Length - AttackCounter);

		}
		Main.projectile[proj].rotation = Main.projectile[proj].velocity.ToRotation();
		AttackCounter++;
	}
	private void ShootWoodBow2() {
		for (int i = 0; i < TerrariaArrayID.AllWoodBowPHM.Length; i++) {
			var vec = Vector2.UnitY.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllWoodBowPHM.Length, 120, i) * -30f;
			int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<WoodBowAttackTwo>(), BossDamagePercentage(.45f), 2, NPC.target);
			if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
				projectile.ItemIDtextureValue = TerrariaArrayID.AllWoodBowPHM[i];
		}
		CurrentAttack++;
		AttackCounter = 0;
		AttackTimer = 0;
		UniversalAttackCoolDown = 145;
	}
	private void ShootStaff(Player player) {
		ChoosenPosition = player.Center.Add(0, 350);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
				lastPlayerPosition = player.Center;
			}
			return;
		}
		BossCircleMovement(5, TerrariaArrayID.AllGemStaffPHM.Length, out float percent);
		if (++AttackTimer <= 5) {
			NPC.velocity = Vector2.Zero;
			return;
		}
		AttackTimer = 0;
		if (AttackCounter >= TerrariaArrayID.AllGemStaffPHM.Length) {
			CurrentAttack++;
			AttackCounter = 0;
			AttackTimer = 0;
			UniversalAttackCoolDown = 165;
			ChoosenPosition = Vector2.Zero;
			return;
		}
		var vec = Vector2.UnitY.RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(0, 360, percent)));
		int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<GemStaffAttackOne>(), BossDamagePercentage(.75f), 2, NPC.target);
		if (Main.projectile[proj].ModProjectile is BaseHostileGemStaff gemstaffProj) {
			gemstaffProj.ItemIDtextureValue = TerrariaArrayID.AllGemStaffPHM[(int)NPC.ai[2]];
			gemstaffProj.ProjectileType = TerrariaArrayID.AllGemStafProjectilePHM[(int)NPC.ai[2]];
		}
		AttackCounter++;
	}
	private void ShootStaff2(Player player) {
		ChoosenPosition = player.Center.Add(0, 150);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
				lastPlayerPosition = player.Center;
			}
			return;
		}
		if (++AttackTimer <= 40) {
			return;
		}
		for (int i = 0; i < TerrariaArrayID.AllGemStaffPHM.Length; i++) {
			int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Main.rand.NextVector2Circular(12, 12), ModContent.ProjectileType<GemStaffAttackTwo>(), BossDamagePercentage(.75f), 2, NPC.target);
			Main.projectile[proj].ai[2] = i;
			Main.projectile[proj].rotation = MathHelper.PiOver4 + MathHelper.PiOver2;
			if (Main.projectile[proj].ModProjectile is BaseHostileGemStaff gemstaffProj) {
				gemstaffProj.SetNPCOwner(NPC.whoAmI);
				gemstaffProj.ItemIDtextureValue = TerrariaArrayID.AllGemStaffPHM[i];
				gemstaffProj.ProjectileType = TerrariaArrayID.AllGemStafProjectilePHM[i];
			}
		}
		Reset(CurrentAttack + 1, 180);
	}
	private void ShootOreBow1(Player player) {
		ChoosenPosition = player.Center.Add(0, 150);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
				lastPlayerPosition = player.Center;
			}
			return;
		}
		if (++AttackTimer <= 20) {
			NPC.velocity = Vector2.Zero;
			return;
		}
		AttackTimer = 0;
		if (AttackCounter >= TerrariaArrayID.AllOreBowPHM.Length) {
			Reset(CurrentAttack + 1, 145);
		}
		var vec = Vector2.UnitY.Vector2DistributeEvenly(8, 360, (int)NPC.ai[2]) * 10f;
		int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<OreBowAttackOne>(), BossDamagePercentage(.55f), 2, NPC.target);
		if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
			projectile.ItemIDtextureValue = TerrariaArrayID.AllOreBowPHM[(int)NPC.ai[2]];
		Main.projectile[proj].ai[1] = 60;
		NPC.ai[2]++;
	}
	private void ShootOreBow2(Player player) {
		ChoosenPosition = player.Center.Add(0, 350);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
				lastPlayerPosition = player.Center;
			}
			return;
		}
		if (AttackCounter >= TerrariaArrayID.AllOreBowPHM.Length - 1) {
			if (NPC.velocity.IsLimitReached(1f)) {
				NPC.velocity *= .8f;
			}
			else {
				NPC.velocity = Vector2.Zero;
				AttackCounter = 0;
				AttackTimer = 0;
				UniversalAttackCoolDown = 145;
				CurrentAttack++;
			}
			return;
		}
		var positionAbovePlayer = Main.player[NPC.target].Center + new Vector2(0, -350);
		NPC.NPCMoveToPosition(positionAbovePlayer, 5f);
		if (++AttackTimer < 10) {
			return;
		}
		AttackTimer = 0;
		int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<OreBowAttackTwo>(), BossDamagePercentage(.55f), 2, NPC.target);
		if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
			projectile.ItemIDtextureValue = TerrariaArrayID.AllOreBowPHM[(int)NPC.ai[2]];
		AttackCounter++;
	}
	private void ShootGun(Player player) {
		ChoosenPosition = player.Center.Add(0, 100);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
			}
			return;
		}
		if (++AttackTimer >= 360) {
			Reset(CurrentAttack + 1, 30);
			return;
		}
		if (AttackCounter >= 1) {
			return;
		}
		int direction;
		if (player.Center.X > NPC.Center.X) {
			direction = 1;
		}
		else {
			direction = -1;
		}
		int minishark = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HostileMinishark>(), BossDamagePercentage(.25f), 2, NPC.target);
		if (Main.projectile[minishark].ModProjectile is BaseHostileGun minisharkproj) {
			minisharkproj.ItemIDtextureValue = ItemID.Minishark;
			minisharkproj.SetNPCOwner(NPC.whoAmI);
		}
		int Musket = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HostileMusket>(), NPC.damage, 2, NPC.target);
		if (Main.projectile[Musket].ModProjectile is BaseHostileGun musketproj) {
			musketproj.ItemIDtextureValue = ItemID.Musket;
			Main.projectile[Musket].ai[2] = direction;
			musketproj.SetNPCOwner(NPC.whoAmI);
		}
		NPC.ai[2]++;
	}
	private void ShootGun2(Player player) {
		ChoosenPosition = player.Center.Add(0, 350);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
				lastPlayerPosition = player.Center;
			}
			return;
		}
		if (++AttackTimer < 12) {
			return;
		}
		AttackTimer = 0;
		if (++AttackCounter >= 12) {
			Reset(0, 120);
		}
		int boomStick = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 20, ModContent.ProjectileType<HostileBoomStick>(), NPC.damage, 2, NPC.target);
		if (Main.projectile[boomStick].ModProjectile is BaseHostileGun hostileGun) {
			hostileGun.ItemIDtextureValue = ItemID.Boomstick;
			hostileGun.SetNPCOwner(NPC.whoAmI);
		}
	}
	public void Reset(float nextAttack, int cooldown) {
		CurrentAttack = nextAttack;
		AttackCounter = 0;
		AttackTimer = 0;
		UniversalAttackCoolDown = cooldown;
		CanTeleport = true;
	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
		Main.instance.LoadNPC(NPC.type);
		Texture2D texture = TextureAssets.Npc[Type].Value;
		if (IsTeleporting) {
			float progress = TeleportTime / (float)TeleportDuration;
			if (progress <= .5f) {
				float progress1sthalf = 1 - TeleportTime / (TeleportDuration * .5f);
				spriteBatch.Draw(texture, NPC.position - screenPos, drawColor.ScaleRGB(progress1sthalf) with { A = (byte)(255 * progress1sthalf) });
			}
			else {
				float progresshalf = 1 - (TeleportTime - 30) * 2 / (float)TeleportDuration;
				drawColor = drawColor.ScaleRGB(1 - progresshalf) with { A = (byte)(255 * (1 - progresshalf)) };
				for (int i = 0; i < 4; i++) {
					spriteBatch.Draw(texture, TeleportPosition + Vector2.One.RotatedBy(90 * (i + 1) * (progresshalf)) * 100 * progresshalf - screenPos, drawColor);

				}
			}
		}
		else {
			spriteBatch.Draw(texture, NPC.position - screenPos, drawColor);
		}
		return false;
	}
	Vector2 lastPlayerPosition = Vector2.Zero;
	private void BossCircleMovement(int delayValue, int AttackEndValue, out float percent) {
		float total = delayValue * AttackEndValue;
		percent = Math.Clamp(((delayValue - NPC.ai[0] >= 0 ? delayValue - NPC.ai[0] : 0) + delayValue * NPC.ai[2]) / total, 0, 1f);
		float rotation = MathHelper.Lerp(0, 360, percent);
		var rotateAroundPlayerCenter = lastPlayerPosition - Vector2.UnitY.RotatedBy(MathHelper.ToRadians(rotation)) * 350;
		NPC.Center = rotateAroundPlayerCenter;
	}
}
