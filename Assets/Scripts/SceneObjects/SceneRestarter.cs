using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Scene))]
public class SceneRestarter : MonoBehaviour
{
	[SerializeField] private Health player;

	private float _restartSceneDelay = 7f;

	private void OnEnable()
	{
		player.Died += DeferredRestart;
	}

	private void OnDisable()
	{
		player.Died -= DeferredRestart;
	}

	private void DeferredRestart()
	{
		Invoke(nameof(Restart), _restartSceneDelay);
	}

	private void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
