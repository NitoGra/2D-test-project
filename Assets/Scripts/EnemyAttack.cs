using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
	[SerializeField] private int _damage;
	[SerializeField] private Collider2D _attackCollider;

	private ContactFilter2D _contactFilter2D = new ContactFilter2D().NoFilter();

	public event Action EnemyWalkOrdered;

	public void DoAttack()
	{
		_attackCollider.gameObject.SetActive(true);
		_attackCollider.transform.position = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
		_attackCollider.transform.rotation = new Quaternion(_attackCollider.transform.rotation.x, _attackCollider.transform.rotation.y, _attackCollider.transform.rotation.z, _attackCollider.transform.rotation.w);
		List<Collider2D> collidersHits = new();

		int colliderHitsCount = _attackCollider.OverlapCollider(_contactFilter2D, collidersHits);

		if (colliderHitsCount > 0)
		{
			foreach (Collider2D collider in collidersHits)
			{
				if (collider.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
				{
					playerHealth.TakeDamage(_damage);
					break;
				}
			}
		}
	}
	
	private void EndAttack()
	{
		_attackCollider.gameObject.SetActive(false);
		EnemyWalkOrdered?.Invoke();
	}
}