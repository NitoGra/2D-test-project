using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Scene), typeof(PlayerAudio), typeof(Mover))]
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
	private bool _isIAlive = true;
	private bool _isIAnimate = true;
	private bool _canIMoving;
	private float _returnToNormalStateDelay = 1f;
	private float _restartSceneDelay = 5f;

	public event Action SitOrdered;
	public event Action JumpOrdered;
	public event Action RunOrdered;
	public event Action IdleOrdered;
	public event Action AttackOrdered;

	protected override void Start()
	{
		base.Start();
		_mover = GetComponent<Mover>();
		_audio = GetComponent<PlayerAudio>();
	}

	private void Update()
	{
		if (_isIAlive == false)
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

		if (_canIMoving)
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
		medicBag.Pickup(_audio);
		Health.Healing((int)medicBag.Value);
	}

	private void TakeCoin(Coin coin)
	{
		coin.Pickup(_audio);
	}

	private bool TryAttack()
	{
		if (Input.GetKey(AttackKey))
		{
			AttackOrdered?.Invoke();
			_canIMoving = false;
			_isAttack = true;
		}

		return _canIMoving;
	}

	private bool TrySit()
	{
		if (Input.GetKey(SitKey))
		{
			SitOrdered?.Invoke();
			_canIMoving = false;
		}

		return _canIMoving;
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
		_isIAlive = false;
		_canIMoving = false;
		_audio.DeadSound();
		Invoke(nameof(RestartScene), _restartSceneDelay);
	}

	protected override void GetHit()
	{
		_isIAnimate = false;
		_isAttack = false;
		_canIMoving = true;
		_audio.DamageSound();
		Invoke(nameof(DoNormalAnimateState), _returnToNormalStateDelay);
	}

	private void PlayAnimation()
	{
		FaceFliper.Flip(_moveVector.x);
		_canIMoving = true;

		if (_isGrounded == false)
		{
			JumpOrdered?.Invoke();
			return;
		}

		JumpOrder();

		if (_isIAnimate == false)
			return;

		if (TryAttack() == false)
			return;

		if (TrySit() == false)
			return;

		if (_moveVector.x != 0)
			RunOrdered?.Invoke();
		else
			IdleOrdered?.Invoke();
	}

	private void DoNormalAnimateState() => _isIAnimate = true;
	private void EndAttack() => _isAttack = false;
	private void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}