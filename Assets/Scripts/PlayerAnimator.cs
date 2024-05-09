using UnityEngine;

[RequireComponent(typeof(MotionControl), typeof(Animator), typeof(PlayerHealth))]
public class PlayerAnimator : MonoBehaviour
{
	private readonly int Sit = Animator.StringToHash(nameof(Sit));
	private readonly int Run = Animator.StringToHash(nameof(Run));
	private readonly int Idle = Animator.StringToHash(nameof(Idle));
	private readonly int Jump = Animator.StringToHash(nameof(Jump));
	private readonly int Attack = Animator.StringToHash(nameof(Attack));
	private readonly int Dead = Animator.StringToHash(nameof(Dead));
	private readonly int GetDamage = Animator.StringToHash(nameof(GetDamage));

	[SerializeField] private MotionControl _motionControl;
	[SerializeField] private Animator _animator;
	[SerializeField] private PlayerHealth _health;
	[SerializeField] private float _deathDelay;

	private void OnEnable()
	{
		_motionControl.JumpOrdered += PlayJump;
		_motionControl.IdleOrdered += PlayIdle;
		_health.DeadOrdered += PlayDead;
		_health.DamageTakeOrderd += PlayDamage;
		_motionControl.RunOrdered += PlayRun;
		_motionControl.SitOrdered += PlaySit;
		_motionControl.AttackOrdered += PlayAttack;
	}

	private void OnDisable()
	{
		_motionControl.JumpOrdered -= PlayJump;
		_motionControl.IdleOrdered -= PlayIdle;
		_health.DeadOrdered -= PlayDead;
		_health.DamageTakeOrderd -= PlayDamage;
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
		Invoke(nameof(StartDead), _deathDelay);
		_animator.Play(Dead);
	}

	private void PlayDamage()
	{
		_animator.Play(GetDamage);
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

	private void StartDead()
	{
		gameObject.SetActive(false);
	}
}