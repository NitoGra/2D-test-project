using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(FaceFliper))]
public class EnemyPatrol : MonoBehaviour
{
	[SerializeField] private List<Transform> _wayPoints;
	[SerializeField] private float _speed;

	private int _index;
	private Transform _wayPoint;
	private FaceFliper _faceFliper;

	private void Start()
	{
		_faceFliper = GetComponent<FaceFliper>();
		_index = 0;
		_wayPoint = _wayPoints[_index];
	}

	private void Update()
	{
		MoveToTargetWayPoint();

		if (transform.position.x == _wayPoint.position.x)
			MakeNextPosition();
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

	private void MoveToTargetWayPoint()
	{
		RotateToTarget();

		Vector2 target = new Vector2(_wayPoint.position.x, transform.position.y);
		transform.position = Vector2.MoveTowards(transform.position, target, _speed * Time.deltaTime);
	}
}