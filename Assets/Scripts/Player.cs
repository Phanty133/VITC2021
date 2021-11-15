using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
	public float score = 0f;
	public float speed = 100f;
	public float dashMod = 0.5f;
	public float maxOffset = 0.5f;
	public float collisionTime = 0.5f;
	public GameObject scoreObj;
	private string lastHeld = "";
	private float timer = 0f;
	private bool held = false;
	private Vector2 curOffset;
	private bool recentCollision = false;
	private float collisionTimer;
	private Vector2 collisionPoint;
	private Vector2 knockbackPos;

	public void OnProjectileCollision() {
		Vector2 randVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		knockbackPos = randVector.normalized * maxOffset;
		recentCollision = true;
		collisionTimer = collisionTime;
		collisionPoint = transform.position;
	}

	void FollowCursor(Vector2? offset = null) {
		Vector3 mousePos = Input.mousePosition;
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
		Vector2 posOffset = offset == null ? new Vector2(0,0) : (Vector2)offset;
		Vector3 precisePlayerPos = new Vector3(worldPos.x, worldPos.y, transform.position.z);

		transform.position = precisePlayerPos + new Vector3(posOffset.x, posOffset.y, 0);
	}

	void UpdateScore() {
		scoreObj.GetComponent<TextMeshProUGUI>().text = Mathf.Floor(score * 10f).ToString();
	}

	void ClampPosition() {
		Vector3 poser = transform.position;
		poser.x = Mathf.Clamp(poser.x, -12f, 12f);
		poser.y = Mathf.Clamp(poser.y, -9f, 9f);
		transform.position = poser;
	}

	// Update is called once per frame
	void Update()
	{
		if (recentCollision) {
			if (Vector2.Distance(transform.position, collisionPoint) > maxOffset * 1.25f) { // If the player has moved their mouse, remove offset
				recentCollision = false;
				curOffset = new Vector2(0, 0);
			} else {
				collisionTimer -= Time.deltaTime;
				curOffset = Vector2.Lerp(new Vector2(0, 0), knockbackPos, collisionTimer / collisionTime);
			}
		}

		score += Time.deltaTime;
		UpdateScore();
		FollowCursor(curOffset);

		// timer -= Time.deltaTime;
		// if (Input.GetKey("w"))
		// {
		// 	if (timer > 0 && lastHeld == "w")
		// 	{
		// 		timer = 0;
		// 		Vector3 pos = transform.position;
		// 		pos.y += speed * dashMod;
		// 		transform.position = pos;
		// 	}
		// 	else
		// 	{
		// 		held = true;
		// 		lastHeld = "w";
		// 		Vector3 pos = transform.position;
		// 		pos.y += speed * Time.deltaTime;
		// 		transform.position = pos;
		// 	}
		// }

		// if (Input.GetKey("s"))
		// {
		// 	if (timer > 0 && lastHeld == "s")
		// 	{
		// 		timer = 0;
		// 		Vector3 pos = transform.position;
		// 		pos.y -= speed * dashMod;
		// 		transform.position = pos;
		// 	}
		// 	else
		// 	{
		// 		held = true;
		// 		lastHeld = "s";
		// 		Vector3 pos = transform.position;
		// 		pos.y -= speed * Time.deltaTime;
		// 		transform.position = pos;
		// 	}
		// }


		// if (Input.GetKey("a"))
		// {
		// 	if (timer > 0 && lastHeld == "a")
		// 	{
		// 		timer = 0;
		// 		Vector3 pos = transform.position;
		// 		pos.x -= speed * dashMod;
		// 		transform.position = pos;
		// 	}
		// 	else
		// 	{
		// 		held = true;
		// 		lastHeld = "a";
		// 		Vector3 pos = transform.position;
		// 		pos.x -= speed * Time.deltaTime;
		// 		transform.position = pos;
		// 	}
		// }

		// if (Input.GetKey("d"))
		// {
		// 	if (timer > 0 && lastHeld == "d")
		// 	{
		// 		timer = 0;
		// 		Vector3 pos = transform.position;
		// 		pos.x += speed * dashMod;
		// 		transform.position = pos;
		// 	}
		// 	else
		// 	{
		// 		held = true;
		// 		lastHeld = "d";
		// 		Vector3 pos = transform.position;
		// 		pos.x += speed * Time.deltaTime;
		// 		transform.position = pos;
		// 	}
		// }

		// if (held && !Input.anyKey)
		// {
		// 	timer = 0.1f;
		// 	held = false;
		// }

		ClampPosition();
	}
}
