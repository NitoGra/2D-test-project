using UnityEngine;

public class Pickups : MonoBehaviour
{
	[SerializeField] public float Value { get; private set; }

	public virtual void Pickup(PlayerAudio playerAudio)
	{
		gameObject.SetActive(false);
	}
}