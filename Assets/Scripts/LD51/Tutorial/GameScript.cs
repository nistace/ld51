using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LD51.Data.GameResources;
using LD51.Data.Misc;
using LD51.Data.World;
using LD51.Menu.Ui;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Utils.Extensions;
using Random = UnityEngine.Random;

namespace LD51.Data.Tutorial {
	public class GameScript : MonoBehaviour {
		public static GameScript instance { get; private set; }

		public class Event : UnityEvent<GameScript> { }

		[SerializeField] protected string[]       _names = { "Osgdrag", "Aksol", "Parhbta" };
		[SerializeField] protected string         _fatherName;
		[SerializeField] protected string         _grandfatherName;
		[SerializeField] protected string         _mayorName;
		[SerializeField] protected PortraitProp[] _portraitPropProbabilities;
		[SerializeField] protected List<string>   _portraitProps = new List<string>();
		[SerializeField] protected ScriptPart[]   _scriptParts;
		[SerializeField] protected int            _currentScriptPartIndex;
		[SerializeField] protected bool           _currentPartUnlocked;

		public string                fatherName      => _fatherName;
		public string                grandfatherName => _grandfatherName;
		public string                mayorName       => _mayorName;
		public IReadOnlyList<string> portraitProps   => _portraitProps;

		public static Event onNewLoopHandled     { get; } = new Event();
		public static Event onScriptPartUnlocked { get; } = new Event();
		public        bool  currentPartUnlocked  => _currentPartUnlocked;

		private void Awake() {
			instance = this;
		}

		private void Start() {
			TryUnlockCurrent();
			_grandfatherName = _names.Random();
			_fatherName = _names.Random();
			_mayorName = _names.Random();
			GenerateNewPortrait();
			GameInventory.onNoFoodLeft.AddListenerOnce(() => StartCoroutine(LoadingScreenUi.FadeIn(() => SceneManager.LoadSceneAsync("Menu"))));
		}

		private void Update() {
			if (GameTime.justStartedNewLoop) {
				_grandfatherName = _fatherName;
				_fatherName = _mayorName;
				_mayorName = _names.Random();
				GenerateNewPortrait();
				onNewLoopHandled.Invoke(this);
			}
		}

		private void GenerateNewPortrait() {
			_portraitProps.Clear();
			foreach (var prop in _portraitPropProbabilities) {
				if (Random.value < prop.probability) _portraitProps.Add(prop.name);
			}
		}

		public bool ContinueScript() {
			if (_scriptParts[_currentScriptPartIndex].effect == ScriptPart.Effect.LoadGame) StartCoroutine(LoadingScreenUi.FadeIn(() => SceneManager.LoadSceneAsync("Game")));
			if (_scriptParts[_currentScriptPartIndex].effect == ScriptPart.Effect.LoadMenu) StartCoroutine(LoadingScreenUi.FadeIn(() => SceneManager.LoadSceneAsync("Menu")));

			_currentPartUnlocked = false;
			_currentScriptPartIndex++;
			if (_currentScriptPartIndex >= _scriptParts.Length) return false;
			TryUnlockCurrent();
			return _currentPartUnlocked;
		}

		private void TryUnlockCurrent() {
			if (_currentPartUnlocked) return;
			if (_scriptParts[_currentScriptPartIndex].unlock == ScriptPart.Unlock.Direct) UnlockCurrentScriptPart();
			else if (_scriptParts[_currentScriptPartIndex].unlock == ScriptPart.Unlock.Wait5Sec) StartCoroutine(UnlockIn5Seconds());
			else if (_scriptParts[_currentScriptPartIndex].unlock == ScriptPart.Unlock.BuildStatue) UnlockWhenStatueBuilt();
			else if (_scriptParts[_currentScriptPartIndex].unlock == ScriptPart.Unlock.Collect4FoodAtOnce) UnlockCollect4FoodAtOnce();
			else if (_scriptParts[_currentScriptPartIndex].unlock == ScriptPart.Unlock.CollectWood) UnlockCollectWood();
			else if (_scriptParts[_currentScriptPartIndex].unlock == ScriptPart.Unlock.BuildSomething) UnlockBuildSomething();
		}

