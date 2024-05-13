using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),typeof(Collider2D))]
public class Pickups : MonoBehaviour
{
	[SerializeField] private float _value;
	[SerializeField] private float _disappearDelay;
	[SerializeField] private AudioSource _audio;

	private SpriteRenderer _spriteRenderer;
	private Collider2D _collider;

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_collider = GetComponent<Collider2D>();
	}

	public float Value => _value;

	public virtual void Pickup(PlayerAudio playerAudio, AudioClip audioClip)
	{
		_audio.clip = audioClip;
		_audio.Play();
		_spriteRenderer.enabled= false;
		_collider.enabled = false;

		if(TryGetComponent(out Rigidbody2D rigidbody))
			rigidbody.simulated= false;

		Invoke(nameof(Disappear), _disappearDelay);
	}

	private void Disappear()
	{
		gameObject.SetActive(false);
	}
}