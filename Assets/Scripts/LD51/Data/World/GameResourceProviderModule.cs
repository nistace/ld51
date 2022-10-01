using LD51.Data.Tensies;
using LD51.Data.World;
using UnityEngine;

namespace LD51.Data.GameResources {
	public class GameResourceProviderModule : MonoBehaviour, IWorldObjectModule, ITensieInteractable {
		[SerializeField] protected WorldObject     _worldObject;
		[SerializeField] protected GameResource    _resource;
		[SerializeField] protected int             _amount            = 10;
		[SerializeField] protected float           _harvestTime       = .5f;
		[SerializeField] protected TensieAnimation _tensieUpAnimation = TensieAnimation.WalkUp;

		public WorldObject worldObject => _worldObject ? _worldObject : _worldObject = GetComponent<WorldObject>();

		private void Reset() => _worldObject = worldObject;

		public void ContinueInteraction(Tensie actor, ref float progress) {
			progress += Time.deltaTime;
			if (progress > _harvestTime) {
				actor.inventory.Add(_resource, 1);
				_amount--;
				progress %= _harvestTime;
			}
		}

		public void SetHovered(bool hovered) => _worldObject.SetHovered(hovered);
		public TensieAnimation GetTensieUpAnimation() => _tensieUpAnimation;
		public Sprite GetActionIcon() => SpriteAtlasLibrary.resourcesAtlas[_resource.layerName]["black_outline"][0];
		public float GetRequiredTime() => _harvestTime;
	}
}