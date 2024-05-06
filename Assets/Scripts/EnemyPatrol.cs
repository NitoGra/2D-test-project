using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(FaceFliper))]
public class EnemyPatrol : MonoBehaviour
{
	[SerializeField] private List<Transform> _wayPoints;
	[SerializeField] private float _speed;
	[SerializeField] private Mover _mover;

	[SerializeField] private Transform[] _froniViewPoints = new Transform[2];
	[SerializeField] private Transform[] _backViewPoints = new Transform[2];

	private int _index;
	private Transform _wayPoint;
	private FaceFliper _faceFliper;
	private GameObject _target;
	private Coroutine _targetLose;

	[SerializeField] private float _secondsHuntDelay = 3;
	[SerializeField] private float _secondsHuntCount = 0;

	public event Action EnemyAttackOrdered;
	public event Action LoseTargetOrdered;

	private void Start()
	{
		_faceFliper = GetComponent<FaceFliper>();
		_index = 0;
		_wayPoint = _wayPoints[_index];
	}

	private void OnEnable()
	{
		LoseTargetOrdered += LoseTarget;
	}

	private void OnDisable()
	{
		LoseTargetOrdered -= LoseTarget;
	}

	private void Update()
	{
		TryFindTarget();


		if (_target == null)
		{
			MoveToTargetWayPoint();
			return;
		}
	}

	private void TryFindTarget()
	{
		RaycastHit2D frontHit = Physics2D.Linecast(_froniViewPoints[0].position, _froniViewPoints[1].position);
		RaycastHit2D backhit = Physics2D.Linecast(_backViewPoints[0].position, _backViewPoints[1].position);

		Debug.DrawLine(_froniViewPoints[0].position, _froniViewPoints[1].position, Color.red, 5);
		Debug.DrawLine(_backViewPoints[0].position, _backViewPoints[1].position, Color.blue, 5);

		if (frontHit.collider != null)
		{
			if (frontHit.collider.TryGetComponent(out PlayerHealth playerHealth))
			{
				_secondsHuntCount = 0;
				_target = playerHealth.gameObject;
				RotateToTarget(_target.transform.position);
				AttackOrder();
			}
		}
		else if (backhit.collider != null)
		{
			if (backhit.collider.TryGetComponent(out PlayerHealth playerHealth))
			{
				_secondsHuntCount = 0;
				_target = playerHealth.gameObject;
				AttackOrder();
			}
		}
	}

	private void AttackOrder()
	{
		if (_target != null)
		{
			EnemyAttackOrdered?.Invoke();

			if (_targetLose == null)
			{
				_targetLose = StartCoroutine(HuntTarget());
			}
		}
	}

	private void MakeNextPosition()
	{
		_index = ++_index % _wayPoints.Count;
		_wayPoint = _wayPoints[_index];
	}

	private void RotateToTarget(Vector2 targetToLook)
	{
		print(transform.position.x + " lool at " + targetToLook);
		_faceFliper.Flip(targetToLook.x - transform.position.x);
	}

	private IEnumerator HuntTarget()
	{
		while (_secondsHuntDelay > _secondsHuntCount)
		{
			_secondsHuntCount++;
			Vector2 target = new Vector2(_target.transform.position.x + 2, transform.position.y + 2f);
			_mover.ImpulseMove(target);
			yield return new WaitForSecondsRealtime(1.0f);
		}

		LoseTargetOrdered?.Invoke();
		yield return null;
	}

	private void LoseTarget()
	{
		print("Цель потеряна!");
		StopCoroutine(_targetLose);
		_target = null;
		_targetLose = null;
	}

	private void MoveToTargetWayPoint()
	{
		RotateToTarget(_wayPoint.position);

		Vector2 target = new Vector2(_wayPoint.position.x, transform.position.y);
		transform.position = Vector2.MoveTowards(transform.position, target, _speed * Time.deltaTime);

		if (transform.position.x == _wayPoint.position.x)
			MakeNextPosition();
	}
}