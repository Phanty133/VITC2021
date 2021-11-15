using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Add : Function {
	public override string id {
		get { return "add"; }
	}

	public override string name {
		get { return "Addition"; }
	}

	public override int tier {
		get { return 0; }
	}

	public override int paramCount {
		get { return 2; }
	}

	public override int priority {
		get { return 0; }
	}

	protected override bool overrideNotation {
		get { return true; }
	}

	protected override string notationTemplate { // An absolute mess
		get {
			if (children[0].id == "const" || children[1].id == "const") {
				if (children[0].id == "const" && children[0].Process(0) == 0) {
					return string.Format("{1}", "", children[1].GetNotation());
				} else if (children[1].id == "const" && children[1].Process(0) == 0) {
					return string.Format("{0}", children[0].GetNotation(), "");
				} else if (children[1].id == "const" && children[1].Process(0) < 0) {
					return string.Format("{0} - {1}", children[0].GetNotation(), children[1].GetNotation().Substring(1));
				} else if (children[0].id == "const" && children[0].Process(0) < 0) {
					return string.Format("{1} - {0}", children[1].GetNotation(), children[0].GetNotation().Substring(1));
				} else {
					return DefaultGetNotation("{0} + {1}");
				}
			} else {
				return DefaultGetNotation("{0} + {1}");
			}
		}
	}

	public Add() {}

	public Add(params Function[] funcParams) : base(funcParams) {}

	public override float Operation(params float[] values)
	{
		return values[0] + values[1];
	}
}