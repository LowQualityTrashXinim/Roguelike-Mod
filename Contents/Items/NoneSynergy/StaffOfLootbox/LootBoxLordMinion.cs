using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Lootbox;
using Roguelike.Contents.Items.NoneSynergy.StaffOfLootbox.Projectiles;
using Roguelike.Contents.NPCs.LootBoxLord.HostileProjectile;
using Roguelike.Texture;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.NoneSynergy.StaffOfLootbox;
internal class LootBoxLordSummon : ModItem {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<WoodenLootBox>();
	public override void SetStaticDefaults() {
		ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
		ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
	}
	public override void SetDefaults() {
		Item.damage = 100;
		Item.mana = 100;
		Item.width = 32;
		Item.height = 32;
		Item.useTime = Item.useAnimation = 20;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.value = Item.sellPrice(gold: 30);
		Item.rare = ItemRarityID.Cyan;
		Item.UseSound = SoundID.Item44;

		Item.noMelee = true;
		Item.DamageType = DamageClass.Summon;
		Item.buffType = ModContent.BuffType<LootBoxLordProtection>();

		Item.shoot = ModContent.ProjectileType<LootBoxLord_Minion>();
	}
	public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position = Main.MouseWorld;
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		player.AddBuff(Item.buffType, 2);
		var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
		projectile.originalDamage = Item.damage;
		return false;
	}

	// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.SlimeStaff)
			.AddIngredient(ItemID.AbigailsFlower)
			.Register();
	}
}
public class LootBoxLordProtection : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		// DisplayName.SetDefault("slimy ghost friend");
		// Description.SetDefault("Will tries his best");

		Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
		Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
	}

	public override void Update(Player player, ref int buffIndex) {
		// If the minions exist reset the buff time, otherwise remove the buff from the player
		if (player.ownedProjectileCounts[ModContent.ProjectileType<LootBoxLord_Minion>()] > 0) {
			player.buffTime[buffIndex] = 18000;
		}
		else {
			player.DelBuff(buffIndex);
			buffIndex--;
		}
	}
}
internal class LootBoxLord_Minion : ModProjectile {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<WoodenLootBox>();
	public override void SetStaticDefaults() {
		ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		Main.projPet[Projectile.type] = true;
		ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
		ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
	}
	public override void SetDefaults() {
		//NPC.damage = 100;
		Projectile.width = 38;
		Projectile.height = 30;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.minion = true;
		Projectile.minionSlots = 1;
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
		Projectile.position = TeleportPosition;
		ChoosenPosition = Vector2.Zero;
		TeleportPosition = Vector2.Zero;
		TeleportTime = 0;
		return true;
	}
	public override bool PreAI() {
		var owner = Main.player[Projectile.owner];
		if (owner.dead || !owner.active) {
			owner.ClearBuff(ModContent.BuffType<LootBoxLordProtection>());
			return false;
		}

		if (owner.HasBuff(ModContent.BuffType<LootBoxLordProtection>())) {
			Projectile.timeLeft = 2;
		}
		return base.PreAI();
	}
	public int BossDamagePercentage(float percentage) => (int)Math.Ceiling(Projectile.damage * percentage);
	//Use NPC.ai[0] to delay attack
	//Use NPC.ai[1] to switch attack
	//Use NPC.ai[2] for calculation
	//Use NPC.ai[3] to do movement
	bool CanSlowDown = false;
	public override void AI() {
		var truePlayer = Main.player[Projectile.owner];
		NPC player = null;
		if (truePlayer.HasMinionAttackTargetNPC) {
			player = Main.npc[truePlayer.MinionAttackTargetNPC];
		}
		else {
			Projectile.Center.LookForHostileNPC(out player, 1000, true);
		}
		if (player == null) {
			var positionAbovePlayer = new Vector2(truePlayer.Center.X, truePlayer.Center.Y - 30);
			var distance = positionAbovePlayer - Projectile.Center;
			if (distance.Length() <= 50f) {
				Projectile.velocity = Vector2.Zero;
				CurrentAttack++;
			}
			else {
				Projectile.velocity = distance.SafeNormalize(Vector2.Zero) * 30f;
			}
			return;
		}
		Lighting.AddLight(Projectile.Center, 1, 1, 1);
		if (CanSlowDown) {
			Projectile.velocity *= .98f;
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
				ShootWoodBow();
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
	private void DeseperationAttack(Entity player) {
		if (AttackCounter <= 0) {
			AttackCounter++;
			int proj = ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, -Vector2.UnitY * 3, ModContent.ProjectileType<HostileMinisharkDesperation>(),
			BossDamagePercentage(.35f), 2, Projectile.owner);
			if (Main.projectile[proj].ModProjectile is BaseProjectile projectile)
				projectile.SetNPCOwner(Projectile.whoAmI);
		}
		AttackTimer++;
		if (AttackTimer % 12 == 0) {
			for (int i = 0; i < 2; i++) {
				int proj = ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One.RotatedBy(MathHelper.ToRadians(180 * i + AttackTimer * 2)) * 3, ModContent.ProjectileType<SwordBroadDesperation>(), BossDamagePercentage(.6f), 2, Projectile.owner);
				if (Main.projectile[proj].ModProjectile is BaseProjectile projectile) {
					projectile.IDtextureValue = Main.rand.Next(TerrariaArrayID.AllOreBroadSword);
					projectile.Projectile.timeLeft = 180;
				}
			}
		}
		if (AttackTimer % 96 == 0) {
			for (int i = 0; i < 16; i++) {
				int proj = ModUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero).Vector2DistributeEvenlyPlus(16, 360, i) * 10, ModContent.ProjectileType<ShortSwordDesperation>(), BossDamagePercentage(.75f), 2, Projectile.owner);
				if (Main.projectile[proj].ModProjectile is BaseProjectile projectile) {
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

	private void Move(Entity player) {
		var positionAbovePlayer = new Vector2(player.Center.X, player.Center.Y - 200);
		var distance = positionAbovePlayer - Projectile.Center;
		if (distance.Length() <= 30f) {
			Projectile.velocity = Vector2.Zero;
			CurrentAttack++;
		}
		Projectile.velocity = distance.SafeNormalize(Vector2.Zero) * 30f;
	}
	public float AttackTimer { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
	public float CurrentAttack { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }
	public float AttackCounter { get => Projectile.ai[2]; set => Projectile.ai[2] = value; }
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
		var vec = -Vector2.UnitY.Vector2DistributeEvenly(8, 120, (int)AttackCounter) * 12f;
		int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vec, ModContent.ProjectileType<LBL_ShortSwordAttackOne>(),
			BossDamagePercentage(.75f), 2, Projectile.owner);
		if (Main.projectile[proj].ModProjectile is BaseProjectile projectile)
			projectile.IDtextureValue = TerrariaArrayID.AllOreShortSword[(int)AttackCounter];
		proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, -vec, ModContent.ProjectileType<LBL_ShortSwordAttackOne>(),
			BossDamagePercentage(.75f), 2, Projectile.owner);
		if (Main.projectile[proj].ModProjectile is BaseProjectile projectile2)
			projectile2.IDtextureValue = TerrariaArrayID.AllOreShortSword[(int)AttackCounter];
		AttackCounter++;
	}
	private void ShootShortSword2(Entity player) {
		//Custom boss movement oooohhh
		ChoosenPosition = player.Center.Add(0, 300);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
			}
			return;
		}
		CanSlowDown = true;
		var distance = player.Center - Projectile.Center;
		Projectile.velocity = distance.SafeNormalize(Vector2.Zero) * distance.Length() / 64f;
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
		int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vec, ModContent.ProjectileType<LBL_ShortSwordAttackTwo>(), BossDamagePercentage(.75f), 2, Projectile.owner);
		if (Main.projectile[proj].ModProjectile is BaseProjectile projectile)
			projectile.IDtextureValue = TerrariaArrayID.AllOreShortSword[(int)AttackCounter];
		Main.projectile[proj].ai[1] = -20;
		Main.projectile[proj].ai[0] = 2;
		Main.projectile[proj].rotation = Main.projectile[proj].velocity.ToRotation() + MathHelper.PiOver4;
		int proj2 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, -Vector2.UnitY * 20, ModContent.ProjectileType<LBL_ShortSwordAttackTwo>(), BossDamagePercentage(.75f), 2, Projectile.owner);
		if (Main.projectile[proj2].ModProjectile is BaseProjectile projectile2)
			projectile2.IDtextureValue = TerrariaArrayID.AllOreShortSword[(int)AttackCounter];
		Main.projectile[proj2].ai[1] = -20;
		Main.projectile[proj2].ai[0] = 2;
		Main.projectile[proj2].rotation = Main.projectile[proj2].velocity.ToRotation() + MathHelper.PiOver4;
		AttackCounter++;
	}
	private void ShootBroadSword(Entity player) {
		//ahh movment, after the attack counter reach
		if (AttackCounter >= TerrariaArrayID.AllOreBroadSword.Length) {
			if (Projectile.velocity.IsLimitReached(1f)) {
				Projectile.velocity *= .8f;
			}
			else {
				UniversalAttackCoolDown = 35;
				AttackTimer = 0;
				AttackCounter = 0;
				CurrentAttack++;
				Projectile.velocity = Vector2.Zero;
			}
			return;
		}//Before attack counter reach
		else {
			if (AttackTimer == 0 && AttackCounter == 0) {
				lastPlayerPosition = player.Center;
				var distance2 = player.Center.Add(0, 300) - Projectile.Center;
				if (distance2.LengthSquared() > 100 * 100) {
					Projectile.velocity = distance2.SafeNormalize(Vector2.Zero) * distance2.Length() / 8f;
					return;
				}
			}
			var distance = lastPlayerPosition - Projectile.Center;
			Projectile.velocity = distance.SafeNormalize(Vector2.Zero) * 20;
			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(90));
		}
		if (++AttackTimer < 10) {
			return;
		}
		AttackTimer = 0;
		var vec = (Projectile.Center - player.Center).SafeNormalize(Vector2.Zero) * 10;
		int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vec, ModContent.ProjectileType<LBL_SwordBroadAttackOne>(), BossDamagePercentage(.85f), 2, Projectile.owner);
		if (Main.projectile[proj].ModProjectile is LBL_SwordBroadAttackOne swordProj) {
			swordProj.SetNPCOwner(Projectile.whoAmI);
			swordProj.IDtextureValue = TerrariaArrayID.AllOreBroadSword[(int)AttackCounter];
		}
		AttackCounter++;
	}
	private void ShootBroadSword2(Entity player) {
		ChoosenPosition = player.Center.Add(0, 200);
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
				int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vec, ModContent.ProjectileType<LBL_SwordBroadAttackTwo>(), BossDamagePercentage(.85f), 2, Projectile.owner);
				Main.projectile[proj].ai[1] = 35;
				if (Main.projectile[proj].ModProjectile is BaseProjectile projectile)
					projectile.IDtextureValue = TerrariaArrayID.AllOreBroadSword[i];
			}
		}
		else {
			for (int i = 0; i < TerrariaArrayID.AllOreBroadSword.Length; i++) {
				var vec = -Vector2.UnitY.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllOreBroadSword.Length, 90, i) * 20f;
				int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vec, ModContent.ProjectileType<LBL_SwordBroadAttackTwo>(), BossDamagePercentage(.85f), 2, Projectile.owner);
				Main.projectile[proj].ai[1] = 35;
				if (Main.projectile[proj].ModProjectile is BaseProjectile projectile)
					projectile.IDtextureValue = TerrariaArrayID.AllOreBroadSword[i];
			}
		}
		UniversalAttackCoolDown = 60;
		CurrentAttack++;
		AttackCounter = 0;
		AttackTimer = 0;
		CanTeleport = true;
	}
	private void ShootWoodBow() {
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
		int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One.InverseVector2DistributeEvenly(TerrariaArrayID.AllWoodBowPHM.Length, 360, AttackCounter) * 3, ModContent.ProjectileType<LBL_WoodBowAttackOne>(), BossDamagePercentage(.65f), 2, Projectile.owner);
		if (Main.projectile[proj].ModProjectile is BaseProjectile projectile) {
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
			int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vec, ModContent.ProjectileType<LBL_WoodBowAttackTwo>(), BossDamagePercentage(.45f), 2, Projectile.owner);
			if (Main.projectile[proj].ModProjectile is BaseProjectile projectile)
				projectile.IDtextureValue = TerrariaArrayID.AllWoodBowPHM[i];
		}
		CurrentAttack++;
		AttackCounter = 0;
		AttackTimer = 0;
		UniversalAttackCoolDown = 145;
	}
	private void ShootStaff(Entity player) {
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
			Projectile.velocity = Vector2.Zero;
			return;
		}
		AttackTimer = 0;
		if (AttackCounter >= TerrariaArrayID.AllGemStaffPHM.Length) {
			Reset(CurrentAttack + 1, 165);
			return;
		}
		var vec = Vector2.UnitY.RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(0, 360, percent)));
		int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vec, ModContent.ProjectileType<LBL_GemStaffAttackOne>(), BossDamagePercentage(.75f), 2, Projectile.owner);
		if (Main.projectile[proj].ModProjectile is BaseGemStaff gemstaffProj) {
			gemstaffProj.IDtextureValue = TerrariaArrayID.AllGemStaffPHM[(int)AttackCounter];
			gemstaffProj.ProjectileType = TerrariaArrayID.AllGemStafProjectilePHM[(int)AttackCounter];
		}
		AttackCounter++;
	}
	private void ShootStaff2(Entity player) {
		ChoosenPosition = player.Center.Add(0, 150);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
				lastPlayerPosition = player.Center;
			}
			return;
		}
		CanSlowDown = true;
		var distance = player.Center - Projectile.Center;
		Projectile.velocity = distance.SafeNormalize(Vector2.Zero) * distance.Length() / 32f;
		if (++AttackTimer <= 40) {
			return;
		}
		for (int i = 0; i < TerrariaArrayID.AllGemStaffPHM.Length; i++) {
			int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2Circular(12, 12), ModContent.ProjectileType<LBL_GemStaffAttackTwo>(), BossDamagePercentage(.75f), 2, Projectile.owner);
			Main.projectile[proj].ai[2] = i;
			Main.projectile[proj].rotation = MathHelper.PiOver4 + MathHelper.PiOver2;
			if (Main.projectile[proj].ModProjectile is BaseGemStaff gemstaffProj) {
				gemstaffProj.SetNPCOwner(Projectile.whoAmI);
				gemstaffProj.IDtextureValue = TerrariaArrayID.AllGemStaffPHM[i];
				gemstaffProj.ProjectileType = TerrariaArrayID.AllGemStafProjectilePHM[i];
			}
		}
		Reset(CurrentAttack + 1, 180);
	}
	private void ShootOreBow1(Entity player) {
		ChoosenPosition = player.Center.Add(0, 150);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
				lastPlayerPosition = player.Center;
			}
			return;
		}
		if (++AttackTimer <= 20) {
			Projectile.velocity = Vector2.Zero;
			return;
		}
		AttackTimer = 0;
		if (AttackCounter >= TerrariaArrayID.AllOreBowPHM.Length) {
			Reset(CurrentAttack + 1, 145);
		}
		var vec = Vector2.UnitY.Vector2DistributeEvenly(8, 360, (int)AttackCounter) * 10f;
		int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vec, ModContent.ProjectileType<LBL_OreBowAttackOne>(), BossDamagePercentage(.55f), 2, Projectile.owner);
		if (Main.projectile[proj].ModProjectile is BaseProjectile projectile)
			projectile.IDtextureValue = TerrariaArrayID.AllOreBowPHM[(int)AttackCounter];
		Main.projectile[proj].ai[1] = 60;
		AttackCounter++;
	}
	private void ShootOreBow2(Entity player) {
		ChoosenPosition = player.Center.Add(0, 350);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
				lastPlayerPosition = player.Center;
			}
			return;
		}
		if (AttackCounter >= TerrariaArrayID.AllOreBowPHM.Length - 1) {
			if (Projectile.velocity.IsLimitReached(1f)) {
				Projectile.velocity *= .8f;
			}
			else {
				Projectile.velocity = Vector2.Zero;
				AttackCounter = 0;
				AttackTimer = 0;
				UniversalAttackCoolDown = 145;
				CurrentAttack++;
			}
			return;
		}
		var positionAbovePlayer = player.Center + new Vector2(0, -350);
		var distance = positionAbovePlayer - Projectile.Center;
		if (distance.Length() <= 30) {
			Projectile.velocity = Vector2.Zero;
		}
		else {
			Projectile.velocity = distance.SafeNormalize(Vector2.Zero) * 5f;
		}
		if (++AttackTimer < 10) {
			return;
		}
		AttackTimer = 0;
		int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<LBL_OreBowAttackTwo>(), BossDamagePercentage(.55f), 2, Projectile.owner);
		if (Main.projectile[proj].ModProjectile is BaseProjectile projectile)
			projectile.IDtextureValue = TerrariaArrayID.AllOreBowPHM[(int)AttackCounter];
		AttackCounter++;
	}
	private void ShootGun(Entity player) {
		ChoosenPosition = player.Center.Add(0, 100);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
			}
			return;
		}
		if (++AttackTimer >= 300) {
			Reset(CurrentAttack + 1, 30);
			return;
		}
		if (AttackCounter >= 1) {
			return;
		}
		int direction;
		if (player.Center.X > Projectile.Center.X) {
			direction = 1;
		}
		else {
			direction = -1;
		}
		int minishark = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<LBL_Minishark>(), BossDamagePercentage(.25f), 2, Projectile.owner);
		if (Main.projectile[minishark].ModProjectile is BaseGun minisharkproj) {
			minisharkproj.IDtextureValue = ItemID.Minishark;
			minisharkproj.SetNPCOwner(Projectile.whoAmI);
		}
		int Musket = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<LBL_Musket>(), Projectile.damage, 2, Projectile.owner);
		if (Main.projectile[Musket].ModProjectile is BaseGun musketproj) {
			musketproj.IDtextureValue = ItemID.Musket;
			Main.projectile[Musket].ai[2] = direction;
			musketproj.SetNPCOwner(Projectile.whoAmI);
		}
		AttackCounter++;
	}
	private void ShootGun2(Entity player) {
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
		if (++AttackCounter >= 6) {
			Reset(CurrentAttack + 1, 60);
			return;
		}
		int boomStick = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 20, ModContent.ProjectileType<LBL_BoomStick>(), BossDamagePercentage(.25f), 2, Projectile.owner);
		if (Main.projectile[boomStick].ModProjectile is BaseGun hostileGun) {
			hostileGun.IDtextureValue = ItemID.Boomstick;
			hostileGun.SetNPCOwner(Projectile.whoAmI);
		}
	}
	private void SpearAttack(Entity player) {
		ChoosenPosition = player.Center.Add(0, 350);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
				lastPlayerPosition = player.Center;
			}
			return;
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
			int Spear = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One.Vector2DistributeEvenlyPlus(16, 360, i) * 6, ModContent.ProjectileType<LBL_SpearAttackOne>(), Projectile.damage, 2, Projectile.owner);
			if (Main.projectile[Spear].ModProjectile is LBL_SpearAttackOne hostileGun) {
				hostileGun.IDtextureValue = ItemID.Spear;
				hostileGun.SetNPCOwner(Projectile.whoAmI);
				hostileGun.TowardTo = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 15;
			}
		}
		AttackCounter++;
	}
	private void BookAttack(Entity player) {
		ChoosenPosition = player.Center.Add(0, 150);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
				lastPlayerPosition = player.Center;
			}
			return;
		}
		if (AttackCounter <= 0) {
			int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<LBL_Waterbolt>(), Projectile.damage, 1, Projectile.owner);
			Projectile projectile = Main.projectile[proj];
			if (projectile.ModProjectile is LBL_Waterbolt book1) {
				book1.SetNPCOwner(Projectile.whoAmI);
			}
			proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<LBL_BookOfSkull>(), Projectile.damage, 1, Projectile.owner);
			projectile = Main.projectile[proj];
			if (projectile.ModProjectile is LBL_BookOfSkull book2) {
				book2.SetNPCOwner(Projectile.whoAmI);
			}
			proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<LBL_DemonScythe>(), Projectile.damage, 1, Projectile.owner);
			projectile = Main.projectile[proj];
			if (projectile.ModProjectile is LBL_DemonScythe book3) {
				book3.SetNPCOwner(Projectile.whoAmI);
			}
			AttackCounter++;
		}
		Reset(CurrentAttack + 1, 240);
	}
	private void BulletHell(Entity player) {
		ChoosenPosition = player.Center.Add(0, 250);
		if (CanTeleport) {
			if (Teleporting(ChoosenPosition)) {
				CanTeleport = false;
				lastPlayerPosition = player.Center;
			}
			return;
		}
		if (AttackCounter <= 0) {
			for (int i = 0; i < 4; i++) {
				int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<LBL_PistolAttackOne>(), BossDamagePercentage(.45f), 1, Projectile.owner);
				Projectile projectile = Main.projectile[proj];
				if (projectile.ModProjectile is LBL_PistolAttackOne pistol) {
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
					pistol.SetNPCOwner(Projectile.whoAmI);
					projectile.ai[0] = 90 * i;
				}
			}
			int proj2 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY * -3, ModContent.ProjectileType<LBL_PistolAttackTwo>(), Projectile.damage, 1, Projectile.owner);
			Projectile projectile2 = Main.projectile[proj2];
			if (projectile2.ModProjectile is LBL_PistolAttackTwo pistol2) {
				pistol2.IDtextureValue = ItemID.PhoenixBlaster;
				pistol2.SetNPCOwner(Projectile.whoAmI);
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
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		var texture = TextureAssets.Projectile[Type].Value;
		var screenPos = Main.screenPosition;
		var drawColor = lightColor;
		var origin = Vector2.Zero;
		if (IsTeleporting) {
			float progress = TeleportTime / (float)TeleportDuration;
			if (progress <= .5f) {
				float progress1sthalf = 1 - TeleportTime / (TeleportDuration * .5f);
				Main.EntitySpriteDraw(texture, Projectile.position - screenPos, null, drawColor.ScaleRGB(progress1sthalf) with { A = (byte)(255 * progress1sthalf) }, 0, origin, 1f, SpriteEffects.None);
			}
			else {
				float progresshalf = 1 - (TeleportTime - 30) * 2 / (float)TeleportDuration;
				drawColor = drawColor.ScaleRGB(1 - progresshalf) with { A = (byte)(255 * (1 - progresshalf)) };
				for (int i = 0; i < 4; i++) {
					Main.EntitySpriteDraw(texture, TeleportPosition + Vector2.One.RotatedBy(90 * (i + 1) * progresshalf) * 100 * progresshalf - screenPos, null, drawColor, 0, origin, 1f, SpriteEffects.None);

				}
			}
		}
		else {
			Main.EntitySpriteDraw(texture, Projectile.position - screenPos, null, drawColor, 0, origin, 1f, SpriteEffects.None);
		}
		return false;
	}
	Vector2 lastPlayerPosition = Vector2.Zero;
	private void BossCircleMovement(int delayValue, int AttackEndValue, out float percent) {
		float total = delayValue * AttackEndValue;
		percent = Math.Clamp(((delayValue - AttackTimer >= 0 ? delayValue - AttackTimer : 0) + delayValue * AttackCounter) / total, 0, 1f);
		float rotation = MathHelper.Lerp(0, 360, percent);
		var rotateAroundPlayerCenter = lastPlayerPosition - Vector2.UnitY.RotatedBy(MathHelper.ToRadians(rotation)) * 350;
		Projectile.Center = rotateAroundPlayerCenter;
	}
}
