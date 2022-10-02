using System;
using System.Collections.Generic;
using UnityEngine;

namespace LD51.Data.World {
	[RequireComponent(typeof(Collider2D))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class ConstructionGhostTrigger : MonoBehaviour {
		private HashSet<Collider2D> colliders     { get; } = new HashSet<Collider2D>();
		public  bool                hasCollisions => colliders.Count > 0;

		public void Enable() {
			gameObject.SetActive(true);
			GetComponent<Rigidbody2D>().simulated = true;
		}

		private void OnTriggerEnter2D(Collider2D col) {
			colliders.Add(col);
		}

		private void OnTriggerExit2D(Collider2D other) {
			colliders.Remove(other);
		}
	}
}