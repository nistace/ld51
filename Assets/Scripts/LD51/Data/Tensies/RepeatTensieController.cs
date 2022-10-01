using UnityEngine;

namespace LD51.Data.Tensies {
	[RequireComponent(typeof(Tensie))]
	public class RepeatTensieController : MonoBehaviour, ITensieController {
		private new Rigidbody2D               rigidbody { get; set; }
		private     Tensie                    tensie    { get; set; }
		private     TensieActionData.KeyFrame frame     { get; set; }

		private void OnEnable() {
			tensie = GetComponent<Tensie>();
			rigidbody = tensie.GetComponent<Rigidbody2D>();
			rigidbody.bodyType = RigidbodyType2D.Kinematic;
			rigidbody.velocity = Vector2.zero;
			frame = tensie.actionData.Read();
			GetKeyFrame();
		}

		public void RemoveComponent() => Destroy(this);

		public TensieActionData.KeyFrame GetKeyFrame() => frame;

		private void Update() {
			frame = tensie.actionData.Read();
			transform.position = frame.position;
		}
	}
}