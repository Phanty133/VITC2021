using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosecant : Function
{
	public override string id {
		get { return "csc"; }
	}

	public override string name {
		get { return "Cosecant"; }
	}

	public override int tier {
		get { return 2; }
	}

	public override int paramCount {
		get { return 1; }
	}

	protected override string notationTemplate {
		get { return "csc({0})"; }
	}

	public Cosecant() {}

	public Cosecant(params Function[] funcParams) : base(funcParams) {}

	public override float Operation(params float[] values)
	{
		return 1 / Mathf.Sin(values[0]);
	}
}
