using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Divide : Function
{
	public override string id {
		get { return "div"; }
	}

	public override string name {
		get { return "Divide"; }
	}

	public override int tier {
		get { return 0; }
	}

	public override int paramCount {
		get { return 2; }
	}

	protected override string notationTemplate {
		get { return "({0} / {1})"; }
	}

	public Divide() {}

	public Divide(params Function[] funcParams) : base(funcParams) {}

	public override float Operation(params float[] values)
	{
		return values[0] / values[1];
	}
}
