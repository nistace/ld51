using System.Collections.Generic;
using System.Linq;
using LD51.Data.Tensies;
using LD51.Data.World;
using UnityEngine;
using UnityEngine.Events;
using Utils.Extensions;

namespace LD51.Data.GameResources {
	public class GameResourceStorageModule : MonoBehaviour, IWorldObjectModule, ITensieInteractable, IWorldObjectInfoSource {
		public class StoringEvent : UnityEvent<GameResource, int> { }

		[SerializeField] protected WorldObject     _worldObject;
		[SerializeField] protected GameResource[]  _resources;
		[SerializeField] protected float           _storeTime         = .5f;
		[SerializeField] protected TensieAnimation _tensieUpAnimation = TensieAnimation.WalkUp;
		[SerializeField] protected Sprite          _actionIcon;

		private Dictionary<GameResource, int> infoResources { get; } = new Dictionary<GameResource, int>();
		public  WorldObject                   worldObject   => _worldObject ? _worldObject : _worldObject = GetComponent<WorldObject>();

		public static StoringEvent onStoredResources { get; } = new StoringEvent();

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
					var amount = actor.inventory.Count(resource);
					GameInventory.inventory.Add(resource, amount);
					actor.inventory.TakeAll(resource);
					onStoredResources.Invoke(resource, amount);
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

		[ContextMenu("Fake Drop 4 food")] private void FakeDrop4Food() => onStoredResources.Invoke(GameInventory.resourceOrder.First(t => t.food), 4);
		[ContextMenu("Fake Drop 1 wood")] private void FakeDrop4Wood() => onStoredResources.Invoke(GameInventory.resourceOrder.First(t => t.name == "Wood"), 1);
	}
}