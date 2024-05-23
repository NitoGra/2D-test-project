using System;
using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField] private float _maxValue;
	[SerializeField] private float _value;

	public event Action Died;
	public event Action Damaged;
	public event Action Healed;

	public float GetMax => _maxValue;
	public float GetValue => _value;

	public void TakeDamage(float damage)
	{
		print(damage);

		if (damage > 0)
		{
			_value -= damage;

			if (_value <= 0)
			{
				_value = 0;
				Died?.Invoke();
			}

			Damaged?.Invoke();
		}

	}

	public void Healing(float healing)
	{
		if (healing > 0)
			_value += healing;

		if (_value > _maxValue)
			_value = _maxValue;

		Healed?.Invoke();
	}
}