using System;
using UnityEngine;

[RequireComponent(typeof(FaceFliper))]
public class KeyDetect : MonoBehaviour
{
	public event Action Sitted;
	public event Action Jumped;
	public event Action Runned;
	public event Action Idled;
	public event Action Attacked;
	public event Action AbilityUsed;

	private const KeyCode JumpKey = KeyCode.Space;
	private const KeyCode SitKey = KeyCode.S;
	private const KeyCode AttackKey = KeyCode.F;
	private const KeyCode AbilityKey = KeyCode.E;
	private const string Horizontal = "Horizontal";

	private bool _isSit = false;
	private bool _isAttack = false;
	private bool _isRun = false;
	private bool _isJump = false;
	private Vector2 _moveVector = new();
	private FaceFliper _faceFliper;

	private void Start()
	{
		_faceFliper = GetComponent<FaceFliper>();
	}

	private void Update()
	{
		_moveVector.x = Input.GetAxis(Horizontal);

		if (TryJump())
			return;
	}

	private void FixedUpdate()
	{
		_moveVector.x = Input.GetAxis(Horizontal);
		MakeNormalState();
		CheckKeys();
	}

	private void CheckKeys()
	{
		if (TryAttack())
			return;
		else if (TryAbility())
			return;
		else if (TrySit())
			return;
		else if (TryRun())
			return;

		TryIdle();
	}

	private bool TryJump()
	{
		if (Input.GetKeyDown(JumpKey))
		{
			Jumped?.Invoke();
			_isJump = true;
		}

		return _isJump;
	}

	private bool TryAttack()
	{
		if (Input.GetKey(AttackKey))
		{
			Attacked?.Invoke();
			_isAttack = true;
		}

		return _isAttack;
	}

	private bool TrySit()
	{
		if (Input.GetKey(SitKey))
		{
			Sitted?.Invoke();
			_isSit = true;
		}

		return _isSit;
	}

	private bool TryAbility()
	{
		if (Input.GetKey(AbilityKey))
		{
			AbilityUsed?.Invoke();
		}

		return _isSit;
	}

	private bool TryRun()
	{
		if (_moveVector.x != 0)
		{
			Runned?.Invoke();
			DoFlip();
			_isRun = true;
		}

		return _isRun;
	}

	private void TryIdle()
	{
		if ((_isRun && _isAttack && _isSit && _isJump) == false)
			Idled?.Invoke();
	}

	private void MakeNormalState()
	{
		_isAttack = false;
		_isSit = false;
		_isRun = false;
		_isJump = false;
	}

	private void DoFlip()
	{
		_faceFliper.Flip(_moveVector.x);
	}

	public Vector2 GetMoveVector() => _moveVector;
}