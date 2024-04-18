using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MotionControl : MonoBehaviour
{
	[SerializeField] private float _speed;
	[SerializeField] private Animator _animator;
	[SerializeField] private float _jumpSpeed;
	[SerializeField] private LayerMask _groundMask;
	[SerializeField] private CircleCollider2D _groundTrigger;
	[SerializeField] private Rigidbody2D _rigidbody2D;

	private Vector2 _moveVector;
	private SpriteRenderer _spriteRenderer;

	public bool IsGrounded => WasGrounded();

	private bool WasGrounded()
	{
		return Physics2D.OverlapCircleAll(_groundTrigger.transform.position, _groundTrigger.radius, _groundMask).Length > 0;
	}

	private void Start()
	{
		//_groundTrigger = GetComponentInChildren<CircleCollider2D>();
		//_rigidbody2D = GetComponent<Rigidbody2D>();
		_animator.Play("stayIdle");
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.W) && IsGrounded)
		{
			Debug.Log("Ïðûæîê");
			_rigidbody2D.AddForce(transform.up * _jumpSpeed, ForceMode2D.Impulse);
			_animator.Play(PlayerAnimatorData.Params.jump);
		}

		if (IsGrounded == false)
		{
			_animator.Play(PlayerAnimatorData.Params.jump);
			return;
		}

		_moveVector.x = Input.GetAxis("Horizontal");
		/*
		if (IsGrounded && _moveVector.x != 0)
		{
			_animator.Play("run");
		}*/

		if (_moveVector.x > 0 && IsGrounded)
		{
			_spriteRenderer.flipX = false;
			_animator.Play("run");

		}
		else if (_moveVector.x < 0 && IsGrounded)
		{
			_spriteRenderer.flipX = true;
			
			_animator.Play("run");
		}
		else
			_animator.Play("stayIdle");

		_rigidbody2D.velocity = new Vector2(Input.GetAxis("Horizontal") * _speed, _rigidbody2D.velocity.y);

		//_rigidbody2D.MovePosition(_rigidbody2D.position + _moveVector * _speed * Time.fixedDeltaTime);
	}

	private void FixedUpdate()
	{

	}
}

public static class PlayerAnimatorData
{ 
	public static class Params
	{
		public static readonly int jump = Animator.StringToHash(nameof(jump));

	}
}