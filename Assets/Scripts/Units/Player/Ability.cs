using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Ability : MonoBehaviour
{
	[SerializeField] float _value;
	[SerializeField] float _delay;
	[SerializeField] float _actionTime;

	[SerializeField] private CircleCollider2D _range;
	[SerializeField] private KeyDetect _keyDetect;

	private Health _health;
	private Coroutine _activeCast = null;
	private WaitForSecondsRealtime _castDelay;
	private ContactFilter2D _contactFilter2D = new ContactFilter2D().NoFilter();

	private void Start()
	{
		_castDelay = new WaitForSecondsRealtime(_delay);
		_health = GetComponent<Health>();
	}

	private void OnEnable()
	{
		_keyDetect.AbilityUsed += Activate;
	}

	private void OnDisable()
	{
		_keyDetect.AbilityUsed -= Activate;
	}

	private void Activate()
	{
		if (_activeCast != null)
			return;

		_activeCast = StartCoroutine(Cast());
	}

	private void BreakCorutine()
	{
		if (_activeCast != null)
			StopCoroutine(_activeCast);

		_activeCast = null;
	}

	private IEnumerator Cast()
	{
		float timer = 0;

		while (timer <= _actionTime)
		{
			if (TryFindEnemies(out List<Collider2D> collidersHits))
				DrainHealth(collidersHits);

			timer += _delay;
			yield return _castDelay;
		}

		BreakCorutine();
	}

	private void DrainHealth(List<Collider2D> collidersHits)
	{
		foreach (Collider2D collidersHit in collidersHits)
		{
			if (collidersHit.TryGetComponent<Enemy>(out Enemy enemy))
			{
				Health health = enemy.GetComponent<Health>();
				health.TakeDamage(_value);
				_health.TakeHeal(_value);
			}
		}
	}

	private bool TryFindEnemies(out List<Collider2D> collidersHits)
	{
		_range.enabled = true;
		collidersHits = new();
		int colliderHitsCount = _range.OverlapCollider(_contactFilter2D, collidersHits);
		_range.enabled = false;
		return colliderHitsCount > 0;
	}
}