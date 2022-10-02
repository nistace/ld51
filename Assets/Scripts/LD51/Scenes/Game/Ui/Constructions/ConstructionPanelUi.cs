using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace LD51.Game.Ui.Constructions {
	public class ConstructionPanelUi : MonoBehaviour {
		[SerializeField] protected Button               _toggleButton;
		[SerializeField] protected bool                 _expanded;
		[SerializeField] protected ConstructionItemUi[] _items;

		private void Start() {
			_toggleButton.onClick.AddListenerOnce(ToggleContent);
			ConstructionItemUi.onAnyClick.AddListenerOnce(_ => Hide());
			Hide();
		}

		public void Initialize() => _items.ForEach(t => t.Init());

		private void Hide() {
			_expanded = false;
			Refresh();
		}

		private void Refresh() => _items.ForEach(t => t.gameObject.SetActive(_expanded));

		private void ToggleContent() {
			_expanded = !_expanded;
			Refresh();
		}

		public void SetInteractable(bool interactable) {
			_toggleButton.interactable = interactable;
			if (!interactable) Hide();
		}
	}
}