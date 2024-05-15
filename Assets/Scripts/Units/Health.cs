using System;
using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField] private int _maxHealth;
	[SerializeField] private int _health;

	public event Action Died;
	public event Action Damaging;

	public void TakeDamage(int damage)
	{
		if (damage > 0)
			_health -= damage;

		if (_health <= 0)
			Died?.Invoke();
		else
			Damaging?.Invoke();
	}

	public void Healing(int healing)
	{
		_health += healing;

		if (_health > _maxHealth)
			_health = _maxHealth;
	}
}