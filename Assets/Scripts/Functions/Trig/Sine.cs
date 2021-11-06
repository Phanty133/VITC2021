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

	protected override string notationTemplate {
		get { return "sin({0})"; }
	}

	public override float Operation(params float[] values)
	{
		return Mathf.Sin(values[0]);
	}
}
