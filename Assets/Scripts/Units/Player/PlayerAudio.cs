using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
	[SerializeField] private Health _health;
	[SerializeField] private Attack _attack;
	[SerializeField] private AudioSource _audio;

	[SerializeField] private AudioClip _medicBagSound;
	[SerializeField] private AudioClip _coinSound;

	[SerializeField] private AudioClip _damageSound;
	[SerializeField] private AudioClip _deadSound;

	[SerializeField] private AudioClip _hitSound;
	[SerializeField] private AudioClip _missSound;

	public AudioClip GetCoinSound => _coinSound;
	public AudioClip GetMedicBagSound => _medicBagSound;

	private void OnEnable()
	{
		_health.Damaged += DamageSound;
		_health.Died += DeadSound;
		_attack.HitSoundPlayed += HitSound;
		_attack.MissSoundPlayed += MissSound;
	}

	private void OnDisable()
	{
		_health.Damaged -= DamageSound;
		_health.Died -= DeadSound;
		_attack.HitSoundPlayed -= HitSound;
		_attack.MissSoundPlayed -= MissSound;
	}

	private void DamageSound()
	{
		_audio.clip = _damageSound;
		_audio.Play();
	}

	private void DeadSound()
	{
		_audio.clip = _deadSound;
		_audio.Play();
	}

	private void HitSound()
	{
		_audio.clip = _hitSound;
		_audio.Play();
	}

	private void MissSound()
	{
		_audio.clip = _missSound;
		_audio.Play();
	}
}