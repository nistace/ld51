using LD51.Data.Tensies;
using LD51.Data.World;
using UnityEngine;

namespace LD51.Data.GameResources {
	public class GameResourceStorageModule : MonoBehaviour, IWorldObjectModule, ITensieInteractable {
		[SerializeField] protected WorldObject     _worldObject;
		[SerializeField] protected GameResource[]  _resources;
		[SerializeField] protected float           _storeTime         = .5f;
		[SerializeField] protected TensieAnimation _tensieUpAnimation = TensieAnimation.WalkUp;
		[SerializeField] protected Sprite          _actionIcon;

		public WorldObject worldObject => _worldObject ? _worldObject : _worldObject = GetComponent<WorldObject>();

		private void Reset() => _worldObject = worldObject;

		public void ContinueInteraction(Tensie actor, ref float progress) {
			if (!actor.inventory.ContainsAny(_resources)) return;
			progress += Time.deltaTime;
			if (progress >= _storeTime) {
				foreach (var resource in _resources) {
					GameInventory.inventory.Add(resource, actor.inventory.Count(resource));
					actor.inventory.TakeAll(resource);
				}
			}
		}

		public void SetHovered(bool hovered) => _worldObject.SetHovered(hovered);
		public TensieAnimation GetTensieUpAnimation() => _tensieUpAnimation;
		public Sprite GetActionIcon() => _actionIcon;
		public float GetRequiredTime() => _storeTime;
	}
}