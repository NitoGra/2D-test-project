using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MotionControl : MonoBehaviour
{
	private const string Horizontal = "Horizontal";
	private const KeyCode JumpKey = KeyCode.W;

	[SerializeField] private float _speed;
	[SerializeField] private Animator _animator;
	[SerializeField] private float _jumpSpeed;
	[SerializeField] private LayerMask _groundMask;

	private CircleCollider2D _groundTrigger;
	private Rigidbody2D _rigidbody2D;
	private Vector2 _moveVector;
	private SpriteRenderer _spriteRenderer;
	public bool IsGrounded => WasGrounded();

	private bool WasGrounded()
	{
		return Physics2D.OverlapCircleAll(_groundTrigger.transform.position, _groundTrigger.radius, _groundMask).Length > 0;
	}

	private void Start()
	{
		_groundTrigger = GetComponentInChildren<CircleCollider2D>();
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (Input.GetKeyUp(JumpKey) && IsGrounded)
		{
			_rigidbody2D.AddForce(transform.up * _jumpSpeed, ForceMode2D.Impulse);
			_animator.Play(PlayerAnimatorData.Params.jump);
		}
	}

	private void FixedUpdate()
	{
		_moveVector.x = Input.GetAxis(Horizontal);

		if (IsGrounded && _moveVector.x != 0)
		{
			_animator.Play(PlayerAnimatorData.Params.run);
		}
		else if(IsGrounded == false)
		{
			_animator.Play(PlayerAnimatorData.Params.jump);
			return;
		}

		if (_moveVector.x > 0)
			_spriteRenderer.flipX = false;
		else if (_moveVector.x < 0)
			_spriteRenderer.flipX = true;
		else
			_animator.Play(PlayerAnimatorData.Params.stayIdle);

		_rigidbody2D.velocity = new Vector2(Input.GetAxis("Horizontal") * _speed, _rigidbody2D.velocity.y);
	}
}

public static class PlayerAnimatorData
{ 
	public static class Params
	{
		public static readonly int jump = Animator.StringToHash(nameof(jump));
		public static readonly int run = Animator.StringToHash(nameof(run));
		public static readonly int stayIdle = Animator.StringToHash(nameof(stayIdle));

	}
}