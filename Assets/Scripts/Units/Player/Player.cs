using System;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(Health))]
public class Player : MonoBehaviour
{
	[Range(MinSpeed, MaxSpeed)]
	[SerializeField] private float _speed;
	[Range(MinSpeed, MaxSpeed)]
	[SerializeField] private float _speedMultiplier;
	[Range(MinSpeed, MaxSpeed)]
	[SerializeField] private float _jumpSpeed;
	[SerializeField] private LayerMask _groundMask;
	[SerializeField] private LayerMask _enemyMask;
	[SerializeField] private CircleCollider2D _groundTrigger;
	[SerializeField] private KeyDetect _keyDetect;

	private const float MinSpeed = 1.0f;
	private const float MaxSpeed = 100.0f;

	private Mover _mover;

	public bool IsGrounded { get; private set; }

	private void Start()
	{
		_mover = GetComponent<Mover>();
	}

	private void FixedUpdate()
	{
		IsGrounded = TryFindGround();
	}

	private void OnEnable()
	{
		_keyDetect.Jumped += Jump;
		_keyDetect.Runned += Run;
	}

	private void OnDisable()
	{
		_keyDetect.Jumped -= Jump;
		_keyDetect.Runned -= Run;
	}

	private void Run()
	{
		_mover.MoveHorizontal(_speed * _speedMultiplier * _keyDetect.GetMoveVector());
	}

	private void Jump()
	{
		if (IsGrounded)
			_mover.MoveImpulse(transform.up * _jumpSpeed);
	}

	private bool TryFindGround()
	{
		bool isGroundHere = Physics2D.OverlapCircleAll(_groundTrigger.transform.position, _groundTrigger.radius, _groundMask).Length > 0;
		bool isEnemyHere = Physics2D.OverlapCircleAll(_groundTrigger.transform.position, _groundTrigger.radius, _enemyMask).Length > 0;
		return isEnemyHere || isGroundHere;
	}
}