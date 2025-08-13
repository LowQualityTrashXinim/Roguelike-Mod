 
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;

namespace Roguelike.Contents.Items.Consumable.SpecialReward
{
	internal class KSNoHitReward : BaseNoHit {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.KingSlimePetItem);
	}
	internal class EoCNoHitReward : BaseNoHit {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.EyeOfCthulhuPetItem);
	}
	internal class EoWNoHitReward : BaseNoHit {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.WormFood);
	}
	internal class BoCNoHitReward : BaseNoHit {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.BrainOfCthulhuPetItem);
	}
	internal class SkeletronNoHitReward : BaseNoHit {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.SkeletronPetItem);
	}
	internal class QueenBeeNoHitReward : BaseNoHit {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.QueenBeePetItem);
	}
	internal class DeerclopNoHitReward : BaseNoHit {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.DeerclopsPetItem);
	}
	internal class WallOfFleshNoHitReward : BaseNoHit {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.WallOfFleshGoatMountItem);
	}
}
