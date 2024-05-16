using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Scene))]
public class SceneRestarter : MonoBehaviour
{
	[SerializeField] private Health player;
	[SerializeField] private float _restartSceneDelay;

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