using UnityEngine;

public class PlayerSit : MonoBehaviour
{
	private readonly int Sit = Animator.StringToHash(nameof(Sit));

	private MotionControl _motionControl;
	private Animator _animator;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_motionControl = GetComponent<MotionControl>();
	}

	private void OnEnable()
	{
		_motionControl.SitOrdered += PlaySit;
	}

	private void OnDisable()
	{
		_motionControl.SitOrdered -= PlaySit;
	}

	private void PlaySit()
	{
		_animator.Play(Sit);
	}
}