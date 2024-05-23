using UnityEngine;

public class FaceFliper : MonoBehaviour
{
	[SerializeField] private Transform _object;
	[SerializeField] private RectTransform _interfase;

	private readonly Quaternion _forwardRotation = Quaternion.Euler(0f, 0f, 0f);
	private readonly Quaternion _backwardRotation = Quaternion.Euler(0f, 180f, 0f);

	public void Flip(float velocityX)
	{
		if (velocityX > 0)
			_object.rotation = _forwardRotation;
		else if (velocityX < 0)
			_object.rotation = _backwardRotation;

		_interfase.rotation = _backwardRotation;
	}
}