using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAudio), typeof(Mover), typeof(Health))]
public class Player : Unit
{
	private const float MinSpeed = 1.0f;
	private const float MaxSpeed = 100.0f;

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

	private Mover _mover;

	public bool IsGrounded { get; private set; }

	protected override void Start()
	{
		base.Start();
		_mover = GetComponent<Mover>();
	}

	private void FixedUpdate()
	{
		IsGrounded = WasGrounded();
		DoFlip();
	}

	private void OnEnable()
	{
		_keyDetect.Jumping += Jump;
		_keyDetect.Runing += Run;
	}

	private void OnDisable()
	{
		_keyDetect.Jumping -= Jump;
		_keyDetect.Runing -= Run;
	}

	private void Run()
	{
		_mover.HorizontalMove(_speed * _speedMultiplier * _keyDetect.GetMoveVector());
	}

	private void Jump()
	{
		if(IsGrounded)
			_mover.ImpulseMove(transform.up * _jumpSpeed);
	}

	private bool WasGrounded()
	{
		bool isGroundHere = Physics2D.OverlapCircleAll(_groundTrigger.transform.position, _groundTrigger.radius, _groundMask).Length > 0;
		bool isEnemyHere = Physics2D.OverlapCircleAll(_groundTrigger.transform.position, _groundTrigger.radius, _enemyMask).Length > 0;
		return isEnemyHere || isGroundHere;
	}

	private void DoFlip()
	{
		FaceFliper.Flip(_keyDetect.GetMoveVector().x);
	}
}