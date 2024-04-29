using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	[SerializeField] private int _health;
	[SerializeField] private EnemyAnimator _enemyAnimator;

	public void TakeDamage(int damage)
	{
		_health -= damage;

		if (_health <= 0)
		{
			_enemyAnimator.PlayDead();
		}
	}

	public void Dead()
	{
		gameObject.SetActive(false);
	}
}