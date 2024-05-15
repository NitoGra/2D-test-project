using System;
using UnityEngine;

public class KeyDetect : MonoBehaviour
{
	private const KeyCode JumpKey = KeyCode.Space;
	private const KeyCode SitKey = KeyCode.S;
	private const KeyCode AttackKey = KeyCode.F;
	private const string Horizontal = "Horizontal";

	private Vector2 _moveVector = new Vector2();
	private bool _isSit = false;
	private bool _isAttack = false;
	private bool _isRun = false;
	private bool _isJump = false;

	public event Action Siting;
	public event Action Jumping;
	public event Action Runing;
	public event Action Idleing;
	public event Action Attacking;

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
			Jumping?.Invoke();
			_isJump = true;
		}

		return _isJump;
	}

	private bool TryAttack()
	{
		if (Input.GetKey(AttackKey))
		{
			Attacking?.Invoke();
			_isAttack = true;
		}

		return _isAttack;
	}

	private bool TrySit()
	{
		if (Input.GetKey(SitKey))
		{
			Siting?.Invoke();
			_isSit = true;
		}

		return _isSit;
	}

	private bool TryRun()
	{
		if (_moveVector.x != 0)
		{
			Runing?.Invoke();
			_isRun = true;
		}

		return _isRun;
	}

	private void TryIdle()
	{
		if ((_isRun && _isAttack && _isSit && _isJump) == false)
			Idleing?.Invoke();
	}

	private void MakeNormalState()
	{
		_isAttack = false;
		_isSit = false;
		_isRun = false;
		_isJump = false;
	}

	public Vector2 GetMoveVector() => _moveVector;
}