using UnityEngine;

public class MotionControl : MonoBehaviour
{
	private const string Horizontal = "Horizontal";
	private const KeyCode JumpKey = KeyCode.W;
	private const KeyCode SitKey = KeyCode.S;

	[SerializeField] private float _speed;
	[SerializeField] private Animator _animator;
	[SerializeField] private float _jumpSpeed;
	[SerializeField] private LayerMask _groundMask;

	private Vector2 _moveVector;
	private CircleCollider2D _groundTrigger;
	private Rigidbody2D _rigidbody2D;
	private SpriteRenderer _spriteRenderer;

	public bool IsGrounded => WasGrounded();

	private void Start()
	{
		_groundTrigger = GetComponentInChildren<CircleCollider2D>();
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if(IsGrounded)
		{
			if (Input.GetKeyUp(JumpKey))
			{
				_rigidbody2D.AddForce(transform.up * _jumpSpeed, ForceMode2D.Impulse);
				_animator.Play(PlayerAnimatorData.Params.jump);
			}

			if (Input.GetKey(SitKey))
			{
				_animator.Play(PlayerAnimatorData.Params.sit);
				return;
			}
		}
		else
			_animator.Play(PlayerAnimatorData.Params.jump);

		_moveVector.x = Input.GetAxis(Horizontal);

		if (IsGrounded && _moveVector.x != 0)
			_animator.Play(PlayerAnimatorData.Params.run);
		
		if (_moveVector.x > 0)
			_spriteRenderer.flipX = false;
		else if (_moveVector.x < 0)
			_spriteRenderer.flipX = true;
		else if(IsGrounded)
			_animator.Play(PlayerAnimatorData.Params.stayIdle);

		_rigidbody2D.velocity = new Vector2(Input.GetAxis(Horizontal) * _speed, _rigidbody2D.velocity.y);
	}

	private bool WasGrounded()
	{
		return Physics2D.OverlapCircleAll(_groundTrigger.transform.position, _groundTrigger.radius, _groundMask).Length > 0;
	}
}

public static class PlayerAnimatorData
{ 
	public static class Params
	{
		public static readonly int jump = Animator.StringToHash(nameof(jump));
		public static readonly int run = Animator.StringToHash(nameof(run));
		public static readonly int stayIdle = Animator.StringToHash(nameof(stayIdle));
		public static readonly int sit = Animator.StringToHash(nameof(sit));

	}
}