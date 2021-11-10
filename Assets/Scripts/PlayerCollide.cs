using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollide : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Projectile")
		{
			Player b;
			if (other.gameObject.TryGetComponent<Player>(out b)) return;
			transform.parent.gameObject.GetComponent<Player>().score -= 25;
			//Destroy(gameObject);
		}
	}
}
