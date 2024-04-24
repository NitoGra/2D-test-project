using System;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(SpriteRenderer))]
public class MotionControl : MonoBehaviour
{
	private const string Horizontal = "Horizontal";
	private const KeyCode JumpKey = KeyCode.W;
	private const KeyCode SitKey = KeyCode.S;

	[SerializeField] private float _speed;
	[SerializeField] private float _jumpSpeed;
	[SerializeField] private LayerMask _groundMask;
	[SerializeField] private CircleCollider2D _groundTrigger;
	[SerializeField] private AudioSource _coinSound;

	private Mover _mover;
	private Vector2 _moveVector;
	private SpriteRenderer _spriteRenderer;
	private bool _isGrounded;
	private bool _isJumped;

	public event Action SitOrdered;
	public event Action JumpOrdered;
	public event Action RunOrdered;
	public event Action IdleOrdered;

	private void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_mover = GetComponent<Mover>();
	}

	private void Update()
	{
		_isGrounded = WasGrounded();

		if (_isGrounded)
		{
			if (Input.GetKeyDown(JumpKey))
			{
				_isJumped = true;
			}

			if (Input.GetKey(SitKey))
			{
				SitOrdered?.Invoke();
				return;
			}
		}
		else
		{
			JumpOrdered?.Invoke();
		}

		_moveVector.x = Input.GetAxis(Horizontal);
	}

	private void FixedUpdate()
	{
		if (_isJumped)
		{
			_mover.ImpulseMove(transform.up * _jumpSpeed);
			_isJumped = false;
		}

		if (_isGrounded && _moveVector.x != 0)
			RunOrdered?.Invoke();

		if (_moveVector.x > 0)
			_spriteRenderer.flipX = false;
		else if (_moveVector.x < 0)
			_spriteRenderer.flipX = true;
		else if (_isGrounded)
			IdleOrdered?.Invoke();

		_mover.HorizontalMove(_moveVector.x * _speed);
	}

	private bool WasGrounded()
	{
		return Physics2D.OverlapCircleAll(_groundTrigger.transform.position, _groundTrigger.radius, _groundMask).Length > 0;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.TryGetComponent<Coin>(out Coin coin))
		{
			coin.PickUp();
			_coinSound.Play();
		}
	}
}