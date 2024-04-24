using UnityEngine;

public class PlayerSat : MonoBehaviour
{
	private static readonly int Sit = Animator.StringToHash(nameof(Sit));

	private MotionControl _motionControl;
	private Animator _animator;

	private void Awake()
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

	private void PlaySit()
	{
		_animator.Play(Sit);
	}
}