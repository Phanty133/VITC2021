using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : Function
{
    public override string id {
		get { return "pow"; }
	}

	public override string name {
		get { return "Addition"; }
	}

	public override int tier {
		get { return 2; }
	}

	public override int paramCount {
		get { return 2; }
	}

	protected override string notationTemplate {
		get { return "({0}^{{1}})"; }
	}

	public override float Operation(params float[] values)
	{
		return Mathf.Pow(values[0], values[1]);
	}
}
