using System;
using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField] private int _maxHealth;
	[SerializeField] private int _health;

	public int GetMaxHealth => _maxHealth;
	public int GetHealth => _health;

	public event Action Died;
	public event Action Damaging;
	public event Action TakeHealing;

	public void TakeDamage(int damage)
	{
		if (damage > 0)
		{
			_health -= damage;

			if (_health <= 0)
			{
				_health = 0;
				Died?.Invoke();
			}

			Damaging?.Invoke();
		}

	}

	public void Healing(int healing)
	{
		if (healing > 0)
			_health += healing;

		if (_health > _maxHealth)
			_health = _maxHealth;

		TakeHealing?.Invoke();
	}
}