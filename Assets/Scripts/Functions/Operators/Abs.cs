using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abs : Function
{
	public override string id {
		get { return "abs"; }
	}

	public override string name {
		get { return "Absolute"; }
	}

	public override int tier {
		get { return 2; }
	}

	public override int paramCount {
		get { return 1; }
	}

	public override int priority {
		get { return 3; }
	}

	protected override bool overrideNotation {
		get { return false; }
	}

	protected override string notationTemplate {
		get { return "|{0}|"; }
	}

	public Abs() {}

	public Abs(params Function[] funcParams) : base(funcParams) {}

	public override float Operation(params float[] values)
	{
		return Mathf.Abs(values[0]);
	}
}