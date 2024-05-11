using UnityEngine;

public class Coin : Pickups
{
	public override void Pickup(PlayerAudio playerAudio)
	{
		base.Pickup(playerAudio);
		playerAudio.CoinSound();
	}
}