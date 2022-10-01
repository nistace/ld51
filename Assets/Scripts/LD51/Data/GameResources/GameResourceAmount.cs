using System;
using UnityEngine;

namespace LD51.Data.GameResources {
	[Serializable]
	public class GameResourceAmount {
		[SerializeField] protected GameResource _resource;
		[SerializeField] protected int          _amount;

		public GameResource resource => _resource;
		public int          amount   => _amount;

		public GameResourceAmount() { }

		public GameResourceAmount(GameResource resource, int amount) {
			_resource = resource;
			_amount = amount;
		}
	}
}