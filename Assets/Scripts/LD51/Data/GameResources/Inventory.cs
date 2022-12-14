using System;
using System.Collections.Generic;
using System.Linq;

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
			if (content[resource] == 0) content.Remove(resource);
			return true;
		}

		public void TakeAll(GameResource resource) {
			if (!content.ContainsKey(resource)) return;
			content.Remove(resource);
		}

		public IEnumerable<GameResource> GetResourcesInInventory() => content.Keys;

		public int Count(GameResource resource) => content.ContainsKey(resource) ? content[resource] : 0;

		public bool ContainsAny(IEnumerable<GameResource> resources) => resources.Any(t => Count(t) > 0);
		public bool ContainsAny(GameResource resource) => Count(resource) > 0;
		public void Clear() => content.Clear();

		public bool IsEmpty() => content.Count == 0;
	}
}