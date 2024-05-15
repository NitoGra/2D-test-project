using System.Collections;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	private readonly int Sit = Animator.StringToHash(nameof(Sit));
	private readonly int Run = Animator.StringToHash(nameof(Run));
	private readonly int Idle = Animator.StringToHash(nameof(Idle));
	private readonly int Jump = Animator.StringToHash(nameof(Jump));
	private readonly int Attack = Animator.StringToHash(nameof(Attack));
	private readonly int Dead = Animator.StringToHash(nameof(Dead));
	private readonly int GetDamage = Animator.StringToHash(nameof(GetDamage));

	[SerializeField] private Player _player;
	[SerializeField] private Animator _animator;
	[SerializeField] private Health _health;
	[SerializeField] private KeyDetect _keyDetect;
	[SerializeField] private float _deathDelay;
	                
	private void OnEnable()
	{
		_health.Died += PlayDead;
		_health.Damaging += PlayDamage;
		_keyDetect.Jumping += PlayJump;
		_keyDetect.Idleing += PlayIdle;
		_keyDetect.Runing += PlayRun;
		_keyDetect.Siting += PlaySit;
		_keyDetect.Attacking += PlayAttack;
	}

	private void OnDisable()
	{
		_health.Died -= PlayDead;
		_health.Damaging -= PlayDamage;
		_keyDetect.Jumping -= PlayJump;
		_keyDetect.Idleing -= PlayIdle;
		_keyDetect.Runing -= PlayRun;
		_keyDetect.Siting -= PlaySit;
		_keyDetect.Attacking -= PlayAttack;
	}

	private void FixedUpdate()
	{
		PlayJump();
	}

	private void PlayIdle()
	{
		if(_player.IsGrounded)
			_animator.Play(Idle);
	}

	private void PlayDead()
	{
		Invoke(nameof(StartDead), _deathDelay);
		_animator.Play(Dead);
	}

	private void PlayDamage()
	{
		//StartCoroutine(DamageAnimation());

		_animator.Play(GetDamage);
	}

	private void PlaySit()
	{
		if (_player.IsGrounded)
			_animator.Play(Sit);
	}

	private void PlayJump()
	{
		if (_player.IsGrounded == false)
			_animator.Play(Jump);
	}

	private void PlayRun()
	{
		if (_player.IsGrounded)
			_animator.Play(Run);
	}

	private void PlayAttack()
	{
		if (_player.IsGrounded)
			_animator.Play(Attack);
	}

	private void StartDead()
	{
		gameObject.SetActive(false);
	}
	/*
	private float DamageGetTime = 2; 

	private IEnumerable DamageAnimation()
	{
		gameObject.GetComponent<SpriteRenderer>().color = Color.red;

		while (_secondsHuntDelay > _secondsHuntCount)
		{
			_secondsHuntCount++;
			_huntDistance = -UnityEngine.Random.Range(0, (int)_attackDistance);
			yield return _rememberDelay;
		}

		LoseTargetOrdered?.Invoke();
		yield return null;
	}*/
}