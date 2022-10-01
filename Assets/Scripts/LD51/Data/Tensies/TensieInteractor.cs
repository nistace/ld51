using UnityEngine;
using Utils.Extensions;

namespace LD51.Data.Tensies {
	public class TensieInteractor : MonoBehaviour {
		[SerializeField] protected GameObject _currentInteractableGameObject;
		[SerializeField] protected float      _progress;
		[SerializeField] protected bool       _showsHover;
		[SerializeField] protected Vector3    _directionOffset = new Vector3(0, .5f, 0);
		[SerializeField] protected Vector3    _upOffset        = new Vector3(0, .5f, 0);

		private ITensieInteractable interactable { get; set; }

		public void OnTriggerEnter2D(Collider2D col) {
			if (_currentInteractableGameObject == col.gameObject) return;
			interactable?.SetHovered(false);
			_currentInteractableGameObject = col.gameObject;
			interactable = _currentInteractableGameObject.GetComponent<ITensieInteractable>();
			interactable?.SetHovered(_showsHover);
			_progress = 0;
		}

		public void OnTriggerExit2D(Collider2D other) {
			if (other.gameObject != _currentInteractableGameObject) return;
			interactable?.SetHovered(false);
			interactable = null;
			_progress = 0;
		}

		public bool TryGetInteractable(out ITensieInteractable interactable) => (interactable = this.interactable) != null;

		public void SetShowHoversEnabled(bool enabled) {
			_showsHover = enabled;
			interactable?.SetHovered(_showsHover);
		}

		public void SetDirection(Direction direction) => transform.localPosition = _directionOffset + _upOffset.Rotate(aroundZAxis: (int)direction * 90);
	}
}