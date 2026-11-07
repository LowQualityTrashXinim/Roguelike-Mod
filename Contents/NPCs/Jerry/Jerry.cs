using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;
using Roguelike.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.NPCs.TheJerry;
public class Jerry : ModNPC {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		NPCID.Sets.DontDoHardmodeScaling[Type] = true;
		NPCID.Sets.NeedsExpertScaling[Type] = false;
	}
	public override void SetDefaults() {
		NPC.lifeMax = 15000;
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
		NPC.strengthMultiplier = 1;
		NPC.despawnEncouraged = false;
		NPC.friendly = false;
		NPC.dontTakeDamage = false;
		NPC.strengthMultiplier = 0;
		NPC.dontTakeDamageFromHostiles = true;
		NPC.GetGlobalNPC<RoguelikeGlobalNPC>().NPC_SpecialException = true;
		segments = new();
	}
	List<Jerry_Segment> segments = new();
	public bool Segment_FullyCreated = false;
	public int Segment_Timer = 0;
	public Vector2 Get_LastSegmentPosition() => segments[segments.Count - 1].NPC.Center;
	public override void OnSpawn(IEntitySource source) {

	}
	public float ExtraDamage = 0;
	public override void ResetEffects() {
		ExtraDamage = 0;
	}
	public override bool PreAI() {
		if (Segment_FullyCreated) {
			return true;
		}
		if (++Segment_Timer >= 60) {
			NPC npc = NPC.NewNPCDirect(NPC.GetSource_FromThis(), NPC.Center.Add(0, 30 * segments.Count + 1), ModContent.NPCType<Jerry_Segment>());
			if (npc.ModNPC is Jerry_Segment mini_jerry) {
				mini_jerry.jerry = this;
				mini_jerry.index = segments.Count;
				segments.Add(mini_jerry);
			}
			if (segments.Count > 10) {
				Segment_FullyCreated = true;
			}
		}
		return false;
	}
	public float Attack_Timer { get => NPC.ai[0]; set => NPC.ai[0] = value; }
	public float Attack_Timer2 { get => NPC.ai[1]; set => NPC.ai[1] = value; }
	public float Attack_Pattern { get => NPC.ai[3]; set => NPC.ai[3] = value; }
	public override void AI() {
		
		switch (Attack_Pattern) {
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
				break;
		}
	}
	public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage += ExtraDamage;
	}
	public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage += ExtraDamage;
	}
}
public class Jerry_Segment : ModNPC {
	public override string Texture => ModTexture.MissingTexture_Default;
	public Jerry jerry = null;
	public override void SetDefaults() {
		NPC.lifeMax = 1000;
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
	}
	public int explodetime = 0;
	public int index = 0;
	public int timer = 30;
	public bool DeathForTimeBeing = false;
	public int DeathTimer = 0;
	public override bool PreAI() {
		if (jerry != null) {
			if (!jerry.NPC.active) {
				jerry = null;
				explodetime = 30 * (1 + index);
			}
		}
		return base.PreAI();
	}
	public override void ResetEffects() {
		if (DeathForTimeBeing) {
			jerry.ExtraDamage += .1f;
		}
	}
	public override void AI() {
		if (--DeathTimer <= 0) {
			DeathForTimeBeing = false;
			NPC.life = NPC.lifeMax;
		}
	}
	public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
		return false;
	}
	public override bool CheckDead() {
		if (!DeathForTimeBeing) {
			DeathForTimeBeing = true;
			DeathTimer = ModUtils.ToSecond(15);
		}
		return jerry == null && explodetime < 0;
	}
}
