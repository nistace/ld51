using LD51.Game.Ui.Constructions;
using UnityEngine;

namespace LD51.Game.Ui {
	public class GameUi : MonoBehaviour {
		private static GameUi instance { get; set; }

		[SerializeField] protected GameInventoryUi     _inventoryUi;
		[SerializeField] protected ConstructionPanelUi _constructionPanel;

		private static GameInventoryUi     inventoryUi       => instance._inventoryUi;
		public static  ConstructionPanelUi constructionPanel => instance._constructionPanel;

		private void Awake() {
			instance = this;
		}

		public static void Initialize() {
			inventoryUi.Initialize();
			constructionPanel.Initialize();
		}
	}
}