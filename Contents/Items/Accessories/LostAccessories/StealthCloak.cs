using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Texture;
using Roguelike.Common.Global;
using Roguelike.Common.Utils;

namespace Roguelike.Contents.Items.Accessories.LostAccessories;
internal class StealthCloak : ModItem {
	public override string Texture => ModTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.Set_LostAccessory(32, 32);
		Item.Set_InfoItem();
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<StealthCloakPlayer>().StealthCloak = true;
		if (player.invis) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, 1.35f);
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MovementSpeed, 1.15f);
		}
	}
}
class StealthCloakPlayer : ModPlayer {
	public bool StealthCloak = false;
	int InvisCooldown = 0;
	public override void ResetEffects() {
		StealthCloak = false;
	}
	public override void UpdateEquips() {
		if (StealthCloak && (--InvisCooldown <= 0 || Player.HasBuff(BuffID.Invisibility))) {
			Player.AddBuff(BuffID.Invisibility, 60);
			InvisCooldown = ModUtils.ToSecond(15);
		}
	}
	public override bool FreeDodge(Player.HurtInfo info) {
		if (!Player.immune && StealthCloak && Player.HasBuff(BuffID.Invisibility) && Main.rand.NextBool(15)) {
			Player.AddImmuneTime(info.CooldownCounter, 60);
			Player.immune = true;
			return true;
		}
		return base.FreeDodge(info);
	}
}
