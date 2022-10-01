using LD51.Data.Tensies;
using Unity.VisualScripting;
using UnityEngine;
using Utils.Extensions;

namespace LD51.Data.World {
	[RequireComponent(typeof(WorldObject))]
	public class TensieSpawnerModule : MonoBehaviour, IWorldObjectModule {
		[SerializeField] protected WorldObject      _worldObject;
		[SerializeField] protected Tensie           _tensiePrefab;
		[SerializeField] protected Transform        _spawn;
		[SerializeField] protected bool             _spawnTensieOnStart;
		[SerializeField] protected TensieActionData _initialData = new TensieActionData();

		public Tensie      tensiePrefab   => _tensiePrefab;
		public Vector3     spawnPosition  => _spawn.position;
		public WorldObject worldObject    => _worldObject ? _worldObject : _worldObject = GetComponent<WorldObject>();
		public Tensie      assignedTensie { get; set; }

		private void Reset() => _worldObject = worldObject;

		private void Start() {
			worldObject.onHoverChanged.AddListenerOnce(HandleHoverChanged);
			if (_spawnTensieOnStart) {
				assignedTensie = Instantiate(tensiePrefab, spawnPosition, Quaternion.identity, null);
				assignedTensie.SetGhost(false);
				assignedTensie.SetSelected(false);
				assignedTensie.controller = assignedTensie.AddComponent<RepeatTensieController>();
				assignedTensie.actionData.CopyFrom(_initialData);
			}
		}

		private void HandleHoverChanged(bool hover) => assignedTensie?.SetHovered(hover);
	}
}