using UnityEngine;

[RequireComponent(typeof(FaceFliper), typeof(EnemyPatrol))]
public class Enemy : MonoBehaviour
{
	[SerializeField] private EnemyHunt _enemyHunt;
	[SerializeField] private Transform[] _frontViewPoints = new Transform[2];
	[SerializeField] private Transform[] _backViewPoints = new Transform[2];

	private float _drawLineDelay = 0.1f;
	private Color _frontColor = Color.red;
	private Color _backColor = Color.blue;

	private FaceFliper _faceFliper;
	private EnemyPatrol _enemyPatrol;

	private GameObject _target;
	public GameObject Target { get { return _target; } private set { Target = _target; } }

	private void OnEnable()
	{
		_enemyHunt.LoseTargetOrdered += EndHunt;
	}

	private void OnDisable()
	{
		_enemyHunt.LoseTargetOrdered -= EndHunt;
	}

	private void Start()
	{
		_faceFliper = GetComponent<FaceFliper>();
		_enemyPatrol = GetComponent<EnemyPatrol>();
	}

	private void FixedUpdate()
	{
		_target = GetTarget();

		if (_target != null)
			StartHunt();
	}

	private void StartPatrol()
	{
		_enemyPatrol.enabled = true;
	}

	private void EndPatrol()
	{
		_enemyPatrol.enabled = false;
	}

	private void StartHunt()
	{
		EndPatrol();
		_enemyHunt.enabled = true;
	}

	private void EndHunt()
	{
		_enemyHunt.enabled = false;
		StartPatrol();
	}

	private bool TryFindHealth(Transform[] _viewPoints, Color lineColor, out Health playerHealth)
	{
		Collider2D collider = GetColliderOnLine(_viewPoints[0].position, _viewPoints[1].position, lineColor);

		if (collider != null)
			if (collider.TryGetComponent(out playerHealth))
				return true;

		playerHealth = null;
		return false;
	}

	private Collider2D GetColliderOnLine(Vector2 startLine, Vector2 endLine, Color lineColor)
	{
		Debug.DrawLine(startLine, endLine, lineColor, _drawLineDelay);
		Collider2D collider = Physics2D.Linecast(startLine, endLine).collider;

		if (collider?.name != gameObject.name)
			return collider;

		return null;
	}

	private void RotateToTarget(Vector2 targetToLook)
	{
		_faceFliper.Flip(targetToLook.x - transform.position.x);
	}

	public GameObject GetTarget()
	{
		if (TryFindHealth(_frontViewPoints, _frontColor, out Health playerHealth))
			return playerHealth.gameObject;
		else if (TryFindHealth(_backViewPoints, _backColor, out playerHealth))
			RotateToTarget(playerHealth.transform.position);

		return playerHealth?.gameObject;
	}
}