using UnityEngine;

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

	public bool IsGrounded => WasGrounded();

	private void Start()
	{
		_groundTrigger = GetComponentInChildren<CircleCollider2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_animator = GetComponent<PlayerAnimator>();
		_mover = GetComponent<Mover>();
	}

	private void Update()
	{
		bool isGrounded = IsGrounded;

		if (isGrounded)
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

		if (isGrounded && _moveVector.x != 0)
			_animator.PlayRun();

		if (_moveVector.x > 0)
			_spriteRenderer.flipX = false;
		else if (_moveVector.x < 0)
			_spriteRenderer.flipX = true;
		else if (isGrounded)
			_animator.PlayIdle();

		_mover.HorizontalMove(_moveVector.x * _speed);
	}

	private bool WasGrounded()
	{
		return Physics2D.OverlapCircleAll(_groundTrigger.transform.position, _groundTrigger.radius, _groundMask).Length > 0;
	}
}