using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Roguelike.Common.General;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items;
using Roguelike.Contents.Items.Lootbox;
using Roguelike.Contents.NPCs.LootBoxLord.Buff;
using Roguelike.Contents.NPCs.LootBoxLord.HostileProjectile;
using Roguelike.Texture;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Roguelike.Contents.NPCs.LootBoxLord;
[AutoloadBossHead]
internal class LootBoxLord : ModNPC {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<WoodenLootBox>();
	public override string BossHeadTexture => Texture;
	public override void SetStaticDefaults() {
		NPCID.Sets.DontDoHardmodeScaling[Type] = true;
		NPCID.Sets.NeedsExpertScaling[Type] = false;
	}
	public override void SetDefaults() {
		NPC.lifeMax = 35000;
		NPC.damage = 100;
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
		NPC.strengthMultiplier = 0;
		NPC.dontTakeDamageFromHostiles = true;
		NPC.GetGlobalNPC<RoguelikeGlobalNPC>().NPC_SpecialException = true;
		HasCreatedDeathTimer = false;
		NPC.BossBar = ModContent.GetInstance<LootBoxLordBossBossBar>();
		if (!Main.dedServ) {
			Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/LootboxLord_BossMusic");
		}
	}
	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
		bestiaryEntry.Info.AddRange([
			new FlavorTextBestiaryInfoElement(ModUtils.LocalizationText("Bestiary",$"{Name}")),
			]);
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
		NPC.lifeMax = 35000;
		NPC.life = NPC.lifeMax;
		NPC.damage = 100;
		NPC.defense = 50;
	}
	public override bool? CanBeHitByProjectile(Projectile projectile) {
		if (projectile.hostile) {
			return false;
		}
		return !IsTeleporting && IframeCounter <= 0;
	}
	public override bool? CanBeHitByItem(Player player, Item item) {
		return !IsTeleporting && IframeCounter <= 0;
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
		if (ModContent.GetInstance<RogueLikeConfig>().SkipCutscene) {
			if (dialogNumber < 3) {
				dialogNumber = 3;
			}
		}
		switch (dialogNumber) {
			case 0:
				dialog = "I recognize you";
				break;
			case 1:
				dialog = "... I see, if that is what you wish for";
				break;
			case 2:
				dialog = "I will entertain you";
				break;
			case 3:
				dialog = "Don't die too soon my grace.";
				color = Color.Red;
				BeforeAttack = false;
				NPC.dontTakeDamage = false;
				UniversalAttackCoolDown = 60;
				int[] itemArr = [.. TerrariaArrayID.MeleePreBoss, .. TerrariaArrayID.MeleePreEoC, .. TerrariaArrayID.MeleeEvilBoss, .. TerrariaArrayID.RangePreBoss, .. TerrariaArrayID.RangePreEoC, .. TerrariaArrayID.RangeSkele, .. TerrariaArrayID.MagicPreBoss, .. TerrariaArrayID.MagicPreEoC, .. TerrariaArrayID.MagicSkele, .. TerrariaArrayID.SummonPreBoss, .. TerrariaArrayID.SummonerPreEoC, .. TerrariaArrayID.SummonSkele,];
				for (int i = 0; i < 200; i++) {
					Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<ProjectileRing>(), NPC.damage, 0, NPC.target);
					proj.ai[0] = 550;
					if (proj.ModProjectile is BaseHostileProjectile hostile) {
						hostile.IDtextureValue = Main.rand.Next(itemArr);
						hostile.SetNPCOwner(NPC.whoAmI);
					}
					proj.ai[1] += MathHelper.ToRadians(MathHelper.Lerp(0, 360, i / 199f));
					proj.rotation += MathHelper.ToRadians(Main.rand.NextFloat(360));
				}
				break;
		}
		dialogCD = 120;
		dialogNumber++;
		ModUtils.CombatTextRevamp(NPC.Hitbox, color, dialog);
	}
	bool CanSlowDown = false;
	bool HasCreatedDeathTimer = false;
	bool CanScream = true;
	public override void ResetEffects() {
	}
	public override void AI() {
		if (Reached110HP) {
			NPC.GetGlobalNPC<RoguelikeGlobalNPC>().Endurance += .4f;
			SpawnUltiOnce = true;
			if (CanScream) {
				CanScream = false;
				SoundEngine.PlaySound(SoundID.NPCDeath10 with { Pitch = -1 }, NPC.Center);
			}
		}
		var player = Main.player[NPC.target];
		CD_DefenseUp = ModUtils.CountDown(CD_DefenseUp);
		IframeCounter = ModUtils.CountDown(IframeCounter);
		if (NPC.GetLifePercent() <= .5) {
			if (!Reached12HP) {
				for (int i = 0; i < 100; i++) {
					Dust dust = Dust.NewDustDirect(NPC.Center, 0, 0, DustID.GemEmerald);
					dust.noGravity = true;
					dust.velocity = Main.rand.NextVector2CircularEdge(10, 10) * Main.rand.NextFloat(.7f, 1.35f);
				}
				for (int i = 0; i < 25; i++) {
					int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Main.rand.NextVector2CircularEdge(10, 10) * Main.rand.NextFloat(.7f, 1.35f), ModContent.ProjectileType<LBL_HealingProjectile>(), 0, 2, NPC.target);
					if (Main.projectile[proj].ModProjectile is LBL_HealingProjectile projectile2)
						projectile2.SetNPCOwner(NPC.whoAmI);
					Main.projectile[proj].ai[2] = NPC.lifeMax * .01f + 1;
				}
				UniversalAttackCoolDown = 120;
				CanSlowDown = true;
			}
			Reached12HP = true;
		}
		if (NPC.GetLifePercent() <= .25) {
			if (!Reached14HP) {
				for (int i = 0; i < 100; i++) {
					Dust dust = Dust.NewDustDirect(NPC.Center, 0, 0, DustID.GemEmerald);
					dust.noGravity = true;
					dust.velocity = Main.rand.NextVector2CircularEdge(10, 10) * Main.rand.NextFloat(.7f, 1.35f);
				}
				for (int i = 0; i < 25; i++) {
					int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Main.rand.NextVector2CircularEdge(10, 10) * Main.rand.NextFloat(.7f, 1.35f), ModContent.ProjectileType<LBL_HealingProjectile>(), 0, 2, NPC.target);
					if (Main.projectile[proj].ModProjectile is LBL_HealingProjectile projectile2)
						projectile2.SetNPCOwner(NPC.whoAmI);
					Main.projectile[proj].ai[2] = NPC.lifeMax * .01f + 1;
				}
				UniversalAttackCoolDown = 120;
				CanSlowDown = true;
			}
			Reached14HP = true;
		}
		if (NPC.GetLifePercent() <= .2) {
			if (!Reached15HP) {
				ModUtils.CombatTextRevamp(NPC.Hitbox, Color.LightBlue, "Endurance +30% for 30s");
				NPC.AddBuff<LootBoxLord_EnduranceUp>(ModUtils.ToSecond(30));
				UniversalAttackCoolDown = 120;
				CanSlowDown = true;
			}
			Reached15HP = true;
		}
		if (NPC.GetLifePercent() <= .1f) {
			if (!Reached110HP) {
				Reset(0, 240);
				CanSlowDown = true;
				Main.NewText("Final domain: Disable life regeneration and healing", Color.DarkRed);
				player.AddBuff<LootboxLord_FinalDomain>(3600);
			}
			Reached110HP = true;
		}
		if (BeforeAttack) {
			Dialog();
			return;
		}
		Lighting.AddLight(NPC.Center, 1, 1, 1);
		if (player.dead || !player.active) {
			NPC.FindClosestPlayer();
			NPC.TargetClosest();
			player = Main.player[NPC.target];
			if (player.dead || !player.active) {
				NPC.active = false;
			}
		}
		if (!HasCreatedDeathTimer && !player.HasBuff<LootboxLord_DeathField>()) {
			HasCreatedDeathTimer = true;
			player.AddBuff<LootboxLord_DeathField>(ModUtils.ToMinute(3) + 120);
			Main.NewText("Lootbox Lord domain : Delayed death - player have 3 minute to kill Lootbox Lord", Color.Red);
		}
		if (NPC.CountNPCS(Type) > 1) {
			if (!AlreadySaidThat) {
				ModUtils.CombatTextRevamp(NPC.Hitbox, Color.Red, "You think so lowly of me ...");
				AlreadySaidThat = true;
				if (!NPC.AnyNPCs(ModContent.NPCType<ElderGuardian>()))
					NPC.NewNPC(NPC.GetSource_FromAI(), NPC.Hitbox.X, NPC.Hitbox.Y, ModContent.NPCType<ElderGuardian>());
			}
		}
		if (CanSlowDown) {
			NPC.velocity *= .98f;
		}
		if (UniversalAttackCoolDown > 0) {
			UniversalAttackCoolDown--;
			return;
		}
		if (Reached110HP) {
			DeseperationAttack(player);
			return;
		}
		CanSlowDown = false;
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
			case 13:
				SpearAttack(player);
				break;
			case 14:
				BulletHell(player);
				break;
			case 15:
				BookAttack(player);
				break;
			default:
				CurrentAttack = 0;
				break;
		}
	}
	private void DeseperationAttack(Player player) {
		if (AttackCounter <= 0) {
			AttackCounter++;
			int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, -Vector2.UnitY * 3, ModContent.ProjectileType<HostileMinisharkDesperation>(),
			BossDamagePercentage(.35f), 2, NPC.target);
			if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
				projectile.SetNPCOwner(NPC.whoAmI);
		}
		AttackTimer++;
		if (AttackTimer % 12 == 0) {
			for (int i = 0; i < 2; i++) {
				int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.One.RotatedBy(MathHelper.ToRadians(180 * i + AttackTimer * 2)) * 3, ModContent.ProjectileType<SwordBroadDesperation>(), BossDamagePercentage(.6f), 2, NPC.target);
				if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile) {
					projectile.IDtextureValue = Main.rand.Next(TerrariaArrayID.AllOreBroadSword);
					projectile.Projectile.timeLeft = 180;
				}
			}
		}
		if (AttackTimer % 96 == 0) {
			for (int i = 0; i < 16; i++) {
				int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, (player.Center - NPC.Center).SafeNormalize(Vector2.Zero).Vector2DistributeEvenlyPlus(16, 360, i) * 10, ModContent.ProjectileType<ShortSwordDesperation>(), BossDamagePercentage(.75f), 2, NPC.target);
				if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile) {
					projectile.IDtextureValue = Main.rand.Next(TerrariaArrayID.AllOreShortSword);
					projectile.Projectile.timeLeft = 90;
				}
			}
		}

	}
	int CD_DefenseUp = 0;
	bool Reached12HP = false;
	bool Reached14HP = false;
	bool Reached15HP = false;
	bool Reached110HP = false;
	int IframeCounter = 0;
	public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10)) {
			IframeCounter = 30;
		}
	}
	public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(10)) {
			IframeCounter = 30;
		}
	}
	public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers) {
		Ability_DefenseUP(ref modifiers, player.GetWeaponDamage(item));
	}
	public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers) {
		Ability_DefenseUP(ref modifiers, projectile.damage);
	}
	private void Ability_DefenseUP(ref NPC.HitModifiers modifiers, int damage) {
		if (CD_DefenseUp == 0 && modifiers.GetDamage(damage, false) >= 100) {
			modifiers.SetMaxDamage(1);
			NPC.AddBuff<LootBoxLord_DefenseUp>(ModUtils.ToSecond(10));
			CD_DefenseUp = ModUtils.ToSecond(30);
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
	public bool AttackIndicator = true;
	private void ShootShortSword() {
		if (AttackCounter >= TerrariaArrayID.AllOreShortSword.Length) {
			Reset(CurrentAttack + 1, 30);
			return;
		}
		if (AttackTimer == 0 && AttackIndicator) {
			AttackIndicator = false;
			ModUtils.DustStar(NPC.Center, DustID.GemRuby, Color.White);
			UniversalAttackCoolDown = 20;
		}
		if (++AttackTimer < 10) {
			return;
		}
		AttackTimer = 0;
		var vec = -Vector2.UnitY.Vector2DistributeEvenly(8, 120, (int)NPC.ai[2]) * 12f;
		int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<ShortSwordAttackOne>(),
			BossDamagePercentage(.75f), 2, NPC.target);
		if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
			projectile.IDtextureValue = TerrariaArrayID.AllOreShortSword[(int)NPC.ai[2]];
		if (Main.expertMode || Main.masterMode) {
			proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, -vec, ModContent.ProjectileType<ShortSwordAttackOne>(),
				BossDamagePercentage(.75f), 2, NPC.target);
			if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile2)
				projectile2.IDtextureValue = TerrariaArrayID.AllOreShortSword[(int)NPC.ai[2]];
		}
		AttackCounter++;
	}
	private void ShootShortSword2(Player player) {
		//Custom boss movement oooohhh
		ChoosenPosition = player.Center.Add(0, 300);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
			}
			return;
		}
		CanSlowDown = true;
		if (AttackTimer == 0 && AttackIndicator) {
			AttackIndicator = false;
			ModUtils.DustStar(NPC.Center, DustID.GemRuby, Color.White);
			UniversalAttackCoolDown = 20;
			return;
		}
		Vector2 distance = player.Center - NPC.Center;
		NPC.velocity = distance.SafeNormalize(Vector2.Zero) * distance.Length() / 64f;
		if (AttackCounter >= TerrariaArrayID.AllOreShortSword.Length - 1) {
			Reset(CurrentAttack + 1, 90);
			return;
		}
		if (++AttackTimer < 20) {
			return;
		}
		AttackTimer = 0;
		var vec = Vector2.UnitX * 20 * Main.rand.NextBool(2).ToDirectionInt();
		int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<ShortSwordAttackTwo>(), BossDamagePercentage(.75f), 2, NPC.target);
		if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
			projectile.IDtextureValue = TerrariaArrayID.AllOreShortSword[(int)AttackCounter];
		Main.projectile[proj].ai[1] = -20;
		Main.projectile[proj].ai[0] = 2;
		Main.projectile[proj].rotation = Main.projectile[proj].velocity.ToRotation() + MathHelper.PiOver4;
		if (Main.expertMode || Main.masterMode) {
			int proj2 = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, -Vector2.UnitY * 20, ModContent.ProjectileType<ShortSwordAttackTwo>(), BossDamagePercentage(.75f), 2, NPC.target);
			if (Main.projectile[proj2].ModProjectile is BaseHostileProjectile projectile2)
				projectile2.IDtextureValue = TerrariaArrayID.AllOreShortSword[(int)AttackCounter];
			Main.projectile[proj2].ai[1] = -20;
			Main.projectile[proj2].ai[0] = 2;
			Main.projectile[proj2].rotation = Main.projectile[proj2].velocity.ToRotation() + MathHelper.PiOver4;
		}
		AttackCounter++;
	}
	private void ShootBroadSword(Player player) {
		//ahh movment, after the attack counter reach
		if (AttackCounter >= TerrariaArrayID.AllOreBroadSword.Length) {
			if (NPC.velocity.IsLimitReached(1f)) {
				NPC.velocity *= .8f;
			}
			else {
				Reset(CurrentAttack + 1, 35);
				NPC.velocity = Vector2.Zero;
			}
			return;
		}//Before attack counter reach
		else {
			if (AttackTimer == 0 && AttackCounter == 0) {
				lastPlayerPosition = player.Center;
				var distance2 = player.Center.Add(0, 300) - NPC.Center;
				if (distance2.LengthSquared() > 100 * 100) {
					NPC.velocity = distance2.SafeNormalize(Vector2.Zero) * distance2.Length() / 8f;
					return;
				}
			}
			var distance = lastPlayerPosition - NPC.Center;
			NPC.velocity = distance.SafeNormalize(Vector2.Zero) * 20;
			NPC.velocity = NPC.velocity.RotatedBy(MathHelper.ToRadians(90));
		}
		if (AttackTimer == 0 && AttackIndicator) {
			AttackIndicator = false;
			ModUtils.DustStar(NPC.Center, DustID.GemRuby, Color.White);
			UniversalAttackCoolDown = 20;
		}
		if (++AttackTimer < 10) {
			return;
		}
		AttackTimer = 0;
		var vec = (NPC.Center - player.Center).SafeNormalize(Vector2.Zero) * 10;
		int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadAttackOne>(), BossDamagePercentage(.85f), 2, NPC.target);
		if (Main.projectile[proj].ModProjectile is SwordBroadAttackOne swordProj) {
			swordProj.SetNPCOwner(NPC.whoAmI);
			swordProj.IDtextureValue = TerrariaArrayID.AllOreBroadSword[(int)AttackCounter];
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
		if (AttackTimer == 10 && AttackIndicator) {
			AttackIndicator = false;
			ModUtils.DustStar(NPC.Center, DustID.GemRuby, Color.White);
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
					projectile.IDtextureValue = TerrariaArrayID.AllOreBroadSword[i];
			}
		}
		else {
			for (int i = 0; i < TerrariaArrayID.AllOreBroadSword.Length; i++) {
				var vec = -Vector2.UnitY.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllOreBroadSword.Length, 90, i) * 20f;
				int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadAttackTwo>(), BossDamagePercentage(.85f), 2, NPC.target);
				Main.projectile[proj].ai[1] = 35;
				if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
					projectile.IDtextureValue = TerrariaArrayID.AllOreBroadSword[i];
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
			Reset(CurrentAttack + 1, 45);
			return;
		}
		if (AttackTimer == 0 && AttackIndicator) {
			AttackIndicator = false;
			ModUtils.DustStar(NPC.Center, DustID.GemRuby, Color.White);
		}
		if (++AttackTimer <= 12) {
			return;
		}
		AttackTimer = 0;
		int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.One.InverseVector2DistributeEvenly(TerrariaArrayID.AllWoodBowPHM.Length, 360, AttackCounter) * 3, ModContent.ProjectileType<WoodBowAttackOne>(), BossDamagePercentage(.65f), 2, NPC.target);
		if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile) {
			projectile.IDtextureValue = TerrariaArrayID.AllWoodBowPHM[(int)AttackCounter];
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
				projectile.IDtextureValue = TerrariaArrayID.AllWoodBowPHM[i];
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
		if (AttackTimer == 0 && AttackIndicator) {
			AttackIndicator = false;
			ModUtils.DustStar(NPC.Center, DustID.GemRuby, Color.White);
		}
		BossCircleMovement(5, TerrariaArrayID.AllGemStaffPHM.Length, out float percent);
		if (++AttackTimer <= 5) {
			NPC.velocity = Vector2.Zero;
			return;
		}
		AttackTimer = 0;
		if (AttackCounter >= TerrariaArrayID.AllGemStaffPHM.Length) {
			Reset(CurrentAttack + 1, 165);
			return;
		}
		var vec = Vector2.UnitY.RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(0, 360, percent)));
		int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<GemStaffAttackOne>(), BossDamagePercentage(.75f), 2, NPC.target);
		if (Main.projectile[proj].ModProjectile is BaseHostileGemStaff gemstaffProj) {
			gemstaffProj.IDtextureValue = TerrariaArrayID.AllGemStaffPHM[(int)NPC.ai[2]];
			gemstaffProj.ProjectileType = TerrariaArrayID.AllGemStafProjectilePHM[(int)NPC.ai[2]];
		}
		AttackCounter++;
	}
	private void ShootStaff2(Player player) {
		ChoosenPosition = player.Center.Add(150, 0);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
				lastPlayerPosition = player.Center;
			}
			return;
		}
		CanSlowDown = true;
		if (AttackIndicator) {
			AttackIndicator = false;
			ModUtils.DustStar(NPC.Center, DustID.GemRuby, Color.White);
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
				gemstaffProj.IDtextureValue = TerrariaArrayID.AllGemStaffPHM[i];
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
		if (AttackIndicator) {
			AttackIndicator = false;
			ModUtils.DustStar(NPC.Center, DustID.GemRuby, Color.White);
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
			projectile.IDtextureValue = TerrariaArrayID.AllOreBowPHM[(int)NPC.ai[2]];
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
				Reset(CurrentAttack + 1, 145);
				NPC.velocity = Vector2.Zero;
			}
			return;
		}
		var positionAbovePlayer = Main.player[NPC.target].Center + new Vector2(0, -350);
		NPC.NPCMoveToPosition(positionAbovePlayer, 5f);
		if (AttackTimer == 0 && AttackIndicator) {
			AttackIndicator = false;
			ModUtils.DustStar(NPC.Center, DustID.GemRuby, Color.White);
		}
		if (++AttackTimer < 10) {
			return;
		}
		AttackTimer = 0;
		int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<OreBowAttackTwo>(), BossDamagePercentage(.55f), 2, NPC.target);
		if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
			projectile.IDtextureValue = TerrariaArrayID.AllOreBowPHM[(int)NPC.ai[2]];
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
		if (++AttackTimer >= 330) {
			Reset(CurrentAttack + 1, 30);
			return;
		}
		if (AttackTimer == 1 && AttackIndicator) {
			AttackIndicator = false;
			ModUtils.DustStar(NPC.Center, DustID.GemRuby, Color.White);
		}
		if (AttackTimer >= 30 || AttackCounter >= 1) {
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
			minisharkproj.IDtextureValue = ItemID.Minishark;
			minisharkproj.SetNPCOwner(NPC.whoAmI);
		}
		int Musket = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HostileMusket>(), NPC.damage, 2, NPC.target);
		if (Main.projectile[Musket].ModProjectile is BaseHostileGun musketproj) {
			musketproj.IDtextureValue = ItemID.Musket;
			Main.projectile[Musket].ai[2] = direction;
			musketproj.SetNPCOwner(NPC.whoAmI);
		}
		AttackCounter++;
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
		if (AttackTimer == 0 && AttackIndicator) {
			AttackIndicator = false;
			ModUtils.DustStar(NPC.Center, DustID.GemRuby, Color.White);
			UniversalAttackCoolDown = 12;
		}
		if (++AttackTimer < 12) {
			return;
		}
		AttackTimer = 0;
		if (++AttackCounter >= 6) {
			Reset(CurrentAttack + 1, 60);
			return;
		}
		int boomStick = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 20, ModContent.ProjectileType<HostileBoomStick>(), BossDamagePercentage(.25f), 2, NPC.target);
		if (Main.projectile[boomStick].ModProjectile is BaseHostileGun hostileGun) {
			hostileGun.IDtextureValue = ItemID.Boomstick;
			hostileGun.SetNPCOwner(NPC.whoAmI);
		}
	}
	private void SpearAttack(Player player) {
		ChoosenPosition = player.Center.Add(0, 350);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
				lastPlayerPosition = player.Center;
			}
			return;
		}
		if (AttackTimer == 0 && AttackIndicator) {
			AttackIndicator = false;
			ModUtils.DustStar(NPC.Center, DustID.GemRuby, Color.White);
		}
		if (AttackCounter >= 3) {
			Reset(CurrentAttack + 1, 90);
			return;
		}
		if (++AttackTimer < 32) {
			return;
		}
		AttackTimer = 0;
		for (int i = 0; i < 16; i++) {
			int Spear = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.One.Vector2DistributeEvenlyPlus(16, 360, i) * 6, ModContent.ProjectileType<SpearAttackOne>(), NPC.damage, 2, NPC.target);
			if (Main.projectile[Spear].ModProjectile is SpearAttackOne hostileGun) {
				hostileGun.IDtextureValue = ItemID.Spear;
				hostileGun.SetNPCOwner(NPC.whoAmI);
				hostileGun.TowardTo = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 15;
			}
		}
		AttackCounter++;
	}
	private void BookAttack(Player player) {
		ChoosenPosition = player.Center.Add(0, 150);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
				lastPlayerPosition = player.Center;
			}
			return;
		}
		if (AttackTimer == 0 && AttackIndicator) {
			AttackIndicator = false;
			ModUtils.DustStar(NPC.Center, DustID.GemRuby, Color.White);
		}
		if (AttackCounter <= 0) {
			int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HostileWaterbolt>(), NPC.damage, 1, NPC.target);
			Projectile projectile = Main.projectile[proj];
			if (projectile.ModProjectile is HostileWaterbolt book1) {
				book1.SetNPCOwner(NPC.whoAmI);
			}
			proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HostileBookOfSkull>(), NPC.damage, 1, NPC.target);
			projectile = Main.projectile[proj];
			if (projectile.ModProjectile is HostileBookOfSkull book2) {
				book2.SetNPCOwner(NPC.whoAmI);
			}
			proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HostileDemonScythe>(), NPC.damage, 1, NPC.target);
			projectile = Main.projectile[proj];
			if (projectile.ModProjectile is HostileDemonScythe book3) {
				book3.SetNPCOwner(NPC.whoAmI);
			}
			AttackCounter++;
		}
		Reset(CurrentAttack + 1, 270);
	}
	private void BulletHell(Player player) {
		ChoosenPosition = player.Center.Add(0, 250);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
				lastPlayerPosition = player.Center;
			}
			return;
		}
		if (AttackTimer == 0 && AttackIndicator) {
			AttackIndicator = false;
			ModUtils.DustStar(NPC.Center, DustID.GemRuby, Color.White);
		}
		if (AttackCounter <= 0) {
			for (int i = 0; i < 4; i++) {
				int proj = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HostilePistolAttackOne>(), BossDamagePercentage(.45f), 1, NPC.target);
				Projectile projectile = Main.projectile[proj];
				if (projectile.ModProjectile is HostilePistolAttackOne pistol) {
					switch (i) {
						case 0:
							pistol.IDtextureValue = ItemID.FlintlockPistol;
							break;
						case 1:
							pistol.IDtextureValue = ItemID.Revolver;
							break;
						case 2:
							pistol.IDtextureValue = ItemID.Handgun;
							break;
						case 3:
							pistol.IDtextureValue = ItemID.TheUndertaker;
							break;
					}
					pistol.SetNPCOwner(NPC.whoAmI);
					projectile.ai[0] = 90 * i;
				}
			}
			int proj2 = ModUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.UnitY * -3, ModContent.ProjectileType<HostilePistolAttackTwo>(), NPC.damage, 1, NPC.target);
			Projectile projectile2 = Main.projectile[proj2];
			if (projectile2.ModProjectile is HostilePistolAttackTwo pistol2) {
				pistol2.IDtextureValue = ItemID.PhoenixBlaster;
				pistol2.SetNPCOwner(NPC.whoAmI);
			}
			AttackCounter++;
		}
		Reset(CurrentAttack + 1, 270);
	}
	public void Reset(float nextAttack, int cooldown) {
		CurrentAttack = nextAttack;
		AttackCounter = 0;
		AttackTimer = 0;
		UniversalAttackCoolDown = cooldown;
		CanTeleport = true;
		AttackIndicator = true;
	}
	bool SpawnUltiOnce = false;
	int AuraCounter = 0;
	int DefenseUp_EnterFrame = 90;
	int DefenseUp_PulseFrame = 0;
	bool Switch = false;
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
		Main.instance.LoadNPC(NPC.type);
		Texture2D texture = TextureAssets.Npc[Type].Value;
		if (IframeCounter > 0) {
			if (IframeCounter % 6 >= 3) {
				return false;
			}
		}
		Vector2 whereNPCKnowItNot = NPC.position;
		if (SpawnUltiOnce) {
			Texture2D ShadingCircle = ModContent.Request<Texture2D>(ModTexture.OuterInnerGlow).Value;
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
			Vector2 origin = texture.Size() * .5f;
			float percentalge = Math.Clamp(1 - AuraCounter / 180f, 0, 1f);
			spriteBatch.Draw(ShadingCircle, NPC.position - Main.screenPosition + origin, null, Color.Red * percentalge, 0, ShadingCircle.Size() * .5f, 15 * ModUtils.OutExpo(AuraCounter / 180f), SpriteEffects.None, 0);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			if (++AuraCounter >= 180) {
				SpawnUltiOnce = false;
			}
		}
		if (IsTeleporting) {
			float progress = TeleportTime / (float)TeleportDuration;
			if (progress <= .5f) {
				float progress1sthalf = 1 - TeleportTime / (TeleportDuration * .5f);
				spriteBatch.Draw(texture, NPC.position - screenPos, drawColor.ScaleRGB(progress1sthalf) with { A = (byte)(255 * progress1sthalf) });
			}
			else {
				whereNPCKnowItNot = TeleportPosition;
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
		//For drawing glow
		float multi = 1f;
		float scale = 1f;
		if (NPC.HasBuff<LootBoxLord_DefenseUp>()) {
			if (--DefenseUp_EnterFrame > 0) {
				multi *= 1 - DefenseUp_EnterFrame / 90f;
				scale += DefenseUp_EnterFrame / 90f;
			}
			else {
				if (DefenseUp_PulseFrame < 100 && !Switch) {
					DefenseUp_PulseFrame++;
				}
				if (DefenseUp_PulseFrame > 0 && Switch) {
					DefenseUp_PulseFrame--;
				}
				if (DefenseUp_PulseFrame >= 100) {
					Switch = true;
				}
				if (DefenseUp_PulseFrame <= 0) {
					Switch = false;
				}
				multi -= DefenseUp_PulseFrame / 100f * .75f;
				scale += DefenseUp_PulseFrame / 300f;
			}
		}
		else {
			DefenseUp_EnterFrame = 90;
			if (DefenseUp_PulseFrame >= 0) {
				DefenseUp_PulseFrame--;
			}
			multi *= DefenseUp_PulseFrame / 100f;
			scale += DefenseUp_PulseFrame / 300f;
		}
		if (multi > 0) {
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
			Texture2D ShadingCircle = ModContent.Request<Texture2D>(ModTexture.OuterInnerGlow).Value;
			Vector2 origin = texture.Size() * .5f;
			Color color = Color.Yellow;
			color.A = (byte)(255 * multi);
			spriteBatch.Draw(ShadingCircle, whereNPCKnowItNot - Main.screenPosition + origin, null, color, 0, ShadingCircle.Size() * .5f, scale, SpriteEffects.None, 0);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
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
public class LootBoxLordBossBossBar : ModBossBar {
	private int bossHeadIndex = -1;

	public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame) {
		// Display the previously assigned head index
		if (bossHeadIndex != -1) {
			return TextureAssets.NpcHeadBoss[bossHeadIndex];
		}
		return null;
	}
	public override bool PreDraw(SpriteBatch spriteBatch, NPC npc, ref BossBarDrawParams drawParams) {
		drawParams.IconScale = .56f;
		return base.PreDraw(spriteBatch, npc, ref drawParams);
	}
	public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float life, ref float lifeMax, ref float shield, ref float shieldMax) {
		// Here the game wants to know if to draw the boss bar or not. Return false whenever the conditions don't apply.
		// If there is no possibility of returning false (or null) the bar will get drawn at times when it shouldn't, so write defensive code!

		NPC npc = Main.npc[info.npcIndexToAimAt];
		if (!npc.active)
			return false;

		// We assign bossHeadIndex here because we need to use it in GetIconTexture
		bossHeadIndex = npc.GetBossHeadTextureIndex();
		if (npc.ModNPC is LootBoxLord body) {
			if (npc.HasBuff<LootBoxLord_DefenseUp>()) {
				shieldMax = (float)ModUtils.ToSecond(10);
				shield = npc.buffTime[npc.FindBuffIndex(ModContent.BuffType<LootBoxLord_DefenseUp>())];
			}
			else {
				shield = 0;
				shieldMax = 0;
			}
			// We did all the calculation work on RemainingShields inside the body NPC already so we just have to fetch the value again
			//shield = body.MinionHealthTotal;
			//shieldMax = body.MinionMaxHealthTotal;
		}
		life = npc.life;
		lifeMax = npc.lifeMax;


		return true;
	}
}
