using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
	[SerializeField] private float _speed;
	[SerializeField] private float _attackDistance;
	[SerializeField] private float _attackDelay;

	[SerializeField] private float _stunOnHitTime;
	[SerializeField] private float _secondsHuntDelay;
	[SerializeField] private Health _health;
	[SerializeField] private List<Transform> _wayPoints;

	[SerializeField] private Transform[] _froniViewPoints = new Transform[2];
	[SerializeField] private Transform[] _backViewPoints = new Transform[2];

	private float _timerToAttack;
	private int _huntDistance;
	private float _secondsHuntCount = 0;
	private WaitForSecondsRealtime _rememberDelay = new WaitForSecondsRealtime(1.0f);

	private GameObject _target;
	private Vector2 _lastTargetPosition;
	private Coroutine _targetLose;

	private int _indexWayPoint;
	private Transform _wayPoint;

	public event Action AttackOrdered;
	public event Action LoseTargetOrdered;

	protected override void Start()
	{
		base.Start();
		_indexWayPoint = 0;
		_wayPoint = _wayPoints[_indexWayPoint];
		_health.GetComponent<Health>();
	}

	private void OnEnable()
	{
		LoseTargetOrdered += LoseTarget;
		_health.Damaging += GetHit;
	}

	private void OnDisable()
	{
		LoseTargetOrdered -= LoseTarget;
		_health.Damaging -= GetHit;
	}

	private void FixedUpdate()
	{
		TryFindTarget();

		if (_target == null)
			MoveToWayPoint();
		else
			MoveToTarget();
	}

	private void TryFindTarget()
	{
		Color frontColor = Color.red;
		Color backColor = Color.blue;
		Collider2D health = GetColliderOnLine(_froniViewPoints[0].position, _froniViewPoints[1].position, frontColor);

		if (health != null)
		{
			if (health.TryGetComponent(out Health playerHealth))
			{
				TargetFound(playerHealth);
				return;
			}
		}

		health = GetColliderOnLine(_backViewPoints[0].position, _backViewPoints[1].position, backColor);

		if (health != null)
		{
			if (health.TryGetComponent(out Health playerHealth))
			{
				RotateToTarget(playerHealth.transform.position);
				TargetFound(playerHealth);
				return;
			}
		}
	}

	private Collider2D GetColliderOnLine(Vector2 startLine, Vector2 endLine, Color lineColor)
	{
		float drawLineDelay = 0.7f;
		Debug.DrawLine(startLine, endLine, lineColor, drawLineDelay);
		return Physics2D.Linecast(startLine, endLine).collider;
	}

	private void TargetFound(Health playerHealth)
	{
		_secondsHuntCount = 0;
		_target = playerHealth.gameObject;
		AttackOrder();
	}

	private void AttackOrder()
	{
		_timerToAttack += Time.deltaTime;

		if (_target != null)
		{
			_lastTargetPosition = _target.transform.position;

			if (_timerToAttack > _attackDelay)
			{
				float distance = Vector2.Distance(_target.transform.position, transform.position);

				if (distance <= _attackDistance)
				{
					RotateToTarget(_target.transform.position);
					AttackOrdered?.Invoke();
					_timerToAttack = 0;
				}
			}

			if (_targetLose == null)
				_targetLose = StartCoroutine(RememberTarget());
		}
	}

	private void MakeNextPosition()
	{
		_indexWayPoint = ++_indexWayPoint % _wayPoints.Count;
		_wayPoint = _wayPoints[_indexWayPoint];
	}

	private void LoseTarget()
	{
		StopCoroutine(_targetLose);
		_target = null;
		_timerToAttack = 0;
		_targetLose = null;
		RotateToTarget(_wayPoint.position);
	}

	private void MoveToWayPoint()
	{
		Vector2 target = new(_wayPoint.position.x, transform.position.y);
		transform.position = Vector2.MoveTowards(transform.position, target, _speed * Time.deltaTime);

		if (transform.position.x == _wayPoint.position.x)
		{
			MakeNextPosition();
			RotateToTarget(_wayPoint.position);
		}
	}

	private void MoveToTarget()
	{
		RotateToTarget(_target.transform.position);
		Vector2 target = new(_lastTargetPosition.x + _huntDistance, transform.position.y);
		transform.position = Vector2.MoveTowards(transform.position, target, _speed * Time.deltaTime);
	}

	private IEnumerator RememberTarget()
	{
		while (_secondsHuntDelay > _secondsHuntCount)
		{
			_secondsHuntCount++;
			_huntDistance = -UnityEngine.Random.Range(0, (int)_attackDistance);
			yield return _rememberDelay;
		}

		LoseTargetOrdered?.Invoke();
		yield return null;
	}

	private void GetHit()
	{
		_timerToAttack -= _stunOnHitTime;
	}
}