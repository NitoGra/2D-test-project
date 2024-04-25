using System;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(SpriteRenderer), typeof(FaceFliper))]
public class MotionControl : MonoBehaviour
{
	private const string Horizontal = "Horizontal";
	private const KeyCode JumpKey = KeyCode.W;
	private const KeyCode SitKey = KeyCode.S;
	private const float MinSpeed = 1.0f;
	private const float MaxSpeed = 100.0f;

	[Range(MinSpeed, MaxSpeed)]
	[SerializeField] private float _speed;
	[SerializeField] private float _speedMultiplier;
	[SerializeField] private float _jumpSpeed;
	[SerializeField] private LayerMask _groundMask;
	[SerializeField] private CircleCollider2D _groundTrigger;
	[SerializeField] private AudioSource _coinSound;

	private FaceFliper _faceFliper;
	private Mover _mover;
	private Vector2 _moveVector;
	private bool _isGrounded;
	private bool _canMoving;

	public event Action SitOrdered;
	public event Action JumpOrdered;
	public event Action RunOrdered;
	public event Action IdleOrdered;

	private void Start()
	{
		_faceFliper = GetComponent<FaceFliper>();
		_mover = GetComponent<Mover>();
	}

	private void Update()
	{
		_faceFliper.Flip(_moveVector.x);
		_isGrounded = WasGrounded();
		_canMoving = true;

		if (_isGrounded == false)
		{
			JumpOrdered?.Invoke();
			return;
		}

		Jump();

		if (TrySit() == false)
			return;

		if (_moveVector.x != 0)
			RunOrdered?.Invoke();
		else
			IdleOrdered?.Invoke();
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

	private void Jump()
	{
		if (Input.GetKeyDown(JumpKey))
		{
			_mover.ImpulseMove(transform.up * _jumpSpeed);
		}
	}

	private void FixedUpdate()
	{
		_moveVector.x = Input.GetAxis(Horizontal);

		if (_canMoving)
			_mover.HorizontalMove(_moveVector * _speed * _speedMultiplier);
	}

	private bool WasGrounded()
	{
		return Physics2D.OverlapCircleAll(_groundTrigger.transform.position, _groundTrigger.radius, _groundMask).Length > 0;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent<Coin>(out Coin coin))
		{
			coin.PickUp();
			_coinSound.Play();
		}
	}
}