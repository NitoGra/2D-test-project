using UnityEngine;

public class FaceFliper : MonoBehaviour
{
	private readonly Quaternion ForwardRotation = Quaternion.Euler(0f, 0f, 0f);
	private readonly Quaternion BackwardRotation = Quaternion.Euler(0f, 180f, 0f);

	[SerializeField] private Transform _object;
	[SerializeField] private RectTransform _interfase;

	public void Flip(float velocityX)
	{
		if (velocityX > 0)
			_object.rotation = ForwardRotation;
		else if (velocityX < 0)
			_object.rotation = BackwardRotation;

		_interfase.rotation = BackwardRotation;
	}
}