﻿using UnityEngine;

[RequireComponent(typeof(Mover), typeof(SpriteRenderer), typeof(FaceFliper))]
public class Unit : MonoBehaviour
{
	[SerializeField] protected Health Health;

	protected FaceFliper FaceFliper;

	protected virtual void OnEnable()
	{
		Health.TakeDamageOrdered += GetHit;
		Health.DeadOrdered += Die;
	}

	protected virtual void OnDisable()
	{
		Health.TakeDamageOrdered -= GetHit;
		Health.DeadOrdered -= Die;
	}

	protected virtual void Start()
	{
		FaceFliper = GetComponent<FaceFliper>();
	}

	protected void RotateToTarget(Vector2 targetToLook)
	{
		FaceFliper.Flip(targetToLook.x - transform.position.x);
	}

	protected virtual void GetHit(){}
	protected virtual void Die(){}
}
