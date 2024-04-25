using UnityEngine;
using UnityEngine.Playables;

public class SitMove : MonoBehaviour
{/*
	private readonly int Sit = Animator.StringToHash(nameof(Sit));
	private readonly int Run = Animator.StringToHash(nameof(Run));
	private readonly int Idle = Animator.StringToHash(nameof(Idle));
	private readonly int Jump = Animator.StringToHash(nameof(Jump));
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
		_motionControl.RunOrdered += PlayRun;
		_motionControl.SitOrdered += PlaySit;
	}

	private void OnDisable()
	{

		_motionControl.IdleOrdered -= PlayIdle;
		_motionControl.RunOrdered -= PlayRun;
		_motionControl.SitOrdered -= PlaySit;
	}


	private void PlayRun()
	{
		_animator.Play(Run);
	}

	private void PlaySit()
	{
		_animator.Play(Sit);
	}*/
}