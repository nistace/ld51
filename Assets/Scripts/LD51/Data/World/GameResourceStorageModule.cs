using System.Collections.Generic;
using System.Linq;
using LD51.Data.Tensies;
using LD51.Data.World;
using UnityEngine;
using Utils.Extensions;

namespace LD51.Data.GameResources {
	public class GameResourceStorageModule : MonoBehaviour, IWorldObjectModule, ITensieInteractable, IWorldObjectInfoSource {
		[SerializeField] protected WorldObject     _worldObject;
		[SerializeField] protected GameResource[]  _resources;
		[SerializeField] protected float           _storeTime         = .5f;
		[SerializeField] protected TensieAnimation _tensieUpAnimation = TensieAnimation.WalkUp;
		[SerializeField] protected Sprite          _actionIcon;

		private Dictionary<GameResource, int> infoResources { get; } = new Dictionary<GameResource, int>();
		public  WorldObject                   worldObject   => _worldObject ? _worldObject : _worldObject = GetComponent<WorldObject>();

		private void Start() {
			worldObject.infoSource = this;
			infoResources.Clear();
			infoResources.SetAll(_resources.Select(t => new KeyValuePair<GameResource, int>(t, 0)));
		}

		private void Reset() => _worldObject = worldObject;

		public bool IsInteractionAllowedTo(Tensie actor) => actor.inventory.ContainsAny(_resources);

		public bool ContinueInteraction(Tensie actor, ref float progress) {
			if (!IsInteractionAllowedTo(actor)) return false;
			progress += Time.deltaTime;
			if (progress >= _storeTime) {
				foreach (var resource in _resources) {
					GameInventory.inventory.Add(resource, actor.inventory.Count(resource));
					actor.inventory.TakeAll(resource);
				}
			}
			return true;
		}

		public void SetHovered(bool hovered) => _worldObject.SetHovered(hovered);
		public TensieAnimation GetTensieUpAnimation() => _tensieUpAnimation;
		public Sprite GetActionIcon() => _actionIcon;
		public Sprite GetInfoActionSprite() => _actionIcon;
		public bool IsInfoResourceSetAmountRelevant() => false;
		public IReadOnlyDictionary<GameResource, int> GetResourceSet() => infoResources;
	}
}