using UnityEngine;

[RequireComponent(typeof(Animator), typeof(EnemyControl), typeof(Health))]
public class EnemyAnimator : MonoBehaviour
{
	private static readonly int EnemyDead = Animator.StringToHash(nameof(EnemyDead));
	private static readonly int EnemyWalk = Animator.StringToHash(nameof(EnemyWalk));
	private static readonly int EnemyAttack = Animator.StringToHash(nameof(EnemyAttack));

	[SerializeField] private Animator _animator;
	[SerializeField] private EnemyControl _control;
	[SerializeField] private Health _health;
	[SerializeField] private Attack _attack;

	private void OnEnable()
	{
		_health.DeadOrdered += PlayDead;
		_control.AttackOrdered += PlayAttack;
	}

	private void OnDisable()
	{
		_health.DeadOrdered -= PlayDead;
		_control.AttackOrdered -= PlayAttack;
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