using System.Collections;
using LD51.Data.Misc;
using LD51.Data.World;
using LD51.Inputs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utils.Coroutines;
using Utils.Extensions;

namespace LD51.Game {
	public class PlaceConstructionState : AbstractStateBehaviour {
		public static PlaceConstructionState state  { get; } = new PlaceConstructionState();
		private       ConstructionSiteModule prefab { get; set; }
		private       ConstructionGhost      ghost  { get; set; }

		private PlaceConstructionState() { }

		public void PrepareState(ConstructionSiteModule prefab) {
			this.prefab = prefab;
			ghost = Object.Instantiate(prefab).MakeGhost();
		}

		protected override void EnableState() {
			base.EnableState();
			WorldObjectManager.SetHoverWithMouseEnabled(true);
			CoroutineRunner.Run(DoBuildingPlacement());
		}

		private IEnumerator DoBuildingPlacement() {
			var placedOrCancelled = false;
			while (!placedOrCancelled) {
				RefreshGhostPosition();
				if (!EventSystem.current.IsPointerOverGameObject() && GameInput.controls.Construction.Place.WasPerformedThisFrame()) {
					placedOrCancelled = TryPlaceConstruction();
				}
				else if (GameInput.controls.Construction.Cancel.WasPerformedThisFrame()) {
					DestroyGhost();
					placedOrCancelled = true;
				}
				yield return null;
			}
			ChangeState(OverviewGameState.state);
		}

		private void RefreshGhostPosition() {
			var aimPosition = (Vector2)GameCamera.main.camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) + ghost.centerOffset;
			var positionInGrid = WorldRenderer.FixObjectPosition(ghost.worldObject, aimPosition);
			ghost.transform.position = positionInGrid;
		}

		private void DestroyGhost() {
			Object.Destroy(ghost.gameObject);
			ghost = null;
		}

		private bool TryPlaceConstruction() {
			if (ghost.hasCollisions) return false;
			Object.Instantiate(prefab, ghost.transform.position, Quaternion.identity);
			DestroyGhost();
			return true;
		}

		protected override void SetListenersEnabled(bool enabled) {
			GameCamera.main.SetControlsEnabled(enabled);
			GameInput.controls.Construction.Aim.SetEnabled(enabled);
			GameInput.controls.Construction.Place.SetEnabled(enabled);
			GameInput.controls.Construction.Cancel.SetEnabled(enabled);
		}
	}
}