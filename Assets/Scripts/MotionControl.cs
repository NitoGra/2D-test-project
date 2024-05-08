using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Mover), typeof(SpriteRenderer), typeof(FaceFliper))]
public class MotionControl : MonoBehaviour
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
	[SerializeField] private PlayerHealth _playerHealth;

	[SerializeField] private AudioSource _audio;
	[SerializeField] private AudioClip _medicBagSound;
	[SerializeField] private AudioClip _coinSound;
	[SerializeField] private AudioClip _damageSound;
	[SerializeField] private AudioClip _deadSound;

	private float _damageVolume = 0.5f;
	private float _normalVolume = 1;

	private FaceFliper _faceFliper;
	private Mover _mover;
	private Vector2 _moveVector;
	private bool _isAttack;
	private bool _isGrounded;
	private bool _canMoving;
	private bool _isIAlive = true;
	private bool _isIAnimate = true;
	private int _medicBagHealing = 2;
	private float _damageDelay = 1f;
	private float _restartSceneDelay = 5f;

	public event Action SitOrdered;
	public event Action JumpOrdered;
	public event Action RunOrdered;
	public event Action IdleOrdered;
	public event Action AttackOrdered;

	private void Start()
	{
		_faceFliper = GetComponent<FaceFliper>();
		_mover = GetComponent<Mover>();
		_playerHealth = GetComponent<PlayerHealth>();
	}

	private void Update()
	{
		if (_isIAlive == false)
			return;

		if (_isAttack)
			return;

		_isGrounded = WasGrounded();
		_faceFliper.Flip(_moveVector.x);
		_canMoving = true;

		if (_isGrounded == false)
		{
			JumpOrdered?.Invoke();
			return;
		}

		Jump();

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

	private void FixedUpdate()
	{
		_moveVector.x = Input.GetAxis(Horizontal);

		if (_canMoving)
			_mover.HorizontalMove(_moveVector * _speed * _speedMultiplier);
	}

	private void OnEnable()
	{
		_playerHealth.DamageTakeOrderd += Damage;
		_playerHealth.DeadOrdered += Dead;
	}

	private void OnDisable()
	{
		_playerHealth.DamageTakeOrderd -= Damage;
		_playerHealth.DeadOrdered -= Dead;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent<Coin>(out Coin coin))
			TakeCoin(coin);
		else if (collision.gameObject.TryGetComponent<MedicBag>(out MedicBag medicBag))
			TakeMedicBag(medicBag);
	}

	private bool TryAttack()
	{
		if (Input.GetKey(AttackKey))
		{
			AttackOrdered?.Invoke();
			_canMoving = false;
			_isAttack = true;
		}

		return _canMoving;
	}

	private void EndAttack()
	{
		_isAttack = false;
	}

	private bool TrySit()
	{
		if (Input.GetKey(SitKey))
		{
			SitOrdered?.Invoke();
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

	private void Jump()
	{
		if (Input.GetKeyDown(JumpKey))
		{
			_isGrounded = false;
			_mover.ImpulseMove(transform.up * _jumpSpeed);
		}
	}

	private void Dead()
	{
		_isIAlive = false;
		_canMoving = false;
		_audio.clip = _deadSound;
		_audio.Play();
		Invoke(nameof(RestartScene),_restartSceneDelay);
	}

	private void Damage()
	{
		_isIAnimate = false;
		_canMoving = true;
		_isAttack = false;
		_audio.clip = _damageSound;
		_audio.volume = _damageVolume;
		_audio.Play();
		Invoke(nameof(NormalState), _damageDelay);
	}

	private void NormalState()
	{
		_audio.volume = _normalVolume;
		_isIAnimate = true;
	}

	private void TakeMedicBag(MedicBag medicBag)
	{
		medicBag.PickUp();
		_audio.clip = _medicBagSound;
		_audio.Play();
		_playerHealth.Healing(_medicBagHealing);
	}

	private void TakeCoin(Coin coin)
	{
		coin.PickUp();
		_audio.clip = _coinSound;
		_audio.Play();
	}

	private void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}