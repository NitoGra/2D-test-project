using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Attack : MonoBehaviour
{
	[SerializeField] private int _damage;
	[SerializeField] private float _punchForce;
	[SerializeField] private float _punchUpForce;
	[SerializeField] private Collider2D _damageCollider;
	[SerializeField] private float _attackEndDelay;

	private PlayerAudio _audio;
	private bool _isAudioPlaying;
	private bool _playHitSound;
	private Collider2D _colliderIgnore;
	private ContactFilter2D _contactFilter2D = new ContactFilter2D().NoFilter();

	public event Action PlayHitSound;
	public event Action PlayMissSound;

	private void Start()
	{
		_audio = GetComponent<PlayerAudio>();
		_isAudioPlaying = _audio != null;
		_colliderIgnore = gameObject.GetComponent<Collider2D>();
	}

	public void DoAttack()
	{
		StartAttack();

		if (TryFindEnemies(out List<Collider2D> collidersHits))
			MakeAttack(collidersHits);

		if (_isAudioPlaying)
			PlaySound();

		Invoke(nameof(EndAttack), _attackEndDelay);
	}

	private bool TryFindEnemies(out List<Collider2D> collidersHits)
	{
		collidersHits = new();
		int colliderHitsCount = _damageCollider.OverlapCollider(_contactFilter2D, collidersHits);
		return colliderHitsCount > 0;
	}

	private void MakeAttack(List<Collider2D> collidersHits)
	{
		foreach (Collider2D collider in collidersHits)
		{
			if (collider == _colliderIgnore)
				continue;

			if (collider.TryGetComponent(out Health health))
			{
				health.TakeDamage(_damage);
				Vector2 punchVector = new(transform.right.x * _punchForce, _punchUpForce);
				health.gameObject.GetComponent<Rigidbody2D>().AddForce(punchVector, ForceMode2D.Impulse);
				_playHitSound = true;
			}
		}
	}

	private void PlaySound()
	{
		if (_playHitSound)
			PlayHitSound?.Invoke();
		else
			PlayMissSound?.Invoke();

		_playHitSound = false;
	}

	private void StartAttack() => _damageCollider.gameObject.SetActive(true);
	private void EndAttack() => _damageCollider.gameObject.SetActive(false);
}