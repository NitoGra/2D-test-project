using UnityEngine;

[RequireComponent(typeof(MotionControl), typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
	private PlayerSit _playerSat;
	private PlayerIdle _playerIdle;
	private PlayerJump _playerJumped;
	private PlayerRun _playerRan;

	public void Awake()
	{
		_playerSat = gameObject.AddComponent<PlayerSit>();
		_playerIdle = gameObject.AddComponent<PlayerIdle>();
		_playerJumped = gameObject.AddComponent<PlayerJump>();
		_playerRan = gameObject.AddComponent<PlayerRun>();
	}
}