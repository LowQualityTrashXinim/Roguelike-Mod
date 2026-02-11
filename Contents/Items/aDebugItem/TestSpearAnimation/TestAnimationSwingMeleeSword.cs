using Microsoft.Xna.Framework;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Weapon.MeleeSynergyWeapon.EverlastingCold;
using Roguelike.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.aDebugItem.TestSpearAnimation;
internal class TestAnimationSwingMeleeSword : ModItem {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<EverlastingCold>();
	public override void SetDefaults() {
		Item.BossRushSetDefault(92, 92, 120, 5f, 20, 20, ItemUseStyleID.Swing, true);
		Item.DamageType = DamageClass.Melee;
		Item.UseSound = SoundID.Item1;
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul system)) {
			system.Ignore_AttackSpeed = true;
			system.AnimationEndTime = 25;
			system.SwingDegree = 155;
			system.SwingStrength = 7f;
			system.SwingType = BossRushUseStyle.SwipeDown;
		}
	}
	public override bool AltFunctionUse(Player player) => true;
	public override float UseSpeedMultiplier(Player player) {
		if (player.altFunctionUse == 2) {
			return 3f;
		}
		return base.UseSpeedMultiplier(player);
	}
	public override void HoldItem(Player player) {
		if (player.altFunctionUse == 2) {
			if (player.ItemAnimationJustStarted) {
				player.AddBuff<Guard_Buff>(player.itemAnimationMax);
				player.velocity *= .1f;
			}
			Item.GetGlobalItem<MeleeWeaponOverhaul>().HideSwingVisual = true;
		}
		else {
			Item.GetGlobalItem<MeleeWeaponOverhaul>().HideSwingVisual = false;
		}
	}
	public override void UseStyle(Player player, Rectangle heldItemFrame) {
		if (player.altFunctionUse == 2) {
			Item.GetGlobalItem<MeleeWeaponOverhaul>().DisableAttackAnimation = true;
			player.itemLocation = player.Center;
			if (player.direction == 1) {
				player.itemRotation = -MathHelper.PiOver4;
				player.itemLocation += Vector2.UnitX * 10;
			}
			else {
				player.itemRotation = MathHelper.PiOver4;
				player.itemLocation += Vector2.UnitX * -10;
			}
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, player.itemRotation - MathHelper.PiOver2 * player.direction);
		}
		else {
			Item.GetGlobalItem<MeleeWeaponOverhaul>().DisableAttackAnimation = false;
			float rotation = MathHelper.ToRadians(50) * player.direction;
			if (player.itemAnimation <= 5) {
				player.itemRotation = Item.GetGlobalItem<MeleeWeaponOverhaul>().RotationAfterMainAnimationEnd + rotation;
				player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, player.itemRotation - MathHelper.PiOver2 - (player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3));
				player.itemLocation = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, player.itemRotation - MathHelper.PiOver2 - (player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3)) + Vector2.UnitX.RotatedBy(player.itemRotation);
				return;
			}
			if (player.itemAnimation <= 25) {
				player.itemRotation = Item.GetGlobalItem<MeleeWeaponOverhaul>().RotationAfterMainAnimationEnd + rotation * (1 - (player.itemAnimation - 5) / 20f);
				player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, player.itemRotation - MathHelper.PiOver2 - (player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3));
				player.itemLocation = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, player.itemRotation - MathHelper.PiOver2 - (player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3)) + Vector2.UnitX.RotatedBy(player.itemRotation);
			}
		}
	}
	public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox) {
		if (player.altFunctionUse == 2) {
			noHitbox = true;
		}
	}
	public override void ModifyItemScale(Player player, ref float scale) {
		//if (player.itemAnimation <= 20) {
		//	float progress;
		//	if (player.itemAnimation <= 10) {
		//		progress = player.itemAnimation / 20f;
		//	}
		//	else {
		//		progress = 1 - player.itemAnimation / 20f;
		//	}
		//	scale -= progress * 1.5f;
		//}
	}
}
public class Guard_Buff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		base.Update(player, ref buffIndex);
	}
}
public class Parry_Buff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		base.Update(player, ref buffIndex);
	}
}
public class Guard_ModPlayer : ModPlayer {
	Vector2 TowardNormalize = Vector2.Zero;
	public int After_Parry = 0;
	public int Parry_Dodge = 10;
	public bool AllowDashDodge = false;
	public override void ResetEffects() {
		if (Parry_Dodge > 0 && AllowDashDodge) {
			if (TowardNormalize == Vector2.Zero) {
				TowardNormalize = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.Zero);
			}
			if (AllowDashDodge) {
				if (Parry_Dodge > 0) {
					Parry_Dodge--;
					Player.velocity = TowardNormalize * 35;
				}
				else {
					AllowDashDodge = false;
				}
			}
			After_Parry = 10;
		}
		else {
			if (--After_Parry > 0) {
				Player.velocity *= .1f;
			}
			Parry_Dodge = 10;
			AllowDashDodge = false;
			TowardNormalize = Vector2.Zero;
		}
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (Player.HasBuff<Guard_Buff>()) {
			modifiers.SourceDamage *= .15f;
			modifiers.Knockback *= .05f;
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (Player.HasBuff<Guard_Buff>()) {
			modifiers.SourceDamage *= .15f;
			modifiers.Knockback *= .05f;
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Player.HasBuff<Parry_Buff>()) {
			Player.AddImmuneTime(ImmunityCooldownID.General, 30);
			AllowDashDodge = true;
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (Player.HasBuff<Guard_Buff>()) {
			Player.AddImmuneTime(ImmunityCooldownID.General, 60);
			Player.AddBuff<Parry_Buff>(90);
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (Player.HasBuff<Guard_Buff>()) {
			Player.AddImmuneTime(ImmunityCooldownID.General, 60);
			Player.AddBuff<Parry_Buff>(90);
		}
	}
}
