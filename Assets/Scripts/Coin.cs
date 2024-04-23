using UnityEngine;

public class Coin : MonoBehaviour
{
	[SerializeField] private AudioSource _audio;
	[SerializeField] private float _delay;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.GetComponent<PlayerAnimator>())
		{
			_audio.Play();
			Invoke(nameof(Die), _delay);
		}
	}

	private void Die()
	{
		Destroy(gameObject);
	}
}