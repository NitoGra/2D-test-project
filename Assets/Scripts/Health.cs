using System;
using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField] private int _maxHealth;
	[SerializeField] private int _health;

	public event Action DieOrdered;
	public event Action TakeDamageOrdered;

	public void TakeDamage(int damage)
	{
		_health -= damage;

		if (_health <= 0)
			DieOrdered?.Invoke();
		else
			TakeDamageOrdered?.Invoke();
	}

	public void Healing(int healing)
	{
		_health += healing;

		if (_health > _maxHealth)
			_health = _maxHealth;
	}
}