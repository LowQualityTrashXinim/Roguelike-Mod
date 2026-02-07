using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Contents.Projectiles;
using Roguelike.Texture;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.NPCs.SwordSage;
//Sword sage : Nadeqye
internal class SwordSage : ModNPC {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override string BossHeadTexture => Texture;
	public override void SetStaticDefaults() {
		NPCID.Sets.DontDoHardmodeScaling[Type] = true;
		NPCID.Sets.NeedsExpertScaling[Type] = false;
		NPCID.Sets.TrailCacheLength[Type] = 50;

	}
	public override void SetDefaults() {
		NPC.lifeMax = 25000;
		NPC.damage = 300;
		NPC.defense = 30;
		NPC.width = 38;
		NPC.height = 30;
		NPC.HitSound = SoundID.NPCHit4;
		NPC.DeathSound = SoundID.NPCDeath1;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.netAlways = true;
		NPC.knockBackResist = .1f;
		NPC.boss = true;
		NPC.dontTakeDamage = true;
		NPC.strengthMultiplier = 1;
		NPC.despawnEncouraged = false;
		NPC.friendly = false;
		NPC.strengthMultiplier = 0;
		NPC.dontTakeDamageFromHostiles = true;
		NPC.GetGlobalNPC<RoguelikeGlobalNPC>().NPC_SpecialException = true;
		//Set special boss health bar here
		//NPC.BossBar = ModContent.GetInstance<LootBoxLordBossBossBar>();
		if (!Main.dedServ) {
			Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/SwordSage_BossMusic");
		}
	}
	public float AttackTimer { get => NPC.ai[0]; set => NPC.ai[0] = value; }
	public float CurrentAttack { get => NPC.ai[1]; set => NPC.ai[1] = value; }
	public float AttackCounter { get => NPC.ai[2]; set => NPC.ai[2] = value; }
	public int UniversalAttackCoolDown = 0;
	public override void AI() {
		if (UniversalAttackCoolDown > 0) {
			UniversalAttackCoolDown--;
			return;
		}
	}
	int Attack1_Counter = 0;
	int Attack1_AttackCounter = 0;
	bool Attack1_Teleport = false;
	private void Attack1(Player player) {
		if (Attack1_Counter <= 0) {
			return;
		}
		if (!Attack1_Teleport) {
			NPC.Center = player.Center.Add(100 * -player.direction, 0);
			Attack1_Teleport = true;
		}
		if (Attack1_Teleport) {
			if (++Attack1_AttackCounter >= 60) {
				//Unlease barrage of strike
				Attack1_Teleport = false;
				Attack1_Counter--;
			}
		}

	}
	int Targetting_Timer = 0;
	Vector2 Target_Pos = Vector2.Zero;
	private void DashAttack(Player player) {
		NPC.velocity *= .9f;
		if (AttackCounter < 5) {
			if (--Targetting_Timer > 0) {
				Target_Pos = player.Center;
				AttackTimer = 10;
				if (AttackCounter == 0) {
					NPC.velocity = player.velocity;
				}
			}
			else {
				if (--AttackTimer <= 0) {
					SoundEngine.PlaySound(SoundID.Item1 with { Pitch = -1 });
					Targetting_Timer = 10;
					Vector2 vel = (Target_Pos - NPC.Center).SafeNormalize(Vector2.Zero) * 300;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, -vel, ModContent.ProjectileType<SimplePiercingProjectile2_Hostile>(), 60, 4, NPC.target, 1, 5, 100);
					NPC.Center = Target_Pos + vel;
					AttackCounter++;
				}
			}
			return;
		}
		Reset(CurrentAttack + 1, 90);
	}
	public void Reset(float nextAttack, int cooldown) {
		CurrentAttack = nextAttack;
		AttackCounter = 0;
		AttackTimer = 0;
		UniversalAttackCoolDown = cooldown;
	}
	//Dialog during boss fight
	//When start the fight
	/*
	 * It is rare to see someone travel this far
	 * You must be here to kill me I assumed
	 * No need for speech, I care not for your intention
	 * I am Nadeqye, Sword of Mias, and I will protect the last of Mias history
	 */
	//For dealing damage
	/*
	 * - Fatal mistake
	 * - It hurt doesn't it
	 * - You shall die
	 * - I shall grant you a quick death
	 */
	//When reach below 50% health
	/*
	 * Don't get your hope up, now is where you meet your end
	 */
	//When reach below 20% health
	/*
	 * Not like this
	 */
	//When defeated
	/*
	 * Sorry my dear, I failed to protect what left of us
	 */
	public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage -= .25f;
	}
}
