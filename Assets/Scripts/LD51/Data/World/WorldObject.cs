using LD51.Data.Misc;
using LD51.Data.World.Ui;
using UnityEngine;
using UnityEngine.Events;
using Utils.Events;

namespace LD51.Data.World {
	public class WorldObject : MonoBehaviour {
		public class Event : UnityEvent<WorldObject> { }

		public static bool hoverWithMouse { get; set; }

		[SerializeField] protected WorldObjectInfoUi _infoUi;
		[SerializeField] protected string            _layerName;
		[SerializeField] protected SpriteRenderer    _renderer;
		[SerializeField] protected bool              _hovered;
		[SerializeField] protected bool              _selected;

		public IWorldObjectVisualHandler visualHandler { get; set; }
		public IWorldObjectInfoSource    infoSource    { get; set; }

		public        BoolEvent onHoverChanged { get; } = new BoolEvent();
		public static Event     onClicked      { get; } = new Event();

		private void RefreshVisuals() => _renderer.sprite = SpriteAtlasLibrary.buildings[_layerName][GetVisualTag()][GameTime.animationFrame];

		private string GetVisualTag() {
			var baseTag = visualHandler?.GetOverridenTag() ?? "default";
			if (_selected) return $"{baseTag}-blue";
			if (_hovered) return $"{baseTag}-white";
			return baseTag;
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

		public void SetSelected(bool selected) => _selected = selected;

		public void SetHovered(bool hovered) {
			_hovered = hovered;
			onHoverChanged.Invoke(_hovered);
		}

		private void Update() {
			RefreshVisuals();
			RefreshInfoUi();
		}

		private void RefreshInfoUi() {
			_infoUi.SetVisible(_hovered);
			if (!_hovered) return;
			_infoUi.actionImage.Show(infoSource?.GetInfoActionSprite());
			_infoUi.resourceSet.Refresh(infoSource?.GetResourceSet(), infoSource?.IsInfoResourceSetAmountRelevant() ?? false);
		}
	}
}