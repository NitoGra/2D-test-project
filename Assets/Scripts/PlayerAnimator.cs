using UnityEngine;

[RequireComponent(typeof(MotionControl), typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
	private SitMove _playerSit;
	private IdleMove _playerIdle;
	private JumpMove _playerJump;
	private RunMove _playerRun;

	public void Awake()
	{
		_playerSit = gameObject.AddComponent<SitMove>();
		_playerIdle = gameObject.AddComponent<IdleMove>();
		_playerJump = gameObject.AddComponent<JumpMove>();
		_playerRun = gameObject.AddComponent<RunMove>();
	}
}