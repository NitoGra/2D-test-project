using System;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(SpriteRenderer), typeof(FaceFliper))]
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

	private FaceFliper _faceFliper;
	private Mover _mover;
	private Vector2 _moveVector;
	private bool _isGrounded;
	private bool _isSit;

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

		if (_isGrounded)
		{
			if (Input.GetKeyDown(JumpKey))
			{
				_mover.ImpulseMove(transform.up * _jumpSpeed);
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

		if (_isGrounded)
			if (_moveVector.x != 0)
				RunOrdered?.Invoke();
			else
				IdleOrdered?.Invoke();

		_mover.HorizontalMove(_moveVector * _speed);
	}
	/*
	private void FixedUpdate()
	{
		if (_isGrounded)
			if (_moveVector.x != 0)
				RunOrdered?.Invoke();
			else
				IdleOrdered?.Invoke();

		_mover.HorizontalMove(_moveVector.x * _speed);
	}*/

	/*
	if (_moveVector.x > 0)
		_spriteRenderer.flipX = false;
	else if (_moveVector.x < 0)
		_spriteRenderer.flipX = true;*/

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