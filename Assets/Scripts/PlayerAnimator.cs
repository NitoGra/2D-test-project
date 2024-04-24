using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MotionControl))]
public class PlayerAnimator : MonoBehaviour
{
	private static readonly int Jump = Animator.StringToHash(nameof(Jump));
	private static readonly int Run = Animator.StringToHash(nameof(Run));
	private static readonly int Idle = Animator.StringToHash(nameof(Idle));
	private static readonly int Sit = Animator.StringToHash(nameof(Sit));

	private Animator _animator;
	private MotionControl _motionControl;

	public void Awake()
	{
		_animator = GetComponent<Animator>();
		_motionControl = GetComponent<MotionControl>();
	}

	private void OnEnable()
	{
		_motionControl.PlayerSat += PlaySit;
	}

	private void OnDisable()
	{
		_motionControl.PlayerSat -= PlaySit;
	}

	public void PlayIdle()
	{
		_animator.Play(Idle);
	}

	public void PlayRun()
	{
		_animator.Play(Run);
	}

	public void PlayJump()
	{
		_animator.Play(Jump);
	}

	public void PlaySit()
	{
		_animator.Play(Sit);
	}
}