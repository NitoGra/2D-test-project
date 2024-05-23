using UnityEngine;
using UnityEngine.UI;

public class BaseHealthBar : MonoBehaviour
{
	[SerializeField] private Slider _sliser;
	[SerializeField] private Health _health;

	public float Value => _sliser.value;
	public void SetValue(float value) => _sliser.value = value;

	public float GetValue => _health.GetValue;
	public float GetMax => _health.GetMax;

	private void OnEnable()
	{
		_health.Healed += ChangeBar;
		_health.Damaged += ChangeBar;
	}

	private void OnDisable()
	{
		_health.Healed -= ChangeBar;
		_health.Damaged -= ChangeBar;
	}

	protected virtual void ChangeBar() { }
}