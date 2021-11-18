using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHalo : MonoBehaviour
{
	float transparency = 0f;
	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Projectile")
		{
			transform.parent.gameObject.GetComponent<Player>().score += Mathf.Pow(2f, other.GetComponent<ProjMovement>().tier) * Time.deltaTime;
			transparency = 1f;
		}
	}
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (transparency > 0) transparency -= 2f * Time.deltaTime;
		if (transparency < 0) transparency = 0;
		gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_Alpha", transparency);
	}
}
