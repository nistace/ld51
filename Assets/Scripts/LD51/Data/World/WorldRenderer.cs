using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils.Extensions;

namespace LD51.Data.World {
	public class WorldRenderer : MonoBehaviour {
		public static WorldRenderer instance { get; private set; }

		[SerializeField] protected Tilemap    _tilemap;
		[SerializeField] protected Vector2Int _min;
		[SerializeField] protected Vector2Int _max;
		[SerializeField] protected Layer[]    _layers;
		[SerializeField] protected string     _seed        = "seed";
		[SerializeField] protected float      _perlinScale = .01f;

		private IReadOnlyList<Vector2>            perlinOffsets { get; set; }
		private Dictionary<string, HashSet<Tile>> tiles         { get; } = new Dictionary<string, HashSet<Tile>>();

		[ContextMenu("Rebuild")] private void Rebuild() => Build();

		private void Awake() {
			instance = this;
		}

		public void Build(string seed = null) {
			_seed = seed ?? _seed;
			_tilemap.ClearAllTiles();

			tiles.Clear();
			foreach (var layerName in _layers) {
				tiles.Add(layerName.name, new HashSet<Tile>());
				foreach (var frame in SpriteAtlasLibrary.grounds[layerName.name]["default"].frames) {
					var tile = ScriptableObject.CreateInstance<Tile>();
					tile.sprite = frame;
					tiles[layerName.name].Add(tile);
				}
			}

			var random = new System.Random(Mathf.FloorToInt(_seed.ToCharArray().Select((t, i) => Mathf.Pow(t, (i + 2f) / 4)).Sum()));
			perlinOffsets = _layers.Select(_ => new Vector2(random.Next() * _perlinScale % 1, random.Next() * _perlinScale % 1)).ToArray();

			for (var x = _min.x; x < _max.x; ++x)
			for (var y = _min.y; y < _max.y; ++y) {
				var position = new Vector3Int(x, y, 0);
				_tilemap.SetTile(position, DetermineTileAtCoordinates(x, y));
				_tilemap.SetColor(position, Color.white);
			}
		}

		private Tile DetermineTileAtCoordinates(int x, int y) {
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
	}
}