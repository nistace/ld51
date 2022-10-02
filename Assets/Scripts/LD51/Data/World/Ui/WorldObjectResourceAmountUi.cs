using System.Collections.Generic;
using LD51.Data.GameResources;
using UnityEngine;

namespace LD51.Data.Tensies {
	public class WorldObjectResourceAmountUi : MonoBehaviour {
		[SerializeField] private WorldObjectResourceAmountLineUi _itemTemplate;

		private List<WorldObjectResourceAmountLineUi> items { get; } = new List<WorldObjectResourceAmountLineUi>();

		private void Start() {
			items.Add(_itemTemplate);
			_itemTemplate.Hide();
		}

		public void Refresh(IReadOnlyDictionary<GameResource, int> resourceAmounts, bool displayAmounts) {
			if (resourceAmounts == null || resourceAmounts.Count == 0) {
				gameObject.SetActive(false);
			}
			else {
				var index = 0;
				foreach (var resource in GameInventory.resourceOrder) {
					if (resourceAmounts.ContainsKey(resource)) {
						while (items.Count <= index) items.Add(Instantiate(_itemTemplate, _itemTemplate.transform.parent));
						items[index].Show(SpriteAtlasLibrary.resourcesAtlas[resource.layerName]["black_outline"][0], displayAmounts ? resourceAmounts[resource] : null);
						index++;
					}
				}
				while (index < items.Count) items[index++].Hide();
				gameObject.SetActive(true);
			}
		}
	}
}