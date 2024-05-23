using System.Collections;
using UnityEngine;

public class SmoothHealthBar : BaseHealthBar
{
	[SerializeField] private float _step;

	private Coroutine _healthChanger;

	private void Start() => Invoke(nameof(ChangeBar), 0.1f);

	protected override void ChangeBar()
	{
		float health = GetValue;
		float maxHealth = GetMax;
		float healthValue = health / maxHealth;

		if (_healthChanger != null)
			BreakCorutine();

		_healthChanger = StartCoroutine(SlmoothChange(healthValue));
	}

	private IEnumerator SlmoothChange(float healthValue)
	{
		while (Value != healthValue)
		{
			SetValue(Mathf.MoveTowards(Value, healthValue, _step * Time.deltaTime));
			yield return null;
		}

		BreakCorutine();
	}

	private void BreakCorutine()
	{
		if (_healthChanger != null)
			StopCoroutine(_healthChanger);

		_healthChanger = null;
	}
}