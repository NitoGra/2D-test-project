using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
	[SerializeField] private Rigidbody2D _coinPrefab;
	[SerializeField] private float _delay;
	[SerializeField] private int _maxCoinCount;
	[SerializeField] private Vector2 _force;

	private int _coinCount;

	private void Awake()
	{
		StartCoroutine(Spawn(_delay));
	}

	private IEnumerator Spawn(float delay)
	{
		WaitForSeconds wait = new WaitForSeconds(delay);

		while (_coinCount < _maxCoinCount)
		{
			_coinCount++;
			Rigidbody2D coin = Instantiate(_coinPrefab);
			coin.transform.position = transform.position;
			coin.AddForce(_force, ForceMode2D.Impulse);
			yield return wait;
		}
	}
}