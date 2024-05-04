using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private int _health;

	public event Action DeadOrdered;

	private void Start()
	{

	}

	public void TakeDamage(int damage)
	{
		_health -= damage;
		print("Урон!");

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
