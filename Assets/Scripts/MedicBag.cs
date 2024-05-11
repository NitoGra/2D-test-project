using UnityEngine;

public class MedicBag : Pickups
{
	public override void Pickup(PlayerAudio playerAudio)
	{
		base.Pickup(playerAudio);
		playerAudio.MedicBagSound();
	}
}