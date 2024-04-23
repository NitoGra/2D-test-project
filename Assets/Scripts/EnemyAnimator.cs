using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
	[SerializeField] private Animator _animator;

	public static class Params
	{
		public static readonly int EnemyDead = Animator.StringToHash(nameof(EnemyDead));
		public static readonly int EnemyRun = Animator.StringToHash(nameof(EnemyRun));
	}

	public void Run()
	{
		_animator.Play(Params.EnemyRun);
	}

	public void Dead()
	{
		_animator.Play(Params.EnemyDead);
	}
}