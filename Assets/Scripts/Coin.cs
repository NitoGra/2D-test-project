using UnityEngine;

public class Coin : MonoBehaviour
{
	[SerializeField] private AudioSource _audio;
	[SerializeField] private float _delay;

	public void Taked()
	{
		_audio.Play();
		Invoke(nameof(Die), _delay);
	}

	private void Die()
	{
		Destroy(gameObject);
	}
}