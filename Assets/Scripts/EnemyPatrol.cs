using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(FaceFliper))]
public class EnemyPatrol : MonoBehaviour
{
	[SerializeField] private List<Transform> _wayPoints;
	[SerializeField] private float _speed;
	[SerializeField] private LayerMask _playerLayer;
	[SerializeField] private float _viewingDistance; //4

	private int _index;
	private Transform _wayPoint;
	private FaceFliper _faceFliper;
	private GameObject _target;
	private Coroutine _targetLose;
	private float _secondsDelay = 3;
	private float _secondsCount = 0;

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
	/*
	private void OnDisable()
	{
		LoseTargetOrdered -= LoseTarget;
	}
	*/
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
		Vector3 origin = new Vector3(transform.position.x + 0.2f, transform.position.y + 0.5f, transform.position.z);
		Vector2 direction = transform.right;

		RaycastHit2D hit = Physics2D.Raycast(origin, direction, _viewingDistance);
		Debug.DrawRay(origin, direction, Color.red, _viewingDistance, true);

		if (hit.collider != _target)
		{
			if (hit.collider.TryGetComponent(out PlayerHealth playerHealth))
			{
				_secondsCount = 0;
				_target = playerHealth.gameObject;
				EnemyAttackOrdered?.Invoke();

				if (_targetLose == null)
				{
					_targetLose = StartCoroutine(HuntTarget());
				}
			}
		}
	}

	private void MakeNextPosition()
	{
		_index = ++_index % _wayPoints.Count;
		_wayPoint = _wayPoints[_index];
	}

	private void RotateToTarget()
	{
		_faceFliper.Flip(_wayPoint.position.x - transform.position.x);
	}

	private IEnumerator HuntTarget()
	{
		while (_secondsDelay > _secondsCount)
		{
			_secondsCount++;
			print(_secondsCount + " секунд прошло");

			Vector2 target = new Vector2(_target.transform.position.x, transform.position.y);
			transform.position = Vector2.MoveTowards(transform.position, target, _speed * Time.deltaTime);

			yield return new WaitForSecondsRealtime(1.0f);
		}

		LoseTargetOrdered?.Invoke();
		yield return null;
	}

	private void LoseTarget()
	{
		print("Цель потеряна!");
		_target = null;
		StopCoroutine(_targetLose);
		_targetLose = null;
		LoseTargetOrdered -= LoseTarget;
	}

	private void MoveToTargetWayPoint()
	{
		RotateToTarget();

		Vector2 target = new Vector2(_wayPoint.position.x, transform.position.y);
		transform.position = Vector2.MoveTowards(transform.position, target, _speed * Time.deltaTime);

		if (transform.position.x == _wayPoint.position.x)
			MakeNextPosition();
	}
}