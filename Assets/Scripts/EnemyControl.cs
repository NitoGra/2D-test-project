using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(FaceFliper))]
public class EnemyControl : MonoBehaviour
{
	[SerializeField] private List<Transform> _wayPoints;
	[SerializeField] private float _speed;
	[SerializeField] private Mover _mover;
	[SerializeField] private float _attackDistance;
	[SerializeField] private float _attackDelay;

	[SerializeField] private float _secondsHuntDelay;
	private float _timerToAttack;

	[SerializeField] private Transform[] _froniViewPoints = new Transform[2];
	[SerializeField] private Transform[] _backViewPoints = new Transform[2];

	private int _huntDistance;
	private float _secondsHuntCount = 0;

	private int _indexWayPoint;
	private Transform _wayPoint;

	private FaceFliper _faceFliper;
	private GameObject _target;
	private Coroutine _targetLose;

	public event Action EnemyAttackOrdered;
	public event Action LoseTargetOrdered;

	private void Start()
	{
		_faceFliper = GetComponent<FaceFliper>();
		_indexWayPoint = 0;
		_wayPoint = _wayPoints[_indexWayPoint];
	}

	private void OnEnable()
	{
		LoseTargetOrdered += LoseTarget;
	}

	private void OnDisable()
	{
		LoseTargetOrdered -= LoseTarget;
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
		RaycastHit2D frontHit = Physics2D.Linecast(_froniViewPoints[0].position, _froniViewPoints[1].position);
		RaycastHit2D backhit = Physics2D.Linecast(_backViewPoints[0].position, _backViewPoints[1].position);

		Debug.DrawLine(_froniViewPoints[0].position, _froniViewPoints[1].position, Color.red, 2);
		Debug.DrawLine(_backViewPoints[0].position, _backViewPoints[1].position, Color.blue, 2);

		if (frontHit.collider != null)
		{
			if (frontHit.collider.TryGetComponent(out PlayerHealth playerHealth))
			{
				_secondsHuntCount = 0;
				_target = playerHealth.gameObject;
				AttackOrder();
			}
		}
		else if (backhit.collider != null)
		{
			if (backhit.collider.TryGetComponent(out PlayerHealth playerHealth))
			{
				_secondsHuntCount = 0;
				_target = playerHealth.gameObject;
				RotateToTarget(_target.transform.position);
				AttackOrder();
			}
		}
	}

	private void AttackOrder()
	{
		_timerToAttack += Time.deltaTime;

		if (_target != null)
		{
			if (_timerToAttack > _attackDelay)
			{
				_timerToAttack = 0;
				float distance = Vector2.Distance(_target.transform.position, transform.position);

				if (distance <= _attackDistance)
					EnemyAttackOrdered?.Invoke();
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

	private void RotateToTarget(Vector2 targetToLook)
	{
		_faceFliper.Flip(targetToLook.x - transform.position.x);
	}

	private void LoseTarget()
	{
		print("Цель потеряна!");
		StopCoroutine(_targetLose);
		_target = null;
		_targetLose = null;
		RotateToTarget(_wayPoint.position);
	}

	private void MoveToWayPoint()
	{
		Vector2 target = new Vector2(_wayPoint.position.x, transform.position.y);
		transform.position = Vector2.MoveTowards(transform.position, target, _speed * Time.deltaTime);

		if (transform.position.x == _wayPoint.position.x)
		{
			MakeNextPosition();
			RotateToTarget(_wayPoint.position);
		}
	}

	private void MoveToTarget()
	{
		Vector2 target = new Vector2(_target.transform.position.x + _huntDistance, transform.position.y);
		RotateToTarget(target);
		transform.position = Vector2.MoveTowards(transform.position, target, _speed * Time.deltaTime);
	}

	private IEnumerator RememberTarget()
	{
		while (_secondsHuntDelay > _secondsHuntCount)
		{
			_secondsHuntCount++;
			int rangeHuntDistance = 4;
			_huntDistance = UnityEngine.Random.Range(rangeHuntDistance/2, rangeHuntDistance);
			yield return new WaitForSecondsRealtime(1.0f);
		}

		LoseTargetOrdered?.Invoke();
		yield return null;
	}
}