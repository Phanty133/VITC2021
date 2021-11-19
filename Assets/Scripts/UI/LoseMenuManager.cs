using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoseMenuManager : MonoBehaviour
{
	public GameObject loseMenu;
	public GameObject loseMenuScore;

	private Player player;

	public void OpenLoseMenu() {
		loseMenu.SetActive(true);
		loseMenuScore.GetComponent<TextMeshProUGUI>().text = string.Format("Score: {0}", player.ScoreInt.ToString());
	}

	public void CloseLoseMenu() {
		loseMenu.SetActive(false);
	}

	private void Awake() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
}
