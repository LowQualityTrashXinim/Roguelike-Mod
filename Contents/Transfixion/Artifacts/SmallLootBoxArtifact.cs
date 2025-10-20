using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Common.Utils;
using Roguelike.Contents.Items.Lootbox;
using Roguelike.Contents.Items.RelicItem;
using Roguelike.Contents.Perks;
using Roguelike.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Roguelike.Contents.Transfixion.Artifacts;

public class SmallLootBoxArtifact : Artifact {
	public override Color DisplayNameColor => Color.Brown;
	public override string TexturePath => ModTexture.Get_MissingTexture("Artifact");
	public override IEnumerable<Item> AddStartingItems(Player player) {
		yield return new Item(ModContent.ItemType<WorldEssence>());
		yield return new Item(ModContent.ItemType<SkillLootBox>());
		Item item = new Item(ModContent.ItemType<Relic>());
		if (item.ModItem is Relic relic) {
			relic.AutoAddRelicTemplate(player, 4);
		}
		yield return item;
	}
}
public class SmallLootboxArtifactPlayer : ModPlayer {
	bool SmallLootbox = false;
	public override void ResetEffects() {
		SmallLootbox = Player.HasArtifact<SmallLootBoxArtifact>();
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (SmallLootbox) {
			damage -= .2f;
		}
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (SmallLootbox) {
			modifiers.FinalDamage += .35f;
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (SmallLootbox) {
			modifiers.FinalDamage += .35f;
		}
	}
	public override void UpdateEquips() {
		if (SmallLootbox)
			Player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.LootDropIncrease, Base: 1);
	}
}
