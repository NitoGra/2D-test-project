using UnityEngine;

public class PlayerJumped : MonoBehaviour
{
	private static readonly int Jump = Animator.StringToHash(nameof(Jump));

	private MotionControl _motionControl;
	private Animator _animator;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_motionControl = GetComponent<MotionControl>();
	}

	private void OnEnable()
	{
		_motionControl.JumpKeyPressed += PlayJump;
	}

	private void OnDisable()
	{
		_motionControl.JumpKeyPressed -= PlayJump;
	}

	private void PlayJump()
	{
		_animator.Play(Jump);
	}
}