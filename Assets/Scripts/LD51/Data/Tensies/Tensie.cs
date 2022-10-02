using LD51.Data.GameResources;
using LD51.Data.Misc;
using LD51.Data.Tensies.Ui;
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

		public  ITensieController controller { get; set; }
		public  TensieActionData  actionData => _actionData;
		public  TensieSharedData  data       => _data;
		public  Inventory         inventory  => _inventory;
		public  TensieInteractor  interactor => _interactor;
		private TensieAnimation   animation  { get; set; } = TensieAnimation.Idle;

		public void SetGhost(bool ghost) {
			_ghost = ghost;
			_collider.enabled = !ghost;
		}

		public void SetHovered(bool hovered) => _hovered = hovered;
		public void SetSelected(bool selected) => _selected = selected;

		private void RefreshVisuals() {
			_renderer.color = _ghost ? _data.ghostColor : Color.white;
			_renderer.sprite = SpriteAtlasLibrary.characters[GetLayerName()][$"{animation}"][GameTime.animationFrame];
		}

		private string GetLayerName() {
			if (_selected) return $"tensie{_modelIndex}_selected";
			if (_hovered) return $"tensie{_modelIndex}_hover";
			return $"tensie{_modelIndex}";
		}

		private void Update() {
			if (GameTime.justStartedNewLoop) inventory.Clear();
			var keyFrame = controller?.GetKeyFrame();
			ITensieInteractable interactedWith = null;
			if (keyFrame != null) {
				interactor.SetDirection(keyFrame.direction);
				if (keyFrame.interacting && interactor.TryGetInteractable(out var interactable)) {
					if (interactable.ContinueInteraction(this, ref _progress)) interactedWith = interactable;
				}
			}
			if ((!keyFrame?.interacting ?? true) || interactedWith == null) _progress = 0;
			RefreshInfoUi(keyFrame?.interacting ?? false, interactedWith);
			animation = EvalAnimation(keyFrame, interactedWith);
			RefreshVisuals();
		}

		private static TensieAnimation EvalAnimation(TensieActionData.KeyFrame keyFrame, ITensieInteractable interactedWith) {
			if (keyFrame == null) return TensieAnimation.Idle;
			if (keyFrame.interacting && interactedWith == null) return TensieAnimation.Dunno;
			if (keyFrame.interacting) return GetDirectionalAnimation(interactedWith.GetTensieUpAnimation(), keyFrame.direction);
			if (keyFrame.moving) return GetDirectionalAnimation(TensieAnimation.WalkUp, keyFrame.direction);
			return TensieAnimation.Idle;
		}

		private static TensieAnimation GetDirectionalAnimation(TensieAnimation upAnimation, Direction direction) => (TensieAnimation)((int)upAnimation + (int)direction);

		private void RefreshInfoUi(bool tryingToInteract, ITensieInteractable interactedWith) {
			if (tryingToInteract && interactedWith != null) _infoUi.progress.Show(interactedWith.GetActionIcon(), _progress);
			else _infoUi.progress.Hide();
			_infoUi.SetQuestionMarkVisible(tryingToInteract && interactedWith == null);
			_infoUi.inventory.Refresh(inventory);
		}
	}
}