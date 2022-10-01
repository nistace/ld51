using LD51.Inputs;
using UnityEngine;
using Utils.Extensions;
using Utils.Types;

namespace LD51.Data.Misc {
	[RequireComponent(typeof(Camera))]
	public class GameCamera : MonoBehaviour {
		public static GameCamera main { get; private set; }

		[SerializeField]                        protected Camera     _camera;
		[SerializeField]                        protected FloatRange _xRange;
		[SerializeField]                        protected FloatRange _yRange;
		[SerializeField]                        protected float      _z                     = -10;
		[SerializeField]                        protected FloatRange _orthographicSizeRange = new FloatRange(5, 20);
		[SerializeField]                        protected Transform  _target;
		[SerializeField]                        protected float      _orthographicSizeOnTarget = 6;
		[Header("Input"), SerializeField]       protected float      _movementSpeed            = 1;
		[SerializeField]                        protected float      _scrollSpeed              = 1;
		[Header("Smooth Damp"), SerializeField] protected Vector3    _movementVelocity;
		[SerializeField]                        protected float      _movementSmoothTime = .3f;
		[SerializeField]                        protected float      _zoomVelocity;
		[SerializeField]                        protected float      _zoomSmoothTime = .9f;
		[SerializeField]                        protected int        _hasToReleaseBeforeMoving;

		private void Reset() => _camera = GetComponent<Camera>();

		private void Awake() {
			main = this;
		}

		private void MoveTo(Vector2 position) {
			transform.position = Vector3.SmoothDamp(transform.position, new Vector3(_xRange.Clamp(position.x), _yRange.Clamp(position.y), _z), ref _movementVelocity, _movementSmoothTime);
		}

		private void MoveBy(Vector2 movement) => MoveTo((Vector2)transform.position + movement);

		private void SetOrthographicSize(float orthographicSize, bool smooth) {
			_camera.orthographicSize =
				smooth ? Mathf.SmoothDamp(_camera.orthographicSize, _orthographicSizeRange.Clamp(orthographicSize), ref _zoomVelocity, _zoomSmoothTime) : _orthographicSizeRange.Clamp(orthographicSize);
		}

		public void SetTarget(Transform target) {
			_target = target;
		}

		private void LateUpdate() {
			if (_target) {
				MoveTo(_target.transform.position);
				SetOrthographicSize(_orthographicSizeOnTarget, true);
			}
			else if (_hasToReleaseBeforeMoving > 0) {
				if (GameInput.controls.Camera.Move.ReadValue<Vector2>().sqrMagnitude < .001f) _hasToReleaseBeforeMoving--;
			}
			else {
				MoveBy(_camera.orthographicSize * _movementSpeed * GameInput.controls.Camera.Move.ReadValue<Vector2>());
				SetOrthographicSize(_camera.orthographicSize + GameInput.controls.Camera.Zoom.ReadValue<Vector2>().y * _scrollSpeed * Time.deltaTime, false);
			}
		}

		public void SetControlsEnabled(bool enabled) {
			GameInput.controls.Camera.Move.SetEnabled(enabled);
			GameInput.controls.Camera.Zoom.SetEnabled(enabled);
			if (enabled) _hasToReleaseBeforeMoving = 5;
		}
	}
}