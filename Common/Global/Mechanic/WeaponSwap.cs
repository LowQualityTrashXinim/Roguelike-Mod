using Roguelike.Common.Global.Mechanic.OutroEffect;
using Roguelike.Contents.Items.Weapon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Roguelike.Common.Global.Mechanic;
public class WeaponSwap : ModPlayer {
	public int ItemTypeCurrent = 0;
	public Item itemOld = null;
	public int ItemTypeOld = 0;
	public bool IsTheItemInQuestionASynergyItem = false;
	public bool JustSwitched = false;
	public override void ResetEffects() {
		Item item = Player.HeldItem;
		IsTheItemInQuestionASynergyItem = item.ModItem is SynergyModItem;
		if (item.type == ItemID.None) {
			return;
		}
		JustSwitched = false;
		if (ItemTypeCurrent != item.type) {
			JustSwitched = true;
		}
		if (Player.itemAnimation == Player.itemAnimationMax) {
			if (ItemTypeCurrent != item.type) {
				ItemTypeCurrent = item.type;
				if (itemOld != null) {
					if (itemOld.TryGetGlobalItem(out GlobalItemHandle handler)) {
						Player.GetModPlayer<OutroEffect_ModPlayer>().Add_OutroEffect(handler.OutroEffect_type);
					}
				}
				itemOld = item;
			}
			ItemTypeOld = ItemTypeCurrent;
		}
	}
	public override void PostUpdate() {
		JustSwitched = false;
	}
	public bool CompareOldvsNewItemType => ItemTypeCurrent != ItemTypeOld || IsTheItemInQuestionASynergyItem;
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (!CompareOldvsNewItemType) {
			if (item.ModItem is SynergyModItem) {
				damage = damage.CombineWith(Player.GetModPlayer<PlayerStatsHandle>().SynergyDamage);
			}
			return;
		}
		damage = damage.CombineWith(Player.GetModPlayer<PlayerStatsHandle>().SynergyDamage);
	}
}
