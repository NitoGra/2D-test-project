using System.Collections;
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

	[SerializeField] private MotionControl _motionControl;
	[SerializeField] private Animator _animator;
	[SerializeField] private PlayerHealth _playerHealth;

	private void OnEnable()
	{
		_playerHealth.DeadOrdered += PlayDead;
		_motionControl.JumpOrdered += PlayJump;
		_motionControl.IdleOrdered += PlayIdle;
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
		StartCoroutine(MakePlayerDead());
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

	private bool IsPlayerDead()
	{
		return _animator.GetCurrentAnimatorStateInfo(0).IsName(nameof(Dead));
	}

	private IEnumerator MakePlayerDead()
	{
		if (IsPlayerDead())
			gameObject.SetActive(false);

		yield return null;
	}
}