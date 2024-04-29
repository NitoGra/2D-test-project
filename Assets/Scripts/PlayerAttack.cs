using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerAttack : MonoBehaviour
{
	[SerializeField] private MotionControl _motionControl;
	[SerializeField] private CapsuleCollider2D _capsuleCollider;
	[SerializeField] private int _damage;
	[SerializeField] private LayerMask _groundMask;
	[SerializeField] private float _punchForce;
	[SerializeField] private float _punchUpForce;

	private ContactFilter2D _contactFilter2D = new ContactFilter2D().NoFilter();

	public void DoAttack()
	{
		List<Collider2D> collidersHits = new();
		_capsuleCollider.gameObject.SetActive(true);
		int colliderHitsCount = _capsuleCollider.OverlapCollider(_contactFilter2D, collidersHits);

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
					break;
				}
			}
		}

		_capsuleCollider.gameObject.SetActive(false);
	}
}