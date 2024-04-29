using UnityEngine;

[RequireComponent(typeof(MotionControl), typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
	private readonly int Sit = Animator.StringToHash(nameof(Sit));
	private readonly int Run = Animator.StringToHash(nameof(Run));
	private readonly int Idle = Animator.StringToHash(nameof(Idle));
	private readonly int Jump = Animator.StringToHash(nameof(Jump));
	private readonly int Attack = Animator.StringToHash(nameof(Attack));
	private readonly int Dead = Animator.StringToHash(nameof(Dead));

	[SerializeField] private MotionControl _motionControl;
	[SerializeField] private Animator _animator;
	[SerializeField] private PlayerHealth _playerHealth;

	private void Start()
	{
		//_playerHealth = GetComponent<PlayerHealth>();
	}

	private void FixedUpdate()
	{
		IsAttackClip();
	}

	private void OnEnable()
	{
		_motionControl.JumpOrdered += PlayJump;
		_motionControl.IdleOrdered += PlayIdle;
		_playerHealth.DeadOrdered += PlayDead;
		_motionControl.RunOrdered += PlayRun;
		_motionControl.SitOrdered += PlaySit;
		_motionControl.AttackOrdered += PlayAttack;
	}

	private void OnDisable()
	{
		_motionControl.JumpOrdered -= PlayJump;
		_motionControl.IdleOrdered -= PlayIdle;
		_playerHealth.DeadOrdered -= PlayDead;
		_motionControl.RunOrdered -= PlayRun;
		_motionControl.SitOrdered -= PlaySit;
		_motionControl.AttackOrdered -= PlayAttack;
	}

	private void PlayIdle()
	{
		_animator.Play(Idle);
	}	
	
	private void PlayDead()
	{
		_animator.Play(Dead);
	}

	private void PlaySit()
	{
		_animator.Play(Sit);
	}

	private void PlayJump()
	{
		_animator.Play(Jump);
	}

	private void PlayRun()
	{
		_animator.Play(Run);
	}

	private void PlayAttack()
	{
		_animator.Play(Attack);
	}

	public bool IsAttackClip()
	{
		return _animator.GetCurrentAnimatorStateInfo(0).IsName(nameof(Attack));
	}
}