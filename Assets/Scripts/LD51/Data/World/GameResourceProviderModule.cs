using System.Collections.Generic;
using LD51.Data.Tensies;
using LD51.Data.World;
using UnityEngine;

namespace LD51.Data.GameResources {
	[RequireComponent(typeof(WorldObject))]
	public class GameResourceProviderModule : MonoBehaviour, IWorldObjectModule, ITensieInteractable, IWorldObjectVisualHandler, IWorldObjectInfoSource {
		[SerializeField] protected WorldObject     _worldObject;
		[SerializeField] protected Collider2D      _collider;
		[SerializeField] protected GameResource    _resource;
		[SerializeField] protected int             _initialAmount     = 10;
		[SerializeField] protected float           _harvestTime       = .5f;
		[SerializeField] protected TensieAnimation _tensieUpAnimation = TensieAnimation.WalkUp;

		private Dictionary<GameResource, int> remainingResources { get; } = new Dictionary<GameResource, int>();

		private int remainingAmount {
			get => remainingResources[_resource];
			set => remainingResources[_resource] = value;
		}

		public WorldObject worldObject => _worldObject ? _worldObject : _worldObject = GetComponent<WorldObject>();

		private void Reset() => _worldObject = worldObject;

		private void Start() {
			worldObject.visualHandler = this;
			worldObject.infoSource = this;
			remainingResources.Clear();
			remainingResources.Add(_resource, _initialAmount);
			_collider.enabled = remainingAmount > 0;
		}

		public bool IsInteractionAllowedTo(Tensie actor) => remainingAmount > 0;

		public bool ContinueInteraction(Tensie actor, ref float progress) {
			if (!IsInteractionAllowedTo(actor)) return false;
			progress += Time.deltaTime / _harvestTime;
			while (remainingAmount > 0 && progress > 1) {
				actor.inventory.Add(_resource, 1);
				remainingAmount--;
				_collider.enabled = remainingAmount > 0;
				progress--;
			}

			return true;
		}

		public void SetHovered(bool hovered) => _worldObject.SetHovered(hovered);
		public TensieAnimation GetTensieUpAnimation() => _tensieUpAnimation;
		public Sprite GetActionIcon() => SpriteAtlasLibrary.resourcesAtlas[_resource.layerName]["black_outline"][0];
		public string GetOverridenTag() => remainingAmount > 0 ? null : "alternative";
		public Sprite GetInfoActionSprite() => null;
		public bool IsInfoResourceSetAmountRelevant() => true;
		public IReadOnlyDictionary<GameResource, int> GetResourceSet() => remainingResources;
	}
}