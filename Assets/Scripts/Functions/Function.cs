using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Function {
	public abstract string id { get; }
	public abstract string name { get; }
	public abstract int tier { get; }
	public abstract int paramCount { get; }
	protected abstract string notationTemplate { get; }

	public List<Function> children = new List<Function>(2);
	public abstract float Operation(params float[] values);

	public Function(params Function[] funcParams) {
		children = new List<Function>(funcParams);
	}

	public float Process(float a) {
		switch (paramCount) {
			case 0:
				return Operation(a);
			case 1:
				return Operation(children[0].Process(a));
			case 2:
				return Operation(children[0].Process(a), children[1].Process(a));
			default:
				throw new System.Exception("Invalid parameter count!");
		}
	}

	public string GetNotation() {
		switch (paramCount) {
			case 0:
				return notationTemplate;
			case 1:
				return string.Format(notationTemplate, children[0].GetNotation());
			case 2:
				return string.Format(notationTemplate, children[0].GetNotation(), children[1].GetNotation());
			default:
				throw new System.Exception("Invalid parameter count!");
		}
	}
}
