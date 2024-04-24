using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyPatrol : MonoBehaviour
{
	[SerializeField] private List<Transform> _wayPoints;
	[SerializeField] private float _speed;

	private int _index;
	private Transform _wayPoint;
	private SpriteRenderer _spriteRenderer;

	private void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_index = 0;
		_wayPoint = _wayPoints[_index];
	}

	private void Update()
	{
		MoveToTargetWayPoint();

		if (transform.position == _wayPoint.position)
			MakeNextPosition();
	}

	private void MakeNextPosition()
	{
		_index = ++_index % _wayPoints.Count == 0 ? 0 : _index++;
		_wayPoint = _wayPoints[_index];
	}

	private void RotateToTarget()
	{
		if (transform.position.x < _wayPoint.position.x)
			_spriteRenderer.flipX = false;
		else
			_spriteRenderer.flipX = true;
	}

	private void MoveToTargetWayPoint()
	{
		RotateToTarget();
		transform.position = Vector3.MoveTowards(transform.position, _wayPoint.position, _speed * Time.deltaTime);
	}
}