using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiply : Function
{
	public override string id {
		get { return "mul"; }
	}

	public override string name {
		get { return "Multiply"; }
	}

	public override int tier {
		get { return 0; }
	}

	public override int paramCount {
		get { return 2; }
	}

	public override int priority {
		get { return 1; }
	}

	protected override bool overrideNotation {
		get { return false; }
	}

	protected override string notationTemplate {
		get {
			if (children[0].id == "const" && children[1].id != "const") {
				return "{0}{1}";
			} else if (children[0].id != "const" && children[1].id == "const") {
				return "{1}{0}";
			} else {
				return "{0} * {1}";
			}
		}
	}

	public Multiply() {}

	public Multiply(params Function[] funcParams) : base(funcParams) {}

	public override float Operation(params float[] values)
	{
		return values[0] * values[1];
	}
}
