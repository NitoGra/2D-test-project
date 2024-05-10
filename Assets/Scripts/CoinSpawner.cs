using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
	[SerializeField] private Coin _coinPrefab;
	[SerializeField] private float _delay;
	[SerializeField] private int _maxCoinCount;
	[SerializeField] private Vector2 _force;

	private void Awake()
	{
		StartCoroutine(Spawn(_delay));
	}

	private IEnumerator Spawn(float delay)
	{
		WaitForSeconds wait = new WaitForSeconds(delay);

		for (int coinCount = 0; coinCount < _maxCoinCount; coinCount++)
		{
			Coin coin = Instantiate(_coinPrefab);
			Rigidbody2D coinRigidbody = coin.GetComponent<Rigidbody2D>();
			coin.transform.SetParent(transform);
			coin.transform.position = transform.position;
			coinRigidbody.AddForce(_force, ForceMode2D.Impulse);
			yield return wait;
		}
	}
}