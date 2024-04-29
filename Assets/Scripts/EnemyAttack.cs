using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
	[SerializeField] private int _damage;
	[SerializeField] private Collider2D _attackCollider;
	[SerializeField] private LayerMask _playerLayer;
	[SerializeField] private float _punchForce;
	[SerializeField] private float _punchUpForce;

	private ContactFilter2D _contactFilter2D = new ContactFilter2D().NoFilter();

	public void DoAttack()
	{
		_attackCollider.gameObject.SetActive(true);
		List<Collider2D> collidersHits = new();

		int colliderHitsCount = _attackCollider.OverlapCollider(_contactFilter2D, collidersHits);

		if (colliderHitsCount > 0)
		{
			foreach (Collider2D collider in collidersHits)
			{
				if (collider.gameObject.TryGetComponent<PlayerAnimator>(out PlayerAnimator playerAnimator))
				{
					PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
					playerHealth.TakeDamage(_damage);
					Vector2 punchVector = new Vector2(transform.right.x * _punchForce, _punchUpForce);
					playerHealth.gameObject.GetComponent<Rigidbody2D>().AddForce(punchVector, ForceMode2D.Impulse);
					break;
				}
			}
		}
	}

	private void EndAttack()
	{
		_attackCollider.gameObject.SetActive(false);
	}
}

/*
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
					EnemyHealth playerHealth = collider.GetComponent<EnemyHealth>();
					playerHealth.TakeDamage(_damage);
					Vector2 punchVector = new Vector2(transform.right.x * _punchForce, _punchUpForce);
					playerHealth.gameObject.GetComponent<Rigidbody2D>().AddForce(punchVector, ForceMode2D.Impulse);
					break;
				}
			}
		}

		_capsuleCollider.gameObject.SetActive(false);
 
 
 
 */
