using LD51.Data.Misc;
using LD51.Data.World;
using LD51.Game.Ui;
using LD51.Game.Ui.Constructions;
using Utils.Extensions;

namespace LD51.Game {
	public class OverviewGameState : AbstractStateBehaviour {
		public static OverviewGameState state { get; } = new OverviewGameState();

		private OverviewGameState() { }

		protected override void EnableState() {
			base.EnableState();
			WorldObjectManager.SetHoverWithMouseEnabled(true);
			GameUi.constructionPanel.SetInteractable(true);
			GameCamera.main.SetTarget(null);
		}

		protected override void DisableState() {
			base.DisableState();
			GameUi.constructionPanel.SetInteractable(false);
		}

		protected override void SetListenersEnabled(bool enabled) {
			WorldObject.onClicked.SetListenerActive(HandleBuildingClicked, enabled);
			GameCamera.main.SetControlsEnabled(enabled);
			ConstructionItemUi.onAnyClick.SetListenerActive(HandleConstructionSelected, enabled);
		}

		private static void HandleConstructionSelected(ConstructionSiteModule prefab) {
			PlaceConstructionState.state.PrepareState(prefab);
			ChangeState(PlaceConstructionState.state);
		}

		private static void HandleBuildingClicked(WorldObject worldObject) {
			if (!worldObject.TryGetComponent<TensieSpawnerModule>(out var tensieSpawner)) return;
			CountdownBeforeCharacterState.state.PrepareState(tensieSpawner);
			ChangeState(CountdownBeforeCharacterState.state);
		}
	}
}