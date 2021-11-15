using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollide : MonoBehaviour
{
	private Player player;

	private void Awake() {
		player = transform.parent.GetComponent<Player>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Projectile")
		{
			Player b;

			if (other.gameObject.TryGetComponent<Player>(out b)) return;

			player.score -= 25;
			player.OnProjectileCollision();
			//Destroy(gameObject);
		}
	}
}
