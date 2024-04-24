using UnityEngine;

public class Coin : MonoBehaviour
{
	[SerializeField] private float _delay;

	public void PickUp() => gameObject.SetActive(false);
}