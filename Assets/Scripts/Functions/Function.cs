using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Function {
	public abstract string id { get; }
	public abstract string name { get; }
	public abstract int tier { get; }
	public abstract int paramCount { get; }
	public abstract int priority { get; }
	protected abstract bool overrideNotation { get; }
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
		if (overrideNotation) {
			return notationTemplate;
		} else {
			return DefaultGetNotation(notationTemplate);
		}
	}

	protected string DefaultGetNotation(string funcTemplate) {
		switch (paramCount) {
			case 0:
				return funcTemplate;
			case 1:
				return string.Format(funcTemplate, children[0].GetNotation());
			case 2:
				bool parentheses1 = children[0].priority <= priority;
				bool parentheses2 = children[1].priority <= priority;
				string ch1 = children[0].GetNotation();
				string ch2 = children[1].GetNotation();

				return string.Format(
					funcTemplate,
					parentheses1 ? "(" + ch1 + ")" : ch1,
					parentheses2 ? "(" + ch2 + ")" : ch2
				);
			default:
				throw new System.Exception("Invalid parameter count!");
		}
	}
}
