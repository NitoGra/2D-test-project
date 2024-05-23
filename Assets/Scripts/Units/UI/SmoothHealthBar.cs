using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothHealthBar : BaseHealth
{
	[SerializeField] private Slider _healthBar;
	[SerializeField] private float _step;

	private Coroutine _healthChanger;

	private void Start() => Invoke(nameof(ChangeBar), 0.1f);

	protected override void ChangeBar()
	{
		float health = Health.GetHealth;
		float maxHealth = Health.GetMaxHealth;
		float healthValue = health / maxHealth;

		if (_healthChanger != null)
			BreakCorutine();

		_healthChanger = StartCoroutine(SlmoothChange(healthValue));
	}

	private IEnumerator SlmoothChange(float healthValue)
	{
		while (_healthBar.value != healthValue)
		{
			_healthBar.value = Mathf.MoveTowards(_healthBar.value, healthValue, _step * Time.deltaTime);
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