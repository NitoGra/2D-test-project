using UnityEngine;

[RequireComponent(typeof(PlayerAudio), typeof(Health))]
public class PlayerPickups : MonoBehaviour
{
	private PlayerAudio _audio;
	private Health _health;

	private void Start()
	{
		_audio = GetComponent<PlayerAudio>();
		_health = GetComponent<Health>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent(out Coin coin))
			TakeCoin(coin);
		else if (collision.gameObject.TryGetComponent(out MedicBag medicBag))
			TakeMedicBag(medicBag);
	}

	private void TakeMedicBag(MedicBag medicBag)
	{
		medicBag.Pickup(_audio, _audio.GetMedicBagSound);
		_health.Healing(medicBag.Value);
	}

	private void TakeCoin(Coin coin)
	{
		coin.Pickup(_audio, _audio.GetCoinSound);
	}
}