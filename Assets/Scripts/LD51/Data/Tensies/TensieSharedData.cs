using UnityEngine;

namespace LD51.Data.Tensies {
	[CreateAssetMenu]
	public class TensieSharedData : ScriptableObject {
		[SerializeField] protected Color _ghostColor     = new Color(1, 1, 1, .5f);
		[SerializeField] protected float _animationSpeed = 1;
		[SerializeField] protected float _movementSpeed  = 3;

		public Color ghostColor     => _ghostColor;
		public float animationSpeed => _animationSpeed;
		public float movementSpeed  => _movementSpeed;
	}
}