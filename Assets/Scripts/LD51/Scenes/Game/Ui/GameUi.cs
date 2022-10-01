using UnityEngine;

namespace LD51.Game.Ui {
	public class GameUi : MonoBehaviour {
		private static GameUi instance { get; set; }

		[SerializeField] protected GameInventoryUi _inventoryUi;

		private static GameInventoryUi inventoryUi => instance._inventoryUi;

		private void Awake() {
			instance = this;
		}

		public static void Initialize() {
			inventoryUi.Initialize();
		}
	}
}