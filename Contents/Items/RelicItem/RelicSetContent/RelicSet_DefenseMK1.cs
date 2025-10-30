using Roguelike.Common.Utils;
using Roguelike.Texture;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.RelicItem.RelicSetContent;
public class DefenseMK1_ModPlayer : ModPlayer {
	public class RelicSet_DefenseMK1 : RelicSet {
		public override void SetStaticDefaults() {
			Requirement = 2;
		}
	}
	public bool set => RelicSetSystem.Check_RelicSetRequirment(Player, RelicSet.GetRelicSetType<RelicSet_DefenseMK1>());
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (set) {
			if (Main.rand.NextBool(5)) {
				modifiers.SourceDamage *= .5f;
				Player.AddBuff<DefenseMK1_Buff>(ModUtils.ToSecond(15));
			}
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (set) {
			if (Main.rand.NextBool(5)) {
				modifiers.SourceDamage *= .5f;
				Player.AddBuff<DefenseMK1_Buff>(ModUtils.ToSecond(15));
			}
		}
	}
	public class DefenseMK1_Buff : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			player.GetDamage<GenericDamageClass>() += .2f;
		}
	}
}
