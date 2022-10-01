using UnityEngine;
using UnityEngine.Events;
using Utils.Events;

namespace LD51.Data.World {
	public class WorldObject : MonoBehaviour {
		public class Event : UnityEvent<WorldObject> { }

		public static bool hoverWithMouse { get; set; }

		[SerializeField] protected string         _layerName;
		[SerializeField] protected SpriteRenderer _renderer;
		[SerializeField] protected bool           _hovered;
		[SerializeField] protected bool           _selected;

		public        BoolEvent onHoverChanged { get; } = new BoolEvent();
		public static Event     onClicked      { get; } = new Event();

		private void RefreshVisuals() {
			_renderer.sprite = SpriteAtlasLibrary.buildings[_layerName][_selected ? "blue-outline" : _hovered ? "white-outline" : "default"][0];
		}

		private void OnMouseEnter() {
			if (!hoverWithMouse) return;
			SetHovered(true);
		}

		private void OnMouseExit() {
			if (!hoverWithMouse) return;
			SetHovered(false);
		}

		private void OnMouseUpAsButton() => onClicked.Invoke(this);

		public void SetSelected(bool selected) {
			_selected = selected;
			RefreshVisuals();
		}

		public void SetHovered(bool hovered) {
			_hovered = hovered;
			RefreshVisuals();
			onHoverChanged.Invoke(_hovered);
		}
	}
}