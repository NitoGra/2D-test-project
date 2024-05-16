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
	private readonly int Corpse = Animator.StringToHash(nameof(Corpse));

	[SerializeField] private Player _player;
	[SerializeField] private Animator _animator;
	[SerializeField] private Health _health;
	[SerializeField] private KeyDetect _keyDetect;
	[SerializeField] private float _deathDelay;
	[SerializeField] private float _damageDelay;

	private bool _isDead = false;
	private bool _isDamage = false;
	private bool IsAnimated => _isDamage || _isDead;

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
		if (IsAnimated)
			return;

		if (_player.IsGrounded)
			_animator.Play(Idle);
	}

	private void PlayDead()
	{
		Invoke(nameof(MakeCorpse), _deathDelay);
		_isDead = true;
		_animator.Play(Dead);
	}

	private void PlayDamage()
	{
		Invoke(nameof(EndDamage), _damageDelay);
		_isDamage = true;
		_animator.Play(GetDamage);
	}

	private void PlaySit()
	{
		if (IsAnimated)
			return;

		if (_player.IsGrounded)
			_animator.Play(Sit);
	}

	private void PlayJump()
	{
		if (IsAnimated)
			return;

		if (_player.IsGrounded == false)
			_animator.Play(Jump);
	}

	private void PlayRun()
	{
		if (IsAnimated)
			return;

		if (_player.IsGrounded)
			_animator.Play(Run);
	}

	private void PlayAttack()
	{
		if (IsAnimated)
			return;

		if (_player.IsGrounded)
			_animator.Play(Attack);
	}

	private void EndDamage()
	{
		_isDamage = false;
	}

	private void MakeCorpse()
	{
		_animator.Play(nameof(Corpse));
		Immobilization();
		KillPlayer();
	}

	private void Immobilization()
	{
		_keyDetect.enabled = false;
	}

	private void KillPlayer()
	{
		_player.name = nameof(Corpse);
		_player.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
		BoxCollider2D box = _player.gameObject.GetComponent<BoxCollider2D>();
		box.size = new Vector2(0.1f, 0.5f);
		box.offset = new Vector2(0, 0.3f);
	}
}