using UnityEngine;

public class PlayerIdle : MonoBehaviour
{
	private static readonly int Idle = Animator.StringToHash(nameof(Idle));

	private MotionControl _motionControl;
	private Animator _animator;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_motionControl = GetComponent<MotionControl>();
	}

	private void OnEnable()
	{
		_motionControl.PlayerIdle += PlayIdle;
	}

	private void OnDisable()
	{
		_motionControl.PlayerIdle -= PlayIdle;
	}

	private void PlayIdle()
	{
		_animator.Play(Idle);
	}
}