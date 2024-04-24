using UnityEngine;

[RequireComponent(typeof(MotionControl), typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
	private PlayerSat _playerSat;
	private PlayerIdle _playerIdle;
	private PlayerJumped _playerJumped;
	private PlayerRan _playerRan;

	public void Awake()
	{
		_playerSat = gameObject.AddComponent<PlayerSat>();
		_playerIdle = gameObject.AddComponent<PlayerIdle>();
		_playerJumped = gameObject.AddComponent<PlayerJumped>();
		_playerRan = gameObject.AddComponent<PlayerRan>();
	}
}