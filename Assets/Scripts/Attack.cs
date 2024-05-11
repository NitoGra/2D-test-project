using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Attack : MonoBehaviour
{
	[SerializeField] private int _damage;
	[SerializeField] private float _punchForce;
	[SerializeField] private float _punchUpForce;
	[SerializeField] private Unit _control;
	[SerializeField] private Collider2D _damageCollider;
	[SerializeField] private float _attackColliderVanishDelay;

	private PlayerAudio _audio;
	private Collider2D _colliderIgnore;
	private ContactFilter2D _contactFilter2D = new ContactFilter2D().NoFilter();

	private void Start()
	{
		_audio = GetComponent<PlayerAudio>();
		_colliderIgnore = gameObject.GetComponent<Collider2D>();
	}

	public void DoAttack()
	{
		List<Collider2D> collidersHits = new();
		_damageCollider.gameObject.SetActive(true);
		int colliderHitsCount = _damageCollider.OverlapCollider(_contactFilter2D, collidersHits);

		if (colliderHitsCount > 0)
		{
			foreach (Collider2D collider in collidersHits)
			{
				if (collider == _colliderIgnore)
					continue;

				if (collider.gameObject.TryGetComponent(out Health health))
				{
					health.TakeDamage(_damage);
					Vector2 punchVector = new(transform.right.x * _punchForce, _punchUpForce);
					health.gameObject.GetComponent<Rigidbody2D>().AddForce(punchVector, ForceMode2D.Impulse);
					_audio?.HitSound();
				}
				else
				{
					_audio?.MissSound();
				}
			}
		}
		else
		{
			_audio?.MissSound();
		}

		Invoke(nameof(ColliderVanish), _attackColliderVanishDelay);
	}

	private void ColliderVanish()
	{
		_damageCollider.gameObject.SetActive(false);
	}
}