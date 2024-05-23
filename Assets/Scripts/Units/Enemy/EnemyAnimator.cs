using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Enemy), typeof(Health))]
public class EnemyAnimator : MonoBehaviour
{
	[SerializeField] private EnemyHunt _hunt;
	[SerializeField] private Animator _animator;
	[SerializeField] private Health _health;
	[SerializeField] private Attack _attack;
	[SerializeField] private float _deathDelay;

	private static readonly int EnemyDead = Animator.StringToHash(nameof(EnemyDead));
	private static readonly int EnemyWalk = Animator.StringToHash(nameof(EnemyWalk));
	private static readonly int EnemyAttack = Animator.StringToHash(nameof(EnemyAttack));

	private void OnEnable()
	{
		_health.Died += PlayDead;
		_hunt.AttackOrdered += PlayAttack;
	}

	private void OnDisable()
	{
		_health.Died -= PlayDead;
		_hunt.AttackOrdered -= PlayAttack;
	}

	private void PlayWalk()
	{
		_animator.Play(EnemyWalk);
	}

	private void PlayDead()
	{
		Invoke(nameof(StartDead), _deathDelay);
		_animator.Play(EnemyDead);
	}

	private void PlayAttack()
	{
		_animator.Play(EnemyAttack);
	}

	private void StartDead()
	{
		gameObject?.SetActive(false);
	}
}