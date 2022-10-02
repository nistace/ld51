using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils.Extensions;

namespace LD51.Data.World {
	public class WorldRenderer : MonoBehaviour {
		public static WorldRenderer instance { get; private set; }

		[SerializeField] protected Tilemap             _tilemap;
		[SerializeField] protected Vector2Int          _min;
		[SerializeField] protected Vector2Int          _max;
		[SerializeField] protected int                 _shoreWidth = 20;
		[SerializeField] protected Layer[]             _layers;
		[SerializeField] protected string              _seed        = "seed";
		[SerializeField] protected float               _perlinScale = .01f;
		[SerializeField] protected List<BoxCollider2D> _boundsColliders;

		private IReadOnlyList<Vector2>            perlinOffsets { get; set; }
		private Dictionary<string, HashSet<Tile>> tiles         { get; } = new Dictionary<string, HashSet<Tile>>();

		[ContextMenu("Rebuild")] private void Rebuild() => Build();

		private void Awake() {
			instance = this;
		}

		public void Build(string seed = null) {
			_seed = seed ?? _seed;
			_tilemap.ClearAllTiles();

			LoadTiles();

			var random = new System.Random(Mathf.FloorToInt(_seed.ToCharArray().Select((t, i) => Mathf.Pow(t, (i + 2f) / 4)).Sum()));
			perlinOffsets = _layers.Select(_ => new Vector2(random.Next() * _perlinScale % 1, random.Next() * _perlinScale % 1)).ToArray();

			for (var x = _min.x - _shoreWidth; x < _max.x + _shoreWidth; ++x)
			for (var y = _min.y - _shoreWidth; y < _max.y + _shoreWidth; ++y) {
				var position = new Vector3Int(x, y, 0);
				_tilemap.SetTile(position, DetermineTileAtCoordinates(x, y));
				_tilemap.SetColor(position, Color.white);
			}

			BuildColliders();
		}

		private void BuildColliders() {
			_boundsColliders[0].offset = new Vector2(_min.x - _shoreWidth / 2f, (_max.y + _min.y) / 2f);
			_boundsColliders[0].size = new Vector2(_shoreWidth, _max.y - _min.y + _shoreWidth * 2);

			_boundsColliders[1].offset = new Vector2(_max.x + (_shoreWidth + 1) / 2f, (_max.y + _min.y) / 2f);
			_boundsColliders[1].size = new Vector2(_shoreWidth - 1, _max.y - _min.y + _shoreWidth * 2);

			_boundsColliders[2].offset = new Vector2((_max.x + _min.x + 1) / 2f, _min.y - _shoreWidth / 2f);
			_boundsColliders[2].size = new Vector2(_max.y - _min.y - 3, _shoreWidth);

			_boundsColliders[3].offset = new Vector2((_max.x + _min.x + 1) / 2f, _max.y + (_shoreWidth + 1) / 2f);
			_boundsColliders[3].size = new Vector2(_max.y - _min.y - 3, _shoreWidth - 1);
		}

		private void LoadTiles() {
			tiles.Clear();
			foreach (var layer in _layers) {
				LoadTileLayer(layer.name);
			}
			LoadTileLayer("water");
		}

		private void LoadTileLayer(string layerName) {
			tiles.Add(layerName, new HashSet<Tile>());
			foreach (var frame in SpriteAtlasLibrary.grounds[layerName]["default"].frames) {
				var tile = ScriptableObject.CreateInstance<Tile>();
				tile.sprite = frame;
				tiles[layerName].Add(tile);
			}
		}

		private Tile DetermineTileAtCoordinates(int x, int y) {
			if (x < _min.x || x > _max.x || y < _min.x || y > _max.y) return tiles["water"].Random();
			for (var i = _layers.Length - 1; i >= 0; i--) {
				if (i == 0 || Mathf.PerlinNoise(perlinOffsets[i].x + (1000 + x) * _perlinScale, perlinOffsets[i].y + (1000 + y) * _perlinScale) < _layers[i].probability) {
					return tiles[_layers[i].name].Random();
				}
			}
			return null;
		}

		[Serializable]
		public class Layer {
			[SerializeField] protected string _name;
			[SerializeField] protected float  _probability = .5f;

			public string name        => _name;
			public float  probability => _probability;
		}

		public static Vector3 FixObjectPosition(WorldObject worldObject, Vector3? positionToFix = null) {
			var x = (positionToFix ?? worldObject.transform.position).x;
			if (worldObject.widthInCells % 2 == 1) x -= .5f;
			x = Mathf.RoundToInt(x);
			if (worldObject.widthInCells % 2 == 1) x += .5f;
			return new Vector3(x, Mathf.RoundToInt((positionToFix ?? worldObject.transform.position).y), 0);
		}

#if UNITY_EDITOR

		[ContextMenu("Preview")] private void Preview() {
			EditorCoroutineUtility.StartCoroutine(DoPreview(), this);
		}

		private IEnumerator DoPreview() {
			var guids = AssetDatabase.FindAssets($"t:{nameof(SpriteAtlasLibrary)}");
			SpriteAtlasLibrary.instance = AssetDatabase.LoadAssetAtPath<SpriteAtlasLibrary>(AssetDatabase.GUIDToAssetPath(guids[0]));
			yield return EditorCoroutineUtility.StartCoroutineOwnerless(SpriteAtlasLibrary.grounds.Build());
			yield return EditorCoroutineUtility.StartCoroutineOwnerless(SpriteAtlasLibrary.buildings.Build());
			Rebuild();
			foreach (var worldObject in FindObjectsOfType<WorldObject>(true)) worldObject.RefreshVisuals();
		}

		[ContextMenu("Fix All Buildings Position")] private void FixAllBuildingsPosition() {
			foreach (var worldObject in FindObjectsOfType<WorldObject>(true)) {
				worldObject.transform.position = FixObjectPosition(worldObject);
			}
		}

#endif
	}
}