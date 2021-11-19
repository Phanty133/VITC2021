using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeTracker : MonoBehaviour
{
	public int life;
	public GameObject playerObj;

	// Update is called once per frame
	void Update()
	{
		if (playerObj.GetComponent<Player>().lives < life) gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
		else gameObject.GetComponent<Image>().color = new Color(1, 0.72f, 0.72f, 1);
	}
}
