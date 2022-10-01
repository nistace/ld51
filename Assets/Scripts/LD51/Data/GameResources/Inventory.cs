using System;
using System.Collections.Generic;

namespace LD51.Data.GameResources {
	[Serializable]
	public class Inventory {
		private Dictionary<GameResource, int> content { get; } = new Dictionary<GameResource, int>();

		public void Add(GameResource resource, int amount) {
			if (!content.ContainsKey(resource)) content.Add(resource, 0);
			content[resource] += amount;
		}

		public bool Take(GameResource resource, int amount) {
			if (amount == 0) return true;
			if (!content.ContainsKey(resource)) return false;
			if (content[resource] < amount) return false;
			content[resource] -= amount;
			return true;
		}

		public int Count(GameResource resource) => content.ContainsKey(resource) ? content[resource] : 0;
	}
}