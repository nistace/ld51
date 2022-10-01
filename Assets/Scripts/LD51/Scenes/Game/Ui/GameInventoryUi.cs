using System;
using System.Collections.Generic;
using LD51.Data.GameResources;
using UnityEngine;

namespace LD51.Game.Ui {
	public class GameInventoryUi : MonoBehaviour {
		[SerializeField] protected GameInventoryResourceUi _template;

		private List<GameInventoryResourceUi> _items { get; } = new List<GameInventoryResourceUi>();

		public void Initialize() {
			_items.Clear();
			_items.Add(_template);
			while (_items.Count < GameInventory.resourceOrder.Length) {
				_items.Add(Instantiate(_template, _template.transform.parent));
			}
			for (var i = 0; i < GameInventory.resourceOrder.Length; ++i) {
				_items[i].SetResource(GameInventory.resourceOrder[i]);
			}
		}
	}
}