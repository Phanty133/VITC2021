using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sine : Function
{
	public override string id {
		get { return "sin"; }
	}

	public override string name {
		get { return "Sine"; }
	}

	public override int tier {
		get { return 1; }
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
		get { return "sin({0})"; }
	}

	public Sine() {}

	public Sine(params Function[] funcParams) : base(funcParams) {}

	public override float Operation(params float[] values)
	{
		return Mathf.Sin(values[0]);
	}
}
