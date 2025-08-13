using Roguelike.Common.Systems.ArtifactSystem;
using Roguelike.Texture;

namespace Roguelike.Contents.Transfixion.Artifacts
{
	internal class RandomArtifact : Artifact {
		public override string TexturePath => ModTexture.Get_MissingTexture("Artifact");
	}
}