		public bool TryGetCurrentScriptPart(out ScriptPart scriptPart) {
			scriptPart = null;
			if (_currentScriptPartIndex < 0 || _currentScriptPartIndex >= _scriptParts.Length) return false;
			scriptPart = _scriptParts[_currentScriptPartIndex];
			return true;
		}

		private IEnumerator UnlockIn5Seconds() {
			yield return new WaitForSeconds(5);
			UnlockCurrentScriptPart();
		}

		private void UnlockCurrentScriptPart() {
			_currentPartUnlocked = true;

			Invoke(nameof(PlayInitialEffects), .1f);

			onScriptPartUnlocked.Invoke(this);
		}

		private void PlayInitialEffects() {
			if (_scriptParts[_currentScriptPartIndex].effect == ScriptPart.Effect.Audio3) GameAudio.desiredLevel = 3;
			else if (_scriptParts[_currentScriptPartIndex].effect == ScriptPart.Effect.Audio4) GameAudio.desiredLevel = 4;
			else if (_scriptParts[_currentScriptPartIndex].effect == ScriptPart.Effect.FocusStatue)
				GameCamera.main.SetTarget(FindObjectsOfType<WorldObject>().SingleOrDefault(t => t.IsMakerStatue())?.transform);
		}

		private void UnlockCollect4FoodAtOnce() {
			GameResourceStorageModule.onStoredResources.AddListenerOnce(UnlockIf4Food);
		}

		private void UnlockIf4Food(GameResource resource, int amount) {
			if (!resource.food) return;
			if (amount < 4) return;
			GameResourceStorageModule.onStoredResources.RemoveListener(UnlockIf4Food);
			UnlockCurrentScriptPart();
		}

		private void UnlockCollectWood() {
			GameResourceStorageModule.onStoredResources.AddListenerOnce(UnlockIfWood);
		}

		private void UnlockIfWood(GameResource resource, int amount) {
			if (resource.name != "Wood") return;
			if (amount < 1) return;
			GameResourceStorageModule.onStoredResources.RemoveListener(UnlockIfWood);
			UnlockCurrentScriptPart();
		}

		private void UnlockBuildSomething() {
			ConstructionSiteModule.onConstructionComplete.AddListenerOnce(UnlockWhateverThisIs);
		}

		private void UnlockWhateverThisIs(ConstructionSiteModule arg0) {
			ConstructionSiteModule.onConstructionComplete.RemoveListener(UnlockWhateverThisIs);
			UnlockCurrentScriptPart();
		}

		private void UnlockWhenStatueBuilt() {
			ConstructionSiteModule.onConstructionComplete.AddListenerOnce(UnlockIfMakerStatue);
		}

		private void UnlockIfMakerStatue(ConstructionSiteModule built) {
			if (!built.resultPrefab.IsMakerStatue()) return;
			ConstructionSiteModule.onConstructionComplete.RemoveListener(UnlockIfMakerStatue);
			UnlockCurrentScriptPart();
		}

		[Serializable] public class PortraitProp {
			[SerializeField] protected string _name;
			[SerializeField] protected float  _probability;

			public string name        => _name;
			public float  probability => _probability;
		}

		[Serializable] public class ScriptPart {
			public enum Unlock {
				Direct             = 0,
				Wait5Sec           = 1,
				BuildStatue        = 2,
				Collect4FoodAtOnce = 3,
				CollectWood        = 4,
				BuildSomething     = 5
			}

			public enum Effect {
				None        = 0,
				Audio3      = 1,
				Audio4      = 2,
				LoadGame    = 3,
				LoadMenu    = 4,
				FocusStatue = 5
			}

			[SerializeField] protected string _text;
			[SerializeField] protected Unlock _unlock;
			[SerializeField] protected Effect _effect     = Effect.None;
			[SerializeField] protected string _buttonText = "Continue";

			public string text       => _text;
			public Unlock unlock     => _unlock;
			public Effect effect     => _effect;
			public string buttonText => _buttonText;
		}

		public static void Restart() {
			instance._currentScriptPartIndex = 0;
			instance._currentPartUnlocked = false;
			instance.TryUnlockCurrent();
		}
	}
}