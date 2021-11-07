using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Projectile") {
			Destroy(gameObject);
		}
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		Vector3 mousePos = Input.mousePosition;
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
		transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
	}
}
