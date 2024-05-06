using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private int _maxHealth;
	[SerializeField] private int _health;

	public event Action DeadOrdered;
	public event Action DamageTakeOrderd;

	public void TakeDamage(int damage)
	{
		_health -= damage;

		if (_health <= 0)
			DeadOrdered?.Invoke();
		else
			DamageTakeOrderd?.Invoke();
	}

	public void Healing(int healing)
	{
		_health += healing;

		if (_health > _maxHealth)
			_health = _maxHealth;
	}
}