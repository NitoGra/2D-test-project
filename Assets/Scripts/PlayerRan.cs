using UnityEngine;

public class PlayerRan : MonoBehaviour
{
	private static readonly int Run = Animator.StringToHash(nameof(Run));

	private MotionControl _motionControl;
	private Animator _animator;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_motionControl = GetComponent<MotionControl>();
	}

	private void OnEnable()
	{
		_motionControl.RunKeyPressed += PlayRun;
	}

	private void OnDisable()
	{
		_motionControl.RunKeyPressed -= PlayRun;
	}

	private void PlayRun()
	{
		_animator.Play(Run);
	}
}