using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	[SerializeField] private int _health;

	public event Action EnemyDeadOrdered;

	public void TakeDamage(int damage)
	{
		_health -= damage;

		if (_health <= 0)
		{
			EnemyDeadOrdered?.Invoke();
		}
	}

	public void Dead()
	{
		gameObject.SetActive(false);
	}
}