using System.Collections;
using UnityEngine;
using Utils.Coroutines;

namespace LD51.Data {
	[CreateAssetMenu]
	public class SpriteAtlasLibrary : ScriptableObject {
		public static SpriteAtlasLibrary instance { get; set; }

		[SerializeField] protected SpriteAtlas _groundAtlas;
		[SerializeField] protected SpriteAtlas _buildingAtlas;
		[SerializeField] protected SpriteAtlas _characterAtlas;
		[SerializeField] protected SpriteAtlas _resourcesAtlas;
		[SerializeField] protected SpriteAtlas _portraitsAtlas;

		public static SpriteAtlas grounds        => instance._groundAtlas;
		public static SpriteAtlas buildings      => instance._buildingAtlas;
		public static SpriteAtlas characters     => instance._characterAtlas;
		public static SpriteAtlas resourcesAtlas => instance._resourcesAtlas;
		public static SpriteAtlas portraits      => instance._portraitsAtlas;

		public static bool loaded { get; private set; }

		public SpriteAtlas groundAtlas => _groundAtlas;

		public IEnumerator BuildAll() {
			loaded = false;
			yield return CoroutineRunner.Run(_groundAtlas.Build());
			yield return CoroutineRunner.Run(_buildingAtlas.Build());
			yield return CoroutineRunner.Run(_characterAtlas.Build());
			yield return CoroutineRunner.Run(_resourcesAtlas.Build());
			yield return CoroutineRunner.Run(_portraitsAtlas.Build());
			loaded = true;
		}
	}
}