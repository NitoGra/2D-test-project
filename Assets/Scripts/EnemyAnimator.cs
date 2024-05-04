using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Animator), typeof(EnemyPatrol), typeof(EnemyHealth))]
public class EnemyAnimator : MonoBehaviour
{
	[SerializeField] private Animator _animator;

	private static readonly int EnemyDead = Animator.StringToHash(nameof(EnemyDead));
	private static readonly int EnemyWalk = Animator.StringToHash(nameof(EnemyWalk));
	private static readonly int EnemyAttack = Animator.StringToHash(nameof(EnemyAttack));

	[SerializeField] private EnemyPatrol _enemyPatrol;
	[SerializeField] private EnemyHealth _enemyHealth;
	[SerializeField] private EnemyAttack _enemyAttack;

	private void OnEnable()
	{
		_enemyAttack.EnemyWalkOrdered += PlayWalk;
		_enemyHealth.EnemyDeadOrdered += PlayDead;
		_enemyPatrol.EnemyAttackOrdered += PlayAttack;
	}

	private void OnDisable()
	{
		_enemyAttack.EnemyWalkOrdered -= PlayWalk;
		_enemyHealth.EnemyDeadOrdered -= PlayDead;
		_enemyPatrol.EnemyAttackOrdered -= PlayAttack;
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