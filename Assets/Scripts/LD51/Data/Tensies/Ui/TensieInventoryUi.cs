using System.Collections.Generic;
using LD51.Data.GameResources;
using UnityEngine;

namespace LD51.Data.Tensies.Ui {
	public class TensieInventoryUi : MonoBehaviour {
		[SerializeField] private TensieInventoryLineUi _itemTemplate;

		private List<TensieInventoryLineUi> items { get; } = new List<TensieInventoryLineUi>();

		private void Start() {
			items.Add(_itemTemplate);
			_itemTemplate.Hide();
		}

		public void Refresh(Inventory inventory) {
			if (inventory == null || inventory.IsEmpty()) {
				gameObject.SetActive(false);
			}
			else {
				var index = 0;
				foreach (var resource in GameInventory.resourceOrder) {
					if (inventory.ContainsAny(resource)) {
						while (items.Count <= index) items.Add(Instantiate(_itemTemplate, _itemTemplate.transform.parent));
						items[index].Show(SpriteAtlasLibrary.resourcesAtlas[resource.layerName]["black_outline"][0], inventory.Count(resource));
						index++;
					}
				}
				while (index < items.Count) items[index++].Hide();
				gameObject.SetActive(true);
			}
		}
	}
}