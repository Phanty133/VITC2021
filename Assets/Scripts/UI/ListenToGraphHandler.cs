using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListenToGraphHandler : MonoBehaviour
{
	public Sprite spriteMute;
	public Sprite spriteUnmute;
	private Toggle toggle;
	private Image image;

	public void OnToggle() {
		if (toggle.isOn) {
			image.sprite = spriteMute;
		} else {
			image.sprite = spriteUnmute;
		}
	}

	private void Awake() {
		toggle = GetComponent<Toggle>();
		image = transform.GetChild(0).GetComponent<Image>();
	}
}
