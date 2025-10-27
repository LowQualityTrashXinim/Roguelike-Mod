using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Roguelike.Contents.Perks.PerkContents;
public class ArcaneMaster : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 2;
		textureString = ModUtils.GetTheSameTextureAsEntity<ArcaneMaster>();
	}
	public override void UpdateEquip(Player player) {
		player.ModPlayerStats().AddStatsToPlayer(PlayerStats.MagicDMG, Multiplicative: 1.1f);
		float addition = (StackAmount(player) - 1) * .05f;
		player.manaCost -= .15f + addition;
		player.GetModPlayer<ArcaneMasterPlayer>().ArcaneMaster = true;
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		ArcaneMasterPlayer modplayer = player.GetModPlayer<ArcaneMasterPlayer>();
		if (modplayer.ArcaneMaster && item.DamageType == DamageClass.Magic) {
			float addition = (StackAmount(player) - 1) * .005f;
			damage += modplayer.ManaCostIncreases * .01f + addition;
		}
	}
	class ArcaneMasterPlayer : ModPlayer {
		public bool ArcaneMaster = false;
		public int ManaCostIncreases = 0;
		public int Decay_ManaCostIncreases = 0;
		public override void ResetEffects() {
			ArcaneMaster = false;
			if (ManaCostIncreases <= 0 || Player.ItemAnimationActive) return;
			if (++Decay_ManaCostIncreases >= 10) {
				ManaCostIncreases--;
				Decay_ManaCostIncreases = 0;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (ArcaneMaster && item.DamageType == DamageClass.Magic) damage += ManaCostIncreases * .01f;
		}
		public override void ModifyManaCost(Item item, ref float reduce, ref float mult) {
			if (ArcaneMaster) mult += ManaCostIncreases * .02f;
		}
		public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (ArcaneMaster) ManaCostIncreases = Math.Clamp(ManaCostIncreases + 1, 0, 60);
			return base.Shoot(item, source, position, velocity, type, damage, knockback);
		}
	}
}
