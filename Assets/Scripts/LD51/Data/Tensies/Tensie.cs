using LD51.Data.GameResources;
using LD51.Data.Misc;
using UnityEngine;

namespace LD51.Data.Tensies {
	public class Tensie : MonoBehaviour {
		[SerializeField] protected SpriteRenderer   _renderer;
		[SerializeField] protected Collider2D       _collider;
		[SerializeField] protected TensieInteractor _interactor;
		[SerializeField] protected TensieInfoUi     _infoUi;
		[SerializeField] protected int              _modelIndex;
		[SerializeField] protected bool             _ghost;
		[SerializeField] protected bool             _selected;
		[SerializeField] protected bool             _hovered;
		[SerializeField] protected TensieSharedData _data;
		[SerializeField] protected TensieActionData _actionData;
		[SerializeField] protected Inventory        _inventory = new Inventory();
		[SerializeField] protected float            _progress;

		public ITensieController controller { get; set; }
		public TensieActionData  actionData => _actionData;
		public TensieSharedData  data       => _data;
		public Inventory         inventory  => _inventory;
		public TensieInteractor  interactor => _interactor;

		public void SetGhost(bool ghost) {
			_ghost = ghost;
			_collider.enabled = !ghost;
			RefreshVisuals();
		}

		public void SetHovered(bool hovered) {
			_hovered = hovered;
			RefreshVisuals();
		}

		public void SetSelected(bool selected) {
			_selected = selected;
			RefreshVisuals();
		}

		private void RefreshVisuals() {
			_renderer.color = _ghost ? _data.ghostColor : Color.white;
			_renderer.sprite = SpriteAtlasLibrary.characters[GetLayerName()][$"{controller?.GetKeyFrame()?.animation ?? TensieAnimation.Idle}"][Mathf.FloorToInt(Time.time * _data.animationSpeed)];
		}

		private string GetLayerName() {
			if (_selected) return $"tensie{_modelIndex}_selected";
			if (_hovered) return $"tensie{_modelIndex}_hover";
			return $"tensie{_modelIndex}";
		}

		private void Update() {
			if (GameTime.justStartedNewLoop) inventory.Clear();
			var keyFrame = controller?.GetKeyFrame();
			ITensieInteractable interactable = null;
			if (keyFrame != null) {
				interactor.SetDirection(keyFrame.direction);
				if (keyFrame.interacting && interactor.TryGetInteractable(out interactable)) {
					interactable.ContinueInteraction(this, ref _progress);
				}
			}
			if ((!keyFrame?.interacting ?? true) || interactable == null) _progress = 0;
			RefreshInfoUi(keyFrame?.interacting ?? false, interactable);
			RefreshVisuals();
		}

		private void RefreshInfoUi(bool tryingToInteract, ITensieInteractable interactable) {
			if (tryingToInteract && interactable != null) _infoUi.progress.Show(interactable.GetActionIcon(), _progress / interactable.GetRequiredTime());
			else _infoUi.progress.Hide();
			_infoUi.SetQuestionMarkVisible(tryingToInteract && interactable == null);
			_infoUi.inventory.Refresh(inventory);
		}
	}
}