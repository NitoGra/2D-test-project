using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
	[SerializeField] private Animator _animator;

	private static readonly int EnemyDead = Animator.StringToHash(nameof(EnemyDead));
	private static readonly int EnemyRun = Animator.StringToHash(nameof(EnemyRun));
	private static readonly int EnemyAttack = Animator.StringToHash(nameof(EnemyAttack));

	public void PlayRun()
	{
		_animator.Play(EnemyRun);
	}

	public void PlayDead()
	{
		_animator.Play(EnemyDead);
	}

	public void PlayAttack()
	{
		_animator.Play(EnemyAttack);
	}
}