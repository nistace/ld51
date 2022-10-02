using System.Collections;
using LD51.Data.Misc;
using LD51.Data.Tensies;
using LD51.Data.World;
using LD51.Inputs;
using UnityEngine;
using Utils.Coroutines;
using Utils.Extensions;

namespace LD51.Game {
	public class CountdownBeforeCharacterState : AbstractStateBehaviour {
		public static CountdownBeforeCharacterState state { get; } = new CountdownBeforeCharacterState();

		private TensieSpawnerModule spawnerModule { get; set; }
		private Tensie              newTensie     { get; set; }

		private CountdownBeforeCharacterState() { }

		public void PrepareState(TensieSpawnerModule spawnerModule) {
			this.spawnerModule = spawnerModule;
			newTensie = Object.Instantiate(spawnerModule.tensiePrefab, spawnerModule.spawnPosition, Quaternion.identity, null);
		}

		protected override void EnableState() {
			base.EnableState();
			spawnerModule.worldObject.SetSelected(true);
			WorldObjectManager.SetHoverWithMouseEnabled(true);
			newTensie.SetSelected(true);
			newTensie.SetGhost(true);
			GameCamera.main.SetTarget(spawnerModule.transform);
			CoroutineRunner.Run(DoCountdown());
		}

		private IEnumerator DoCountdown() {
			var cancelled = false;
			while (!cancelled && !GameTime.justStartedNewLoop) {
				cancelled = GameInput.controls.PrepareTensie.Cancel.WasPerformedThisFrame();
				yield return null;
			}
			if (cancelled) {
				Object.Destroy(newTensie.gameObject);
				spawnerModule.worldObject.SetSelected(false);
				ChangeState(OverviewGameState.state);
			}
			else {
				if (spawnerModule.assignedTensie) Object.Destroy(spawnerModule.assignedTensie.gameObject);
				DefineCharacterActionState.state.PrepareState(spawnerModule, newTensie);
				ChangeState(DefineCharacterActionState.state);
			}
		}

		protected override void SetListenersEnabled(bool enabled) {
			GameInput.controls.PrepareTensie.Cancel.SetEnabled(enabled);
		}
	}
}