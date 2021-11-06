using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosine : Function
{
	public override string id {
		get { return "cos"; }
	}

	public override string name {
		get { return "Cosine"; }
	}

	public override int tier {
		get { return 1; }
	}

	public override int paramCount {
		get { return 1; }
	}

	protected override string notationTemplate {
		get { return "cos({0})"; }
	}

	public override float Operation(params float[] values)
	{
		return Mathf.Cos(values[0]);
	}
}
