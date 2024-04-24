using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(Mover))]
public class MotionControl : MonoBehaviour
{
	private const string Horizontal = "Horizontal";
	private const KeyCode JumpKey = KeyCode.W;
	private const KeyCode SitKey = KeyCode.S;

	[SerializeField] private float _speed;
	[SerializeField] private float _jumpSpeed;
	[SerializeField] private LayerMask _groundMask;

	private PlayerAnimator _animator;
	private Mover _mover;
	private Vector2 _moveVector;
	private CircleCollider2D _groundTrigger;
	private SpriteRenderer _spriteRenderer;
	private bool _isGrounded;

	private void Start()
	{
		_groundTrigger = GetComponentInChildren<CircleCollider2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_animator = GetComponent<PlayerAnimator>();
		_mover = GetComponent<Mover>();
	}

	private void Update()
	{
		_isGrounded = WasGrounded();

		if (_isGrounded)
		{
			if (Input.GetKeyDown(JumpKey))
			{
				_mover.ImpulseMove(transform.up * _jumpSpeed);
			}

			if (Input.GetKey(SitKey))
			{
				_animator.PlaySit();
				return;
			}
		}
		else
		{
			_animator.PlayJump();
		}

		_moveVector.x = Input.GetAxis(Horizontal);

		if (_isGrounded && _moveVector.x != 0)
			_animator.PlayRun();

		if (_moveVector.x > 0)
			_spriteRenderer.flipX = false;
		else if (_moveVector.x < 0)
			_spriteRenderer.flipX = true;
		else if (_isGrounded)
			_animator.PlayIdle();

		_mover.HorizontalMove(_moveVector.x * _speed);
	}

	private bool WasGrounded()
	{
		return Physics2D.OverlapCircleAll(_groundTrigger.transform.position, _groundTrigger.radius, _groundMask).Length > 0;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.TryGetComponent<Coin>(out Coin coin))
			coin.Taked();
	}
}