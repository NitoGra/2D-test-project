using UnityEngine;

public class Mover : MonoBehaviour
{
	private Rigidbody2D _rigidbody2D;

	public void Awake()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
	}

	public void HorizontalMove(float xSpeedOnRigidbody2D)
	{
		_rigidbody2D.velocity = new Vector2(xSpeedOnRigidbody2D, _rigidbody2D.velocity.y);
	}

	public void ImpulseMove(Vector2 impulseDirection)
	{
		_rigidbody2D.AddForce(impulseDirection, ForceMode2D.Impulse);
	}
}