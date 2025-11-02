using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Perks.PerkContents;
public class BloodStrike : Perk {
	public override void SetDefaults() {
		textureString = ModUtils.GetTheSameTextureAsEntity<BloodStrike>();
		CanBeStack = false;
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		damage *= 1.36f;
		damage.Flat += 7;
	}
	public override void OnUseItem(Player player, Item item) {
		if (item.IsAWeapon() && player.itemAnimation == player.itemAnimationMax && player.ItemAnimationActive) {
			int damage = (int)Math.Round(player.GetWeaponDamage(player.HeldItem) * .05f);
			player.statLife = Math.Clamp(player.statLife - damage, 0, player.statLifeMax2);
			ModUtils.CombatTextRevamp(player.Hitbox, Color.Red, "-" + damage, Main.rand.Next(-10, 40));
		}
	}
}
