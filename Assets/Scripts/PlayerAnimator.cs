using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	private Animator Animator;
	private static readonly int Jump = Animator.StringToHash(nameof(Jump));
	private static readonly int Run = Animator.StringToHash(nameof(Run));
	private static readonly int Idle = Animator.StringToHash(nameof(Idle));
	private static readonly int Sit = Animator.StringToHash(nameof(Sit));

	public void Awake()
	{
		Animator = GetComponent<Animator>();
	}

	public void PlayIdle()
	{
		Animator.Play(Idle);
	}

	public void PlayRun()
	{
		Animator.Play(Run);
	}

	public void PlayJump()
	{
		Animator.Play(Jump);
	}

	public void PlaySit()
	{
		Animator.Play(Sit);
	}
}