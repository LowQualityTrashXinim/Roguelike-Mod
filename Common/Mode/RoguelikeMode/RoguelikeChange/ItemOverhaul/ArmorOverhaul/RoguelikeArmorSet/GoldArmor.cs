﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Mode.RoguelikeMode.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class GoldArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.GoldHelmet;
		bodyID = ItemID.GoldChainmail;
		legID = ItemID.GoldGreaves;
	}
}
public class GoldHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.GoldHelmet;
		Add_Defense = 2;
		TypeEquipment = Type_Head;
		ArmorName = "GoldArmor";
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<RoguelikeArmorPlayer>().MidasChance += .05f;
	}
}
public class GoldChainmail : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.GoldChainmail;
		Add_Defense = 3;
		TypeEquipment = Type_Body;
		ArmorName = "GoldArmor";
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<RoguelikeArmorPlayer>().MidasChance += .05f;
	}
}
public class GoldGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.GoldGreaves;
		Add_Defense = 2;
		TypeEquipment = Type_Leg;
		ArmorName = "GoldArmor";
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<RoguelikeArmorPlayer>().MidasChance += .05f;
	}
}
public class GoldArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("GoldArmor", this);
	}
	public override void Armor_ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (npc.HasBuff(BuffID.Midas)) {
			modifiers.SetMaxDamage(1);
			modifiers.SourceDamage.Base += npc.defense;
		}
	}
	public override void Armor_OnHitByNPC(NPC target, Player.HurtInfo hurtInfo) {
		if (target.HasBuff(BuffID.Midas)) {
			Player.Heal(5);
			target.DelBuff(target.FindBuffIndex(BuffID.Midas));
		}
	}
}
