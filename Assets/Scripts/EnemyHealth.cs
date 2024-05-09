using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	[SerializeField] private int _health;

	public event Action DeadOrdered;
	public event Action DamageOrdered;

	public void TakeDamage(int damage)
	{
		_health -= damage;
		DamageOrdered?.Invoke();

		if (_health <= 0)
		{
			DeadOrdered?.Invoke();
		}
	}
	
	public void Dead()
	{
		gameObject.SetActive(false);
	}
}