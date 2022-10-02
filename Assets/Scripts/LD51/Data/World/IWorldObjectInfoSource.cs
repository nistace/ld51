using System.Collections.Generic;
using LD51.Data.GameResources;
using UnityEngine;

namespace LD51.Data.World {
	public interface IWorldObjectInfoSource {
		public Sprite GetInfoActionSprite();
		bool IsInfoResourceSetAmountRelevant();
		IReadOnlyDictionary<GameResource, int> GetResourceSet();
	}
}