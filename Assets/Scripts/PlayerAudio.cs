using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
	[SerializeField] private AudioSource _audio;
	[SerializeField] private AudioClip _medicBagSound;
	[SerializeField] private AudioClip _coinSound;
	[SerializeField] private AudioClip _damageSound;
	[SerializeField] private AudioClip _deadSound;

	[SerializeField] private AudioClip _hitSound;
	[SerializeField] private AudioClip _missSound;

	public void CoinSound()
	{
		_audio.clip = _coinSound;
		_audio.Play();
	}

	public void DamageSound()
	{
		_audio.clip = _damageSound;
		_audio.Play();
	}

	public void HitSound()
	{
		_audio.clip = _hitSound;
		_audio.Play();
	}

	public void MissSound()
	{
		_audio.clip = _missSound;
		_audio.Play();
	}

	public void MedicBagSound()
	{
		_audio.clip = _medicBagSound;
		_audio.Play();
	}

	public void DeadSound()
	{
		_audio.clip = _deadSound;
		_audio.Play();
	}
}
