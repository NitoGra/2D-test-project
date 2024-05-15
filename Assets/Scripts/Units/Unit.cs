using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(FaceFliper))]
public class Unit : MonoBehaviour
{
	protected FaceFliper FaceFliper;

	protected virtual void Start()
	{
		FaceFliper = GetComponent<FaceFliper>();
	}

	protected void RotateToTarget(Vector2 targetToLook)
	{
		FaceFliper.Flip(targetToLook.x - transform.position.x);
	}
}