using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secant : Function
{
	public override string id {
		get { return "sec"; }
	}

	public override string name {
		get { return "Secant"; }
	}

	public override int tier {
		get { return 2; }
	}

	public override int paramCount {
		get { return 1; }
	}

	public override int priority {
		get { return 2; }
	}

	protected override bool overrideNotation {
		get { return false; }
	}

	protected override string notationTemplate {
		get { return "sec({0})"; }
	}

	public Secant() {}

	public Secant(params Function[] funcParams) : base(funcParams) {}

	public override float Operation(params float[] values)
	{
		return 1 / Mathf.Cos(values[0]);
	}
}
