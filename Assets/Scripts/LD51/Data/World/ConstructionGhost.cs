using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Extensions;

namespace LD51.Data.World {
	[RequireComponent(typeof(WorldObject))]
	public class ConstructionGhost : MonoBehaviour {
		[SerializeField] protected WorldObject    _worldObject;
		[SerializeField] protected SpriteRenderer _renderer;

		public      WorldObject    worldObject => _worldObject ? _worldObject : _worldObject = GetComponent<WorldObject>();
		private new SpriteRenderer renderer    => _renderer ? _renderer : _renderer = GetComponentInChildren<SpriteRenderer>();

		private List<ConstructionGhostTrigger> colliders     { get; } = new List<ConstructionGhostTrigger>();
		public  bool                           hasCollisions => colliders.Any(t => t.hasCollisions);

		public Vector2 centerOffset { get; private set; }

		public void Start() {
			colliders.Clear();
			colliders.AddRange(GetComponentsInChildren<ConstructionGhostTrigger>(true));
			colliders.ForEach(t => t.Enable());
			renderer.sortingOrder = 10000;
			var thisCollider = GetComponent<Collider2D>();
			thisCollider.enabled = false;
			centerOffset = -thisCollider.offset;
		}

		private void Update() => renderer.color = (hasCollisions ? Color.red : Color.green).With(a: .5f);
	}
}