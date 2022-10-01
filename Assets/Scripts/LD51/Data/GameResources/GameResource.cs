using UnityEngine;
using Utils.Extensions;

namespace LD51.Data.GameResources {
	[CreateAssetMenu]
	public class GameResource : ScriptableObject {
		[SerializeField] protected bool _food;
		[SerializeField] protected bool _constructionMaterial;

		public bool   food                 => _food;
		public bool   constructionMaterial => _constructionMaterial;
		public string layerName            => name.CamelCase();
	}
}