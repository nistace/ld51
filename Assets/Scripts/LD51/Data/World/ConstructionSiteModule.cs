using System.Collections.Generic;
using System.Linq;
using LD51.Data.GameResources;
using LD51.Data.Tensies;
using UnityEngine;
using UnityEngine.Events;
using Utils.Extensions;

namespace LD51.Data.World {
	[RequireComponent(typeof(WorldObject))]
	public class ConstructionSiteModule : MonoBehaviour, ITensieInteractable, IWorldObjectVisualHandler, IWorldObjectInfoSource {
		public class Event : UnityEvent<ConstructionSiteModule> { }

		[SerializeField] protected WorldObject          _worldObject;
		[SerializeField] protected GameResourceAmount[] _requiredResources;
		[SerializeField] protected float                _timeToAddOneResource = .5f;
		[SerializeField] protected WorldObject          _resultPrefab;
		[SerializeField] protected Sprite               _actionIcon;

		private ConstructionGhost                 ghost             { get; set; }
		public  WorldObject                       worldObject       => _worldObject ? _worldObject : _worldObject = GetComponent<WorldObject>();
		public  WorldObject                       resultPrefab      => _resultPrefab;
		public  IReadOnlyList<GameResourceAmount> requiredResources => _requiredResources;
		private Dictionary<GameResource, int>     missingResources  { get; } = new Dictionary<GameResource, int>();

		public static Event onConstructionComplete { get; } = new Event();

		private void Reset() => _worldObject = worldObject;

		public void Start() {
			worldObject.visualHandler = this;
			worldObject.infoSource = this;
			missingResources.SetAll(_requiredResources.Where(t => t.amount > 0).Select(t => new KeyValuePair<GameResource, int>(t.resource, t.amount)));
		}

		private bool TryCanInteract(out GameResource resource) {
			resource = missingResources.Keys.FirstOrDefault(GameInventory.inventory.ContainsAny);
			return resource;
		}

		public bool IsInteractionAllowedTo(Tensie actor) => TryCanInteract(out _);

		public bool ContinueInteraction(Tensie actor, ref float progress) {
			if (!TryCanInteract(out var resource)) return false;

			progress += Time.deltaTime / _timeToAddOneResource;
			while (progress > 1) {
				GameInventory.inventory.Take(resource, 1);
				missingResources[resource]--;
				if (missingResources[resource] == 0) {
					missingResources.Remove(resource);
					if (missingResources.Count == 0) onConstructionComplete.Invoke(this);
				}
				progress--;
			}
			return true;
		}

		public void SetHovered(bool hovered) => worldObject.SetHovered(hovered);

		public TensieAnimation GetTensieUpAnimation() => TensieAnimation.KickUp;

		public Sprite GetActionIcon() => _actionIcon;
		public string GetOverridenTag() => ghost ? null : "construction";
		public Sprite GetInfoActionSprite() => _actionIcon;
		public bool IsInfoResourceSetAmountRelevant() => true;
		public IReadOnlyDictionary<GameResource, int> GetResourceSet() => missingResources;

		public ConstructionGhost MakeGhost() {
			ghost = this.GetOrAddComponent<ConstructionGhost>();
			return ghost;
		}
	}
}