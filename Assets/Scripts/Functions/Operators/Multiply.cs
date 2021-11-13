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

	protected override string notationTemplate {
		get { return "({0} * {1})"; }
	}

	public Multiply() {}

	public Multiply(params Function[] funcParams) : base(funcParams) {}

	public override float Operation(params float[] values)
	{
		return values[0] * values[1];
	}
}
