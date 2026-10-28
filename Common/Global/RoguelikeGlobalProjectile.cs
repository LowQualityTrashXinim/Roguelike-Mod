using Microsoft.Xna.Framework;
using Roguelike.Common.Systems.Skill;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.BuilderItem;
using Roguelike.Contents.Items.Consumable.Ammo;
using Roguelike.Contents.Items.NoneSynergy.MagicBow;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Contents.Items.Weapon.RangeSynergyWeapon.HeavenSmg;
using Roguelike.Contents.Items.Weapon.RangeSynergyWeapon.PulseRifle;
using Roguelike.Contents.Items.Weapon.RangeSynergyWeapon.Unforgiving;
using Roguelike.Contents.Projectiles;
using Roguelike.Contents.Transfixion.Perks.PerkContents;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Global;
internal class RoguelikeGlobalProjectile : GlobalProjectile {
	public override bool InstancePerEntity => true;
	public bool CanThisProjectileBeDevoured = true;

	//This is the source controller, in the future we will add in more data type for source controller
	//But if any of the following below that doesn't match what you want or find, you can instead use Source_CustomContextInfo
	//To check your own source information and make your own declaration via parameter "misc"
	public int Source_ItemType = -1;
	public int Source_ProjectileType = -1;
	public string Source_CustomContextInfo = string.Empty;
	public bool Source_FromDeathScatterShot = false;

	public bool IsFromMinion = false;
	public bool IsFromRelic = false;
	public bool IsFromBoss = false;
	public bool IsFromNPC = false;

	public int OnKill_ScatterShot = -1;
	public bool ComeFromSkill = false;

