using UnityEngine;

[RequireComponent(typeof(Animator), typeof(EnemyControl), typeof(EnemyHealth))]
public class EnemyAnimator : MonoBehaviour
{
	[SerializeField] private Animator _animator;

	private static readonly int EnemyDead = Animator.StringToHash(nameof(EnemyDead));
	private static readonly int EnemyWalk = Animator.StringToHash(nameof(EnemyWalk));
	private static readonly int EnemyAttack = Animator.StringToHash(nameof(EnemyAttack));

	[SerializeField] private EnemyControl _control;
	[SerializeField] private EnemyHealth _health;
	[SerializeField] private EnemyAttack _attack;

	private void OnEnable()
	{
		_attack.EnemyWalkOrdered += PlayWalk;
		_health.EnemyDeadOrdered += PlayDead;
		_control.EnemyAttackOrdered += PlayAttack;
	}

	private void OnDisable()
	{
		_attack.EnemyWalkOrdered -= PlayWalk;
		_health.EnemyDeadOrdered -= PlayDead;
		_control.EnemyAttackOrdered -= PlayAttack;
	}
	
	private void PlayWalk()
	{
		_animator.Play(EnemyWalk);
	}

	private void PlayDead()
	{
		_animator.Play(EnemyDead);
	}

	private void PlayAttack()
	{
		_animator.Play(EnemyAttack);
	}
}