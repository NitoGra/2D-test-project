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
		_motionControl.IdleOrdered += PlayIdle;
	}

	private void OnDisable()
	{
		_motionControl.IdleOrdered -= PlayIdle;
	}

	private void PlayIdle()
	{
		_animator.Play(Idle);
	}
}