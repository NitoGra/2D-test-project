using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAudio), typeof(Mover))]
public class Player : Unit
{
	private const string Horizontal = "Horizontal";
	private const KeyCode JumpKey = KeyCode.Space;
	private const KeyCode SitKey = KeyCode.S;
	private const KeyCode AttackKey = KeyCode.F;
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

	private PlayerAudio _audio;
	private Mover _mover;

	private Vector2 _moveVector;
	private bool _isAttack;
	private bool _isJump = false;
	private bool _isGrounded;
	private bool _isAlive = true;
	private bool _isAnimate = true;
	private bool _canMoving;
	private float _returnToNormalStateDelay = 1f;

	public event Action Siting;
	public event Action Jumping;
	public event Action Runing;
	public event Action Idleing;
	public event Action Attacking;

	protected override void Start()
	{
		base.Start();
		_mover = GetComponent<Mover>();
		_audio = GetComponent<PlayerAudio>();
	}

	private void Update()
	{
		if (_isAlive == false)
			return;

		if (_isAttack)
			return;

		PlayAnimation();
	}

	private void FixedUpdate()
	{
		_isGrounded = WasGrounded();
		_moveVector.x = Input.GetAxis(Horizontal);
		Jump();

		if (_canMoving)
			_mover.HorizontalMove(_moveVector * _speed * _speedMultiplier);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent(out Coin coin))
			TakeCoin(coin);
		else if (collision.gameObject.TryGetComponent(out MedicBag medicBag))
			TakeMedicBag(medicBag);
	}

	private void TakeMedicBag(MedicBag medicBag)
	{
		medicBag.Pickup(_audio, _audio.GetMedicBagSound);
		Health.Healing((int)medicBag.Value);
	}

	private void TakeCoin(Coin coin)
	{
		coin.Pickup(_audio, _audio.GetCoinSound);
	}

	private bool TryAttack()
	{
		if (Input.GetKey(AttackKey))
		{
			Attacking?.Invoke();
			_canMoving = false;
			_isAttack = true;
		}

		return _canMoving;
	}

	private bool TrySit()
	{
		if (Input.GetKey(SitKey))
		{
			Siting?.Invoke();
			_canMoving = false;
		}

		return _canMoving;
	}

	private bool WasGrounded()
	{
		bool isGroundHere = Physics2D.OverlapCircleAll(_groundTrigger.transform.position, _groundTrigger.radius, _groundMask).Length > 0;
		bool isEnemyHere = Physics2D.OverlapCircleAll(_groundTrigger.transform.position, _groundTrigger.radius, _enemyMask).Length > 0;
		return isEnemyHere || isGroundHere;
	}

	private void JumpOrder()
	{
		if (Input.GetKeyDown(JumpKey))
		{
			_isGrounded = false;
			_isJump = true;
		}
	}

	private void Jump()
	{
		if (_isJump)
		{
			_mover.ImpulseMove(transform.up * _jumpSpeed);
			_isJump = false;
		}
	}

	protected override void Die()
	{
		_isAlive = false;
		_canMoving = false;
		_audio.DeadSound();
	}

	protected override void GetHit()
	{
		_isAnimate = false;
		_isAttack = false;
		_canMoving = true;
		_audio.DamageSound();
		Invoke(nameof(DoNormalAnimateState), _returnToNormalStateDelay);
	}

	private void PlayAnimation()
	{
		FaceFliper.Flip(_moveVector.x);
		_canMoving = true;

		if (_isGrounded == false)
		{
			Jumping?.Invoke();
			return;
		}

		JumpOrder();

		if (_isAnimate == false)
			return;

		if (TryAttack() == false)
			return;

		if (TrySit() == false)
			return;

		if (_moveVector.x != 0)
			Runing?.Invoke();
		else
			Idleing?.Invoke();
	}

	private void DoNormalAnimateState() => _isAnimate = true;
	private void EndAttack() => _isAttack = false;
}