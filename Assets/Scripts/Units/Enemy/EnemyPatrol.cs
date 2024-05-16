using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FaceFliper))]
public class EnemyPatrol : MonoBehaviour
{
	[SerializeField] private float _speed;
	[SerializeField] private List<Transform> _wayPoints;

	private int _indexWayPoint;
	private Transform _wayPoint;
	private FaceFliper _faceFliper;

	private void Awake()
	{
		_indexWayPoint = 0;
		_wayPoint = _wayPoints[_indexWayPoint];
	}

	private void Start()
	{
		_faceFliper = GetComponent<FaceFliper>();
	}

	private void OnEnable()
	{
		RotateToTarget(_wayPoint.position);
	}

	private void FixedUpdate()
	{
		MoveToWayPoint();
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

	private void MakeNextPosition()
	{
		_indexWayPoint = ++_indexWayPoint % _wayPoints.Count;
		_wayPoint = _wayPoints[_indexWayPoint];
	}

	private void RotateToTarget(Vector2 targetToLook)
	{
		_faceFliper?.Flip(targetToLook.x - transform.position.x);
	}
}