using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Common.RoguelikeMode;
using Roguelike.Common.Systems;
using Roguelike.Common.Systems.SpoilSystem;
using Roguelike.Common.Utils;
using Roguelike.Contents.Transfixion.Perks;
using Roguelike.Texture;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Roguelike.Contents.Items.Consumable;
internal class TheGear : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
		Item.Set_InfoItem();
	}
	int delayBetweenSwitch = 0;
	int CurrentItemTexture = 0;
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		string text = "";
		if (RogueLikeWorldGen.BiomeZone[Bid.ShrineOfOffering].Where(re => re.Contains(Main.LocalPlayer.position.ToTileCoordinates())).Any()) {
			text = "Made an offer and you shall receive";
		}
		if (RogueLikeWorldGen.BiomeZone[Bid.Dungeon].Where(re => re.Contains(Main.LocalPlayer.position.ToTileCoordinates())).Any()) {
			text = "Powerful curses present here. Best to leave soon";
		}
		if (NPC.downedMoonlord) {
			text = "There are more to be discover...";
		}
		if (text == "") {
			return;
		}
		foreach (TooltipLine line in tooltips) {
			if (line.Name == "Tooltip0") {
				line.Text = text;
			}
		}
	}
	public override bool? UseItem(Player player) {
		if (!player.dead && player.ItemAnimationJustStarted) {
			if (RogueLikeWorldGen.Check_PlayerPositionZone(player, Bid.ShrineOfOffering)) {
				player.GetModPlayer<SpoilsPlayer>().SpoilsGift = ModSpoilSystem.GetSpoilsList().Where(s => s.RareValue == 999).Select(x => x.Name).ToList();
				ModContent.GetInstance<UniversalSystem>().ActivateSpoilsUI();
				return true;
			}
			if (RogueLikeWorldGen.Check_PlayerPositionZone(player, Bid.Dungeon)) {
				for (int i = 0; i < 10; i++) {
					Point point = (player.Center + Main.rand.NextVector2CircularEdge(1000, 1000)).ToPoint();
					NPC.NewNPC(NPC.GetSource_NaturalSpawn(), point.X, point.Y, NPCID.DungeonGuardian);
				}
			}
			else {
				KillPlayer(player);
			}
		}
		return true;
	}
	private void KillPlayer(Player player) {
		string whoAmI = "\"True\" God";
		if (NPC.downedMoonlord) {
			whoAmI = "False God";
		}
		player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromLiteral($"{player.name} fail to confront {whoAmI}")), 9999999999, 1);
	}
	public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
		int length = TextureAssets.Item.Length;
		if (length > 0) {
			int itemToLoad;
			if (delayBetweenSwitch <= 0) {
				itemToLoad = Main.rand.Next(length);
				CurrentItemTexture = itemToLoad;
				delayBetweenSwitch = 7;
			}
			else {
				itemToLoad = CurrentItemTexture;
				delayBetweenSwitch--;
			}
			Main.instance.LoadItem(itemToLoad);
			Texture2D texture = TextureAssets.Item[itemToLoad].Value;
			spriteBatch.Draw(texture, position, null, Color.White, 0, texture.Size() * .5f, scale, SpriteEffects.None, 0);
			return false;
		}
		return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
	}
}
