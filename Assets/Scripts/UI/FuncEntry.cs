using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FuncEntry : MonoBehaviour
{
	public void SetGraph(Function function, Color color) {
		TextMeshProUGUI text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
		Image colorImg = transform.Find("Color").GetComponent<Image>();

		colorImg.color = color;
		text.text = "y = " + function.GetNotation();
	}
}
