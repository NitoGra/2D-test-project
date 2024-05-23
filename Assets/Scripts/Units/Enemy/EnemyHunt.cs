using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(FaceFliper), typeof(Enemy))]
public class EnemyHunt : MonoBehaviour
{
	[SerializeField] private float _attackDistance;
	[SerializeField] private float _attackDelay;
	[SerializeField] private float _stunOnHitTime;
	[SerializeField] private float _huntEndDelay;
	[SerializeField] private float _huntSpeed;
	[SerializeField] private Health _health;

	public event Action AttackOrdered;
	public event Action LoseTargetOrdered;

	private int _huntDistance;
	private float _huntRemembersCount = 0;
	private float _timerToAttack = 0;
	private WaitForSecondsRealtime _rememberDelay = new(1.0f);
	private WaitForSecondsRealtime _attackPartDelay = new(1.0f);

	private Vector2 _lastTargetPosition;
	private Coroutine _targetLose;
	private Coroutine _keepAttack;

	private Enemy _enemy;
	private FaceFliper _faceFliper;
	private GameObject _target;
	private bool _isAlive = true;

	private void Start()
	{
		_enemy = GetComponent<Enemy>();
		_faceFliper = GetComponent<FaceFliper>();
	}

	private void OnEnable()
	{
		_health.Damaged += GetHit;
		_health.Died+= Die;
		LoseTargetOrdered += LoseTarget;
	}

	private void OnDisable()
	{
		_health.Damaged -= GetHit;
		_health.Died -= Die;
		LoseTargetOrdered -= LoseTarget;
	}

	private void FixedUpdate()
	{
		if (_isAlive)
		{
			FoundTarget();
			MoveToTarget();
		}
	}

	private void FoundTarget()
	{
		_target = _enemy.Target;

		if (_target != null)
		{
			_huntRemembersCount = 0;

			if (_keepAttack == null)
				_keepAttack = StartCoroutine(AttackTarget());

			if (_targetLose == null)
				_targetLose = StartCoroutine(RememberTarget());
		}
	}

	private void Die() => _isAlive = false;

	private IEnumerator AttackTarget()
	{
		while (_timerToAttack < _attackDelay)
		{
			if (_target != null)
				_lastTargetPosition = _target.transform.position;

			_timerToAttack++;
			yield return _attackPartDelay;
		}

		AttackOrdered?.Invoke();
		_timerToAttack = 0;
		StopCoroutine(_keepAttack);
		_keepAttack = null;
	}

	private void LoseTarget()
	{
		StopCoroutine(_targetLose);
		_timerToAttack = 0;
		_targetLose = null;
		_enemy.enabled = true;
	}

	private void MoveToTarget()
	{
		if (_target != null)
			RotateToTarget(_target.transform.position);

		Vector2 target = new(_lastTargetPosition.x + _huntDistance, transform.position.y);
		transform.position = Vector2.MoveTowards(transform.position, target, _huntSpeed * Time.deltaTime);
	}

	private IEnumerator RememberTarget()
	{
		while (_huntRemembersCount < _huntEndDelay)
		{
			_huntRemembersCount++;
			_huntDistance = -UnityEngine.Random.Range(0, (int)_attackDistance);
			yield return _rememberDelay;
		}

		LoseTargetOrdered?.Invoke();
	}

	private void RotateToTarget(Vector2 targetToLook)
	{
		_faceFliper.Flip(targetToLook.x - transform.position.x);
	}

	private void GetHit()
	{
		_timerToAttack -= _stunOnHitTime;
	}
}