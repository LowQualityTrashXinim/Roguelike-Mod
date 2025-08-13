using Terraria;
using Terraria.ID;
using Roguelike.Texture;
using Terraria.ModLoader;
using Roguelike.Common.Global;
using Roguelike.Contents.Items.Weapon;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Perks.WeaponUpgrade.Content;
internal class RefinedUpgrade_GlobalItem : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (!UpgradePlayer.Check_Upgrade(Main.CurrentPlayer, WeaponUpgradeID.RefinedUpgrade)) {
			return;
		}
		if (entity.axe <= 0 || entity.noMelee) {
			return;
		}
		if (entity.TryGetGlobalItem(out GlobalItemHandle glo)) {
			glo.CriticalDamage += 2f;
		}
		entity.damage += 10;

	}
	public override void HoldItem(Item item, Player player) {
		if (UpgradePlayer.Check_Upgrade(player, WeaponUpgradeID.RefinedUpgrade)) {
			if (item.axe <= 0 || item.noMelee) {
				return;
			}
		}
	}
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		if (UpgradePlayer.Check_Upgrade(player, WeaponUpgradeID.RefinedUpgrade)) {
			if (item.axe <= 0 || item.noMelee) {
				return;
			}
			if (Main.rand.NextBool(3)) {
				target.AddBuff(ModContent.BuffType<Axe_BleedDebuff>(), ModUtils.ToSecond(Main.rand.Next(3, 8)));
			}
		}
	}
}
class Axe_BleedDebuff : ModBuff {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense -= .2f;
		npc.lifeRegen -= 10;
	}
}
public class RefinedUpgrade : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		list_category.Add(PerkCategory.WeaponUpgrade);
	}
	public override void OnChoose(Player player) {
		UpgradePlayer.Add_Upgrade(player, WeaponUpgradeID.RefinedUpgrade);
		Mod.Reflesh_GlobalItem(player);
		int[] Orestaff = {
		ItemID.CopperAxe,
		ItemID.TinAxe,
		ItemID.IronAxe,
		ItemID.LeadAxe,
		ItemID.SilverAxe,
		ItemID.TungstenAxe,
		ItemID.GoldAxe,
		ItemID.PlatinumAxe
		}; 
		player.QuickSpawnItem(player.GetSource_Misc("WeaponUpgrade"), Main.rand.Next(Orestaff));
	}
}
