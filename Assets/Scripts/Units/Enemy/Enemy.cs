using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(FaceFliper), typeof(EnemyPatrol), typeof(Health))]
public class Enemy : MonoBehaviour
{
	[SerializeField] private Health _health;

	[SerializeField] private float _attackDistance;
	[SerializeField] private float _attackDelay;
	[SerializeField] private float _stunOnHitTime;
	[SerializeField] private float _huntEndDelay;
	[SerializeField] private float _huntSpeed;

	[SerializeField] private Transform[] _frontViewPoints = new Transform[2];
	[SerializeField] private Transform[] _backViewPoints = new Transform[2];

	private FaceFliper _faceFliper;
	private int _huntDistance;
	private float _timerToAttack;
	private float _huntRemembersCount = 0;
	private WaitForSecondsRealtime _rememberDelay = new(1.0f);

	private GameObject _target;
	private Vector2 _lastTargetPosition;
	private Coroutine _targetLose;
	private EnemyPatrol _enemyPatrol;

	private float _drawLineDelay = 0.1f;
	private Color _frontColor = Color.red;
	private Color _backColor = Color.blue;

	public event Action AttackOrdered;
	public event Action LoseTargetOrdered;

	private void Start()
	{
		_faceFliper = GetComponent<FaceFliper>();
		_enemyPatrol = GetComponent<EnemyPatrol>();
		_health = GetComponent<Health>();
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
		if (TryFindTarget())
			EndPatrol();

		if (_target == null)
			StartPatrol();
		else
			MoveToTarget();
	}

	private void StartPatrol()
	{
		_enemyPatrol.enabled = true;
	}

	private void EndPatrol()
	{
		_enemyPatrol.enabled = false;
	}

	private bool TryFindTarget()
	{
		if (TryFindCollider(_frontViewPoints, _frontColor, out Health playerHealth))
		{
			return true;
		}
		else if (TryFindCollider(_backViewPoints, _backColor, out playerHealth))
		{
			RotateToTarget(playerHealth.transform.position);
			return true;
		}

		return false;
	}

	private bool TryFindCollider(Transform[] _viewPoints, Color lineColor, out Health playerHealth)
	{
		Collider2D health = GetColliderOnLine(_viewPoints[0].position, _viewPoints[1].position, lineColor);

		if (health != null)
		{
			if (health.TryGetComponent(out playerHealth))
			{
				TargetFound(playerHealth);
				return true;
			}
		}

		playerHealth = null;
		return false;
	}

	private Collider2D GetColliderOnLine(Vector2 startLine, Vector2 endLine, Color lineColor)
	{
		Debug.DrawLine(startLine, endLine, lineColor, _drawLineDelay);

		Collider2D collider = Physics2D.Linecast(startLine, endLine).collider;

		if (collider?.name != gameObject.name)
			return collider;

		return null;
	}

	private void TargetFound(Health playerHealth)
	{
		_huntRemembersCount = 0;
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

	private void LoseTarget()
	{
		StopCoroutine(_targetLose);
		_target = null;
		_timerToAttack = 0;
		_targetLose = null;
	}

	private void MoveToTarget()
	{
		RotateToTarget(_target.transform.position);
		Vector2 target = new(_lastTargetPosition.x + _huntDistance, transform.position.y);
		transform.position = Vector2.MoveTowards(transform.position, target, _huntSpeed * Time.deltaTime);
	}

	private void RotateToTarget(Vector2 targetToLook)
	{
		_faceFliper.Flip(targetToLook.x - transform.position.x);
	}

	private IEnumerator RememberTarget()
	{
		while (_huntEndDelay > _huntRemembersCount)
		{
			_huntRemembersCount++;
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