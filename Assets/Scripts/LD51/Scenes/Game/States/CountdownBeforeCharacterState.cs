using System.Collections;
using LD51.Data;
using LD51.Data.Tensies;
using LD51.Data.World;
using UnityEngine;
using Utils.Coroutines;

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
			newTensie.SetSelected(true);
			newTensie.SetGhost(true);
			GameCamera.main.SetTarget(spawnerModule.transform);
			CoroutineRunner.Run(DoCountdown());
		}

		private IEnumerator DoCountdown() {
			while (!GameTime.justStartedNewLoop) yield return null;
			if (spawnerModule.assignedTensie) Object.Destroy(spawnerModule.assignedTensie.gameObject);
			DefineCharacterActionState.state.PrepareState(spawnerModule, newTensie);
			ChangeState(DefineCharacterActionState.state);
		}

		protected override void SetListenersEnabled(bool enabled) { }
	}
}