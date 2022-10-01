using System;
using LD51.Data.Tensies;
using UnityEngine;
using Utils.Extensions;

namespace LD51.Data.World {
	[RequireComponent(typeof(WorldObject))]
	public class TensieSpawnerModule : MonoBehaviour, IWorldObjectModule {
		[SerializeField] protected WorldObject _worldObject;
		[SerializeField] protected Tensie      _tensiePrefab;
		[SerializeField] protected Transform   _spawn;

		public Tensie      tensiePrefab   => _tensiePrefab;
		public Vector3     spawnPosition  => _spawn.position;
		public WorldObject worldObject    => _worldObject ? _worldObject : _worldObject = GetComponent<WorldObject>();
		public Tensie      assignedTensie { get; set; }

		private void Reset() => _worldObject = worldObject;

		private void Start() {
			worldObject.onHoverChanged.AddListenerOnce(HandleHoverChanged);
		}

		private void HandleHoverChanged(bool hover) => assignedTensie?.SetHovered(hover);
	}
}