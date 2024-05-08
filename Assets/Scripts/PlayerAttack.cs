using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerAttack : MonoBehaviour
{
	[SerializeField] private MotionControl _motionControl;
	[SerializeField] private CapsuleCollider2D _damageCollider;
	[SerializeField] private int _damage;
	[SerializeField] private float _punchForce;
	[SerializeField] private float _punchUpForce;

	private AudioSource _audio;
	[SerializeField] private AudioClip _punchHitSound;
	[SerializeField] private AudioClip _punchMissSound;

	private ContactFilter2D _contactFilter2D = new ContactFilter2D().NoFilter();

	private void Start()
	{
		_audio = GetComponent<AudioSource>();
	}

	public void DoAttack()
	{
		List<Collider2D> collidersHits = new();
		_damageCollider.gameObject.SetActive(true);
		int colliderHitsCount = _damageCollider.OverlapCollider(_contactFilter2D, collidersHits);
		_audio.clip = _punchMissSound;

		if (colliderHitsCount > 0)
		{
			foreach (Collider2D collider in collidersHits)
			{
				if (collider.gameObject.TryGetComponent<EnemyAnimator>(out EnemyAnimator enemyAnimator))
				{
					EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
					enemyHealth.TakeDamage(_damage);
					Vector2 punchVector = new Vector2(transform.right.x * _punchForce, _punchUpForce);
					enemyHealth.gameObject.GetComponent<Rigidbody2D>().AddForce(punchVector, ForceMode2D.Impulse);
					_audio.clip = _punchHitSound;
					break;
				}
			}
		}

		_audio.Play();
		_damageCollider.gameObject.SetActive(false);
	}
}