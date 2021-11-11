using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
	public float score = 0f;
	public float speed = 100f;
	public float dashMod = 0.5f;
	public GameObject scoreObj;
	private string lastHeld = "";
	private float timer = 0f;
	private bool held = false;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		score += Time.deltaTime;
		scoreObj.GetComponent<TextMeshProUGUI>().text = Mathf.Floor(score * 10f).ToString();
		// Vector3 mousePos = Input.mousePosition;
		// Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
		// transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
		timer -= Time.deltaTime;
		if (Input.GetKey("w"))
		{
			if (timer > 0 && lastHeld == "w")
			{
				timer = 0;
				Vector3 pos = transform.position;
				pos.y += speed * dashMod;
				transform.position = pos;
			}
			else
			{
				held = true;
				lastHeld = "w";
				Vector3 pos = transform.position;
				pos.y += speed * Time.deltaTime;
				transform.position = pos;
			}
		}

		if (Input.GetKey("s"))
		{
			if (timer > 0 && lastHeld == "s")
			{
				timer = 0;
				Vector3 pos = transform.position;
				pos.y -= speed * dashMod;
				transform.position = pos;
			}
			else
			{
				held = true;
				lastHeld = "s";
				Vector3 pos = transform.position;
				pos.y -= speed * Time.deltaTime;
				transform.position = pos;
			}
		}


		if (Input.GetKey("a"))
		{
			if (timer > 0 && lastHeld == "a")
			{
				timer = 0;
				Vector3 pos = transform.position;
				pos.x -= speed * dashMod;
				transform.position = pos;
			}
			else
			{
				held = true;
				lastHeld = "a";
				Vector3 pos = transform.position;
				pos.x -= speed * Time.deltaTime;
				transform.position = pos;
			}
		}

		if (Input.GetKey("d"))
		{
			if (timer > 0 && lastHeld == "d")
			{
				timer = 0;
				Vector3 pos = transform.position;
				pos.x += speed * dashMod;
				transform.position = pos;
			}
			else
			{
				held = true;
				lastHeld = "d";
				Vector3 pos = transform.position;
				pos.x += speed * Time.deltaTime;
				transform.position = pos;
			}
		}

		if (held && !Input.anyKey)
		{
			timer = 0.1f;
			held = false;
		}

		Vector3 poser = transform.position;
		poser.x = Mathf.Clamp(poser.x, -12f, 12f);
		poser.y = Mathf.Clamp(poser.y, -9f, 9f);
		transform.position = poser;
	}
}
