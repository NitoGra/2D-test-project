using UnityEngine;

[RequireComponent(typeof(MotionControl), typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
	private readonly int Sit = Animator.StringToHash(nameof(Sit));
	private readonly int Run = Animator.StringToHash(nameof(Run));
	private readonly int Idle = Animator.StringToHash(nameof(Idle));
	private readonly int Jump = Animator.StringToHash(nameof(Jump));

	[SerializeField] private MotionControl _motionControl;
	[SerializeField] private Animator _animator;

	private void OnEnable()
	{
		_motionControl.JumpOrdered += PlayJump;
		_motionControl.IdleOrdered += PlayIdle;
		_motionControl.RunOrdered += PlayRun;
		_motionControl.SitOrdered += PlaySit;
	}

	private void OnDisable()
	{
		_motionControl.JumpOrdered -= PlayJump;
		_motionControl.IdleOrdered -= PlayIdle;
		_motionControl.RunOrdered -= PlayRun;
		_motionControl.SitOrdered -= PlaySit;
	}

	private void PlayIdle()
	{
		_animator.Play(Idle);
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

	public bool IsSitClip()
	{
		return _animator.GetCurrentAnimatorStateInfo(0).IsName(nameof(Sit));
	}
}