using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	public Animator Animator;

	public static class Params
	{
		public static readonly int Jump = Animator.StringToHash(nameof(Jump));
		public static readonly int Run = Animator.StringToHash(nameof(Run));
		public static readonly int Idle = Animator.StringToHash(nameof(Idle));
		public static readonly int Sit = Animator.StringToHash(nameof(Sit));
	}

	public void Awake()
	{
		Animator = GetComponent<Animator>();
	}

	public void Idle()
	{
		Animator.Play(Params.Idle);
	}

	public void Run()
	{
		Animator.Play(Params.Run);
	}

	public void Jump()
	{
		Animator.Play(Params.Jump);
	}

	public void Sit()
	{
		Animator.Play(Params.Sit);
	}
}