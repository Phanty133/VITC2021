using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tangent : Function
{
    public override string id {
		get { return "tan"; }
	}

	public override string name {
		get { return "Tangent"; }
	}

	public override int tier {
		get { return 1; }
	}

	public override int paramCount {
		get { return 1; }
	}

	protected override string notationTemplate {
		get { return "tan({0})"; }
	}

	public override float Operation(params float[] values)
	{
		return Mathf.Tan(values[0]);
	}
}