	public int constanttimer = 0;
	public int NPC_WhoAmI = -1;
	public float TravelDistanceBeforeKill = -1f;
	public float VelocityMultiplier = 1f;
	public float CritDamage = 0;
	public int EnergyRegainOnHit = 0;
	public int SetCrit = 0;
	public int CustomDataValue = 0;
	/// <summary>
	/// A force timer that will delete projectile<br/>
	/// By default, it is set to -1<br/>
	/// Setting it to 0 will force a deletion of a projectile
	/// </summary>
	public int Force_TimeLeft = -1;
	/// <summary>
	/// This is for projectile that is spawned via duplicate projectile method<br/><br/>
	/// <b>Return true if it is from duplication</b>
	/// </summary>
	public bool IsASubProjectile = false;
	/// <summary>
	/// This is time left that is set in SetDefaults, <br/>
	/// any modification later set in the code or outside scope of NewProjectile will be ignored
	/// </summary>
	public int InitialTimeLeft { get; private set; } = 0;
	public List<ProjectileAIModify> list_modifier = new();
	public static void Add_projectileAImodify(int type, ref Projectile proj) {
		ProjectileAIModify ai = ProjectileModify_System.Get_Modify(type);
		if (ai == null) {
			return;
		}
		ai.OnAdded(proj);
		proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().list_modifier.Add(ai);
	}
	public override void SetDefaults(Projectile entity) {
		list_modifier = new();
		constanttimer = 0;
	}
	public override void OnSpawn(Projectile projectile, IEntitySource source) {
		if (source is null) {
			return;
		}
		InitialTimeLeft = projectile.timeLeft;
		if (source is EntitySource_ItemUse parent) {
			if (parent.Item.ModItem is Relic) {
				IsFromRelic = true;
			}
			Source_ItemType = parent.Item.type;
			if (Source_ItemType == ModContent.ItemType<PulseRifle>()) {
				projectile.usesLocalNPCImmunity = true;
				projectile.localNPCHitCooldown = 10;
			}
		}
		if (source is EntitySource_Misc parent2 && parent2.Context == "OnKill_ScatterShot") {
			Source_FromDeathScatterShot = true;
		}
		if (source is EntitySource_Parent parent3) {
			if (parent3.Entity is Projectile possibly) {
				Source_ProjectileType = possibly.type;
				if (possibly.minion) {
					IsFromMinion = true;
				}
				//Attempt to get item source from that minion
				if (possibly.TryGetGlobalProjectile(out RoguelikeGlobalProjectile global)) {
					if (global.Source_ItemType != 0) {
						Source_ItemType = global.Source_ItemType;
					}
				}
			}
			if (parent3.Context != null) {
				if (parent3.Context == "subProj") {
					IsASubProjectile = true;
				}
			}
			if (parent3.Entity is NPC npc) {
				if (npc.boss) {
					IsFromBoss = true;
				}
				IsFromNPC = true;
				NPC_WhoAmI = npc.whoAmI;

			}
		}
		Source_CustomContextInfo = source.Context;
		if (source.Context == "Skill") {
			ComeFromSkill = true;
		}
	}
	public override bool PreAI(Projectile projectile) {
		if (++constanttimer >= 10000000) {
			constanttimer = 0;
		}
		Pre_VelocityHandler(projectile);
		foreach (var modifier in list_modifier) {
			modifier.PreAI(projectile);
		}
		return base.PreAI(projectile);
	}
	private void Pre_VelocityHandler(Projectile projectile) {
		if (VelocityMultiplier != 0) {
			projectile.velocity /= VelocityMultiplier;
		}
		else {
			projectile.velocity /= .001f;
		}
	}
	private void HardCodedInteractionFromUnforgiving(Projectile projectile) {
		if (projectile.type == ProjectileID.ShadowFlameArrow && CustomDataValue == 1) {
			for (int i = 0; i < 2; i++) {
				Dust flame = Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Shadowflame);
				flame.velocity = projectile.velocity * -.1f;
				flame.position += Main.rand.NextVector2Circular(8, 8);
				flame.scale = Main.rand.NextFloat(.8f, 1.1f);
			}
			if (projectile.timeLeft % 10 == 0) {
				Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center + Main.rand.NextVector2Circular(12, 12), Vector2.Zero, ModContent.ProjectileType<Roguelike_SpiritFlame>(), (int)(projectile.damage * .7f), 2, projectile.owner);
			}
		}
	}
	private void Post_VelocityHandler(Projectile projectile, Player player) {
		if (VelocityMultiplier != 0) {
			projectile.velocity *= VelocityMultiplier;
		}
		else {
			projectile.velocity *= .001f;
		}
		if (projectile.hostile) {
			VelocityMultiplier = 1f + player.GetModPlayer<PlayerStatsHandle>().Hostile_ProjectileVelocityAddition;
		}
		else {
			VelocityMultiplier = 1f;
		}
	}
	public override void PostAI(Projectile projectile) {
		foreach (var modifier in list_modifier) {
			modifier.PostAI(projectile);
		}
		HardCodedInteractionFromUnforgiving(projectile);
		if (Force_TimeLeft != -1) {
			Force_TimeLeft--;
			if (Force_TimeLeft == 0) {
				projectile.Kill();
			}
		}
		var player = Main.player[projectile.owner];
		Post_VelocityHandler(projectile, player);
		if (TravelDistanceBeforeKill > 0 && Vector2.DistanceSquared(player.Center, projectile.Center) >= TravelDistanceBeforeKill * TravelDistanceBeforeKill) {
			projectile.Kill();
		}
	}
	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Source_CustomContextInfo == "Skill" && projectile.type == ProjectileID.Blizzard) {
			var player = Main.player[projectile.owner];
			target.AddBuff(BuffID.Frozen, ModUtils.ToSecond(Main.rand.Next(4, 7)));
			target.Center.LookForHostileNPC(out var npclist, 75);
			var hitweaker = hit;
			hitweaker.Damage = (int)(hit.Damage * .54f);
			foreach (var npc in npclist) {
				if (npc.whoAmI == target.whoAmI) {
					continue;
				}
				player.StrikeNPCDirect(npc, hitweaker);
			}
		}
		foreach (var modifier in list_modifier) {
			modifier.OnHitNPC(projectile, target);
		}
	}
	int BouncyCollide = 0;
	public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity) {
		var player = Main.player[projectile.owner];
		RoguelikeGlobalProjectile global = projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>();
		if (player.GetModPlayer<SlimyChalicePlayer>().Bouncy && !projectile.minion && !projectile.hostile
			&& projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType != ItemID.None) {
			projectile.tileCollide = true;
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
			if (projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
			if (projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
			if (projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().Force_TimeLeft == -1)
				projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().Force_TimeLeft = 180;
			if (++BouncyCollide > 1) return false;
			projectile.damage = (int)(projectile.damage * 1.2f);
			return false;
		}
		return true;
	}
	public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers) {
		if (IsFromNPC) {
			if (NPC_WhoAmI >= 0 && NPC_WhoAmI < Main.npc.Length) {
				if (Main.npc[NPC_WhoAmI].TryGetGlobalNPC(out RoguelikeGlobalNPC global)) {
					modifiers.SourceDamage = modifiers.SourceDamage.CombineWith(global.DamageIncrease.CombineWith(global.Static_DamageIncrease));
				}
			}
		}
	}
	public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) {
		Player player = Main.player[projectile.owner];
		player.EnergyHeal(EnergyRegainOnHit);
		modifiers.CritDamage += CritDamage;
	}
	public override void OnKill(Projectile projectile, int timeLeft) {
		foreach (var modifier in list_modifier) {
			modifier.OnKill(projectile);
		}
		var player = Main.player[projectile.owner];
		if (Source_FromDeathScatterShot
			|| OnKill_ScatterShot <= 0
			|| player.heldProj == projectile.owner
			|| ProjectileID.Sets.SingleGrappleHook[projectile.type]
			|| projectile.hostile
			|| !projectile.friendly
			|| projectile.minion
			|| projectile.sentry
			|| projectile.aiStyle == ProjAIStyleID.Vilethorn
			|| projectile.aiStyle == ProjAIStyleID.Spear
			|| projectile.aiStyle == ProjAIStyleID.MechanicalPiranha
			|| projectile.aiStyle == ProjAIStyleID.Rainbow
			|| projectile.aiStyle == ProjAIStyleID.HeldProjectile
			|| projectile.aiStyle == ProjAIStyleID.Yoyo
			|| projectile.aiStyle == ProjAIStyleID.HorizontalRay
			|| projectile.aiStyle == ProjAIStyleID.Hook
			|| projectile.type == ProjectileID.PhantasmArrow
			|| projectile.type == ProjectileID.IchorDart
			|| projectile.type == ProjectileID.ExplosiveBunny
			|| projectile.type == ProjectileID.FinalFractal
			|| projectile.type == ProjectileID.PortalGun
			|| projectile.type == ProjectileID.PortalGunBolt
			|| projectile.type == ProjectileID.PortalGunGate
			|| projectile.type == ProjectileID.LightsBane
			|| projectile.type == ModContent.ProjectileType<LeafProjectile>()
			|| projectile.type == ModContent.ProjectileType<AngelicSmgHeld>()
			|| projectile.type == ModContent.ProjectileType<MagicBullet>()//This is to prevent lag
			|| projectile.type == ModContent.ProjectileType<DiamondGemP>()
			|| projectile.type == ModContent.ProjectileType<ArenaMakerProj>()
			|| projectile.type == ModContent.ProjectileType<NeoDynamiteExplosion>()
			|| projectile.type == ModContent.ProjectileType<TowerDestructionProjectile>()) {
			return;
		}
		if (!projectile.velocity.IsLimitReached(1)) {
			projectile.velocity *= Main.rand.NextFloat(5, 7);
		}
		for (int i = 0; i < OnKill_ScatterShot; i++) {
			int proj = Projectile.NewProjectile(projectile.GetSource_Misc("OnKill_ScatterShot"), projectile.Center, projectile.velocity.Vector2RotateByRandom(360), projectile.type, (int)(projectile.damage * .65f), projectile.knockBack * .55f, projectile.owner);
			Main.projectile[proj].timeLeft = 120;
		}
	}
}
public class ProjectileModify_System : ModSystem {
	private static List<ProjectileAIModify> _modify = new();
	public static int TotalCount => _modify.Count;
	public static short Register(ProjectileAIModify skill) {
		if (_modify == null) {
			_modify = new();
		}
		ModTypeLookup<ProjectileAIModify>.Register(skill);
		_modify.Add(skill);
		return (short)(_modify.Count - 1);
	}
	public static ProjectileAIModify Get_Modify(int type) {
		return type >= 0 && type < _modify.Count ? _modify[type] : null;
	}
	public override void Unload() {
		_modify = null;
	}
}
public abstract class ProjectileAIModify : ModType {
	short Type = 0;
	public static short Get_ModifyType<T>() where T : ProjectileAIModify => ModContent.GetInstance<T>().Type;
	#region sealing basic hook 
	protected sealed override void Register() {
		Type = ProjectileModify_System.Register(this);
	}
	public override sealed void SetupContent() {
		base.SetupContent();
	}
	public override sealed void Load() { }
	public override sealed bool Equals(object obj) {
		return base.Equals(obj);
	}
	public override sealed void SetStaticDefaults() {
		base.SetStaticDefaults();
	}
	public override sealed bool IsLoadingEnabled(Mod mod) {
		return base.IsLoadingEnabled(mod);
	}
	public override sealed int GetHashCode() {
		return base.GetHashCode();
	}
	public override sealed void Unload() {
		base.Unload();
	}
	public override sealed string ToString() {
		return base.ToString();
	}
	public override sealed string Name => base.Name;
	#endregion
	public virtual void OnAdded(Projectile projectile) {

	}
	public virtual void PreAI(Projectile projectile) {

	}
	public virtual void PostAI(Projectile projectile) {

	}
	public virtual void OnHitNPC(Projectile projectile, NPC target) {

	}
	public virtual void OnKill(Projectile projectile) {

	}
}
