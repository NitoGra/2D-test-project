using UnityEngine;

public class Pickups : MonoBehaviour
{
	[SerializeField] private float _value;

	public virtual void Pickup(PlayerAudio playerAudio)
	{
		gameObject.SetActive(false);
	}

	public float Value() => _value;
}