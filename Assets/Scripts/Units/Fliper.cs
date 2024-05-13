using UnityEngine;

public class FaceFliper : MonoBehaviour
{
	private readonly Quaternion ForwardRotation = Quaternion.Euler(0f, 0f, 0f);
	private readonly Quaternion BackwardRotation = Quaternion.Euler(0f, 180f, 0f);

	[SerializeField] private Transform _transform;

	public void Flip(float velocityX)
	{
		if (velocityX > 0)
			_transform.rotation = ForwardRotation;
		else if (velocityX < 0)
			_transform.rotation = BackwardRotation;
	}
}