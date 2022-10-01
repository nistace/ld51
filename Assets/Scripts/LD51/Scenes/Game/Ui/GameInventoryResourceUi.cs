using LD51.Data;
using LD51.Data.GameResources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD51.Game.Ui {
	public class GameInventoryResourceUi : MonoBehaviour {
		[SerializeField] protected GameResource _gameResource;
		[SerializeField] protected Image        _image;
		[SerializeField] protected TMP_Text     _amountText;

		public void SetResource(GameResource gameResource) {
			_gameResource = gameResource;
			_image.sprite = SpriteAtlasLibrary.resourcesAtlas[gameResource.layerName]["white_outline"][0];
		}

		private string GetAmountText() {
			if (!_gameResource) return string.Empty;
			if (_gameResource.food) {
				return $"{GameInventory.inventory.Count(_gameResource)} <#800>(-{GameInventory.foodConsumption} / turn)";
			}
			return $"{GameInventory.inventory.Count(_gameResource)}";
		}

		private void Update() => _amountText.text = GetAmountText();
	}
}