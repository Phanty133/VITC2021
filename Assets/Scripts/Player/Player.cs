using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class Player : MonoBehaviour
{
	[HideInInspector]
	public float score = 0f;
	public float speed = 100f;
	public float dashMod = 0.5f;
	public float maxOffset = 0.5f;
	public float collisionTime = 0.5f;
	public int maxLives = 5;
	public float scoreGoal = 10f;
	public AnimationCurve intensityOverTime;
	public VolumeProfile volumeObj;
	public GameObject scoreObj;
	private string lastHeld = "";
	private float timer = 0f;
	private bool held = false;
	private Vector2 curOffset;
	private bool recentCollision = false;
	private int lives = 0;
	private float nextGoal = 0f;
	private float collisionTimer;
	private Vector2 collisionPoint;
	private Vector2 knockbackPos;

	public void OnProjectileCollision() {
		// The player jumping around after collision feels very weird

		// Vector2 randVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		// knockbackPos = randVector.normalized * maxOffset;
		// recentCollision = true;
		// collisionTimer = collisionTime;
		// collisionPoint = transform.position;

		// Instead, let's just fuck up the entire screen :)
		collisionTimer = collisionTime;
		recentCollision = true;
		lives--;
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

	void LoseGame() {
		Debug.Log("YOU DA LOSER YOU DA LOSER YOU DA LOSER");
	}

	void Start() {
		lives = maxLives;
		nextGoal = scoreGoal;
	}

	// Update is called once per frame
	void Update()
	{
		if (recentCollision) {
			// if (Vector2.Distance(transform.position, collisionPoint) > maxOffset * 1.25f) { // If the player has moved their mouse, remove offset
			// 	recentCollision = false;
			// 	curOffset = new Vector2(0, 0);
			// } else {
			// 	collisionTimer -= Time.deltaTime;
			// 	curOffset = Vector2.Lerp(new Vector2(0, 0), knockbackPos, collisionTimer / collisionTime);
			// }
			if (collisionTimer > 0) {
				collisionTimer -= Time.deltaTime;
				DamageEffect damageEffect;
				if(volumeObj.TryGet(out damageEffect)) damageEffect.intensity.value = intensityOverTime.Evaluate(collisionTimer / collisionTime);
			} else {
				recentCollision = false;
			}
		}

		if(score >= nextGoal / 10f) {
			if(lives < maxLives) {
				lives++;
			}
			nextGoal += scoreGoal;
		} 

		if(lives == 0) {
			LoseGame();
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
