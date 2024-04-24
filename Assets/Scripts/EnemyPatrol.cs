using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
		_index = ++_index % _wayPoints.Count;
		_wayPoint = _wayPoints[_index];
	}

	private void RotateToTarget()
	{
		_spriteRenderer.flipX = transform.position.x >= _wayPoint.position.x;
	}

	private void MoveToTargetWayPoint()
	{
		RotateToTarget();
		transform.position = Vector3.MoveTowards(transform.position, _wayPoint.position, _speed * Time.deltaTime);
	}
}