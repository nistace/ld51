using LD51.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Extensions;

namespace LD51.Data.Tensies {
	[RequireComponent(typeof(Tensie))]
	public class InputTensieController : MonoBehaviour, ITensieController {
		private new Rigidbody2D               rigidbody    { get; set; }
		private     Tensie                    tensie       { get; set; }
		private     TensieActionData.KeyFrame lastKeyFrame { get; set; }
		private     bool                      interacting  { get; set; }

		private void OnEnable() {
			tensie = GetComponent<Tensie>();
			rigidbody = tensie.GetComponent<Rigidbody2D>();
			GameInput.controls.Tensie.Enable();
			GameInput.controls.Tensie.Interact.AddAnyListenerOnce(HandelInteractInput);
			tensie.actionData.Clear();
			rigidbody.bodyType = RigidbodyType2D.Dynamic;
			lastKeyFrame = new TensieActionData.KeyFrame { direction = Direction.Down, interacting = false, position = transform.position };
			tensie.interactor.SetShowHoversEnabled(true);
		}

		private void HandelInteractInput(InputAction.CallbackContext obj) => interacting = obj.performed;

		private void OnDisable() {
			GameInput.controls.Tensie.Disable();
			GameInput.controls.Tensie.Interact.RemoveAnyListener(HandelInteractInput);
			tensie.interactor.SetShowHoversEnabled(false);
		}

		public TensieActionData.KeyFrame GetKeyFrame() => lastKeyFrame;

		private void Update() {
			var movement = interacting ? Vector3.zero : (Vector3)GameInput.controls.Tensie.Move.ReadValue<Vector2>();
			lastKeyFrame = new TensieActionData.KeyFrame {
				position = transform.position, direction = GetDirection(movement), interacting = interacting, moving = Mathf.Max(Mathf.Abs(movement.x), Mathf.Abs(movement.y)) > .1f
			};
			tensie.actionData.Write(lastKeyFrame);
			rigidbody.velocity = movement == Vector3.zero ? Vector3.zero : movement * tensie.data.movementSpeed;
		}

		private Direction GetDirection(Vector3 movement) {
			if (interacting) return lastKeyFrame.direction;
			if (movement.x > .1f) return Direction.Right;
			if (movement.x < -.1f) return Direction.Left;
			if (movement.y > .1f) return Direction.Up;
			if (movement.y < -.1f) return Direction.Down;
			return lastKeyFrame.direction;
		}

		public void RemoveComponent() => Destroy(this);
	}
}