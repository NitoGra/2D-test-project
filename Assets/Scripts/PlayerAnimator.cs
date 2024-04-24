using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MotionControl))]
public class PlayerAnimator : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private MotionControl _motionControl;

	private PlayerIdle _playerIdle;
	private PlayerJumped _playerJumped;
	private PlayerSat _playerSat;
	private PlayerRan _playerRan;

	public void Awake()
	{
		_playerIdle.SetAnimatorAndControl(_motionControl, _animator);
		_playerJumped.SetAnimatorAndControl(_motionControl, _animator);
		_playerSat.SetAnimatorAndControl(_motionControl, _animator);
		_playerRan.SetAnimatorAndControl(_motionControl, _animator);
	}
}

public class PlayerIdle : MonoBehaviour
{
	private static readonly int Idle = Animator.StringToHash(nameof(Idle));

	private MotionControl _motionControl;
	private Animator _animator;

	public void SetAnimatorAndControl(MotionControl motionControl, Animator animator)
	{
		_motionControl = motionControl;
		_animator = animator;
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

public class PlayerSat : MonoBehaviour
{
	private static readonly int Sit = Animator.StringToHash(nameof(Sit));

	private MotionControl _motionControl;
	private Animator _animator;

	public void SetAnimatorAndControl(MotionControl motionControl, Animator animator)
	{
		_motionControl = motionControl;
		_animator = animator;
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

public class PlayerJumped : MonoBehaviour
{
	private static readonly int Jump = Animator.StringToHash(nameof(Jump));

	private MotionControl _motionControl;
	private Animator _animator;

	public void SetAnimatorAndControl(MotionControl motionControl, Animator animator)
	{
		_motionControl = motionControl;
		_animator = animator;
	}

	private void OnEnable()
	{
		_motionControl.PlayerJumped += PlayJump;
	}

	private void OnDisable()
	{
		_motionControl.PlayerJumped -= PlayJump;
	}

	private void PlayJump()
	{
		_animator.Play(Jump);
	}
}

public class PlayerRan : MonoBehaviour
{
	private static readonly int Run = Animator.StringToHash(nameof(Run));

	private MotionControl _motionControl;
	private Animator _animator;

	public void SetAnimatorAndControl(MotionControl motionControl, Animator animator)
	{
		_motionControl = motionControl;
		_animator = animator;
	}

	private void OnEnable()
	{
		_motionControl.PlayerRan += PlayRun;
	}

	private void OnDisable()
	{
		_motionControl.PlayerRan -= PlayRun;
	}

	private void PlayRun()
	{
		_animator.Play(Run);
	}
}