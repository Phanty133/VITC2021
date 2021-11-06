using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cotangent : Function
{
	public override string id {
		get { return "cot"; }
	}

	public override string name {
		get { return "Cotangent"; }
	}

	public override int tier {
		get { return 2; }
	}

	public override int paramCount {
		get { return 1; }
	}

	protected override string notationTemplate {
		get { return "cot({0})"; }
	}

	public override float Operation(params float[] values)
	{
		return 1 / Mathf.Tan(values[0]);
	}
}
