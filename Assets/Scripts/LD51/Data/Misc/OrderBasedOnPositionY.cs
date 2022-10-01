using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OrderBasedOnPositionY : MonoBehaviour {
	[SerializeField] protected float _maxY;
	[SerializeField] protected float _precision = 10;
	[SerializeField] protected bool  _onlyOnStart;

	private SpriteRenderer spriteRenderer { get; set; }

	private void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		RefreshOrder();
		if (_onlyOnStart) Destroy(this);
	}

	private void RefreshOrder() {
		spriteRenderer.sortingOrder = Mathf.FloorToInt((_maxY - transform.position.y) * _precision);
	}

	private void Update() => RefreshOrder();
}