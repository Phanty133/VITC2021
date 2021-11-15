using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unknown : Function
{
	public override string id {
		get { return "unknown"; }
	}

	public override string name {
		get { return "Unknown"; }
	}

	public override int tier {
		get { return 0; }
	}

	public override int paramCount {
		get { return 0; }
	}

	public override int priority {
		get { return 4; }
	}

	protected override bool overrideNotation {
		get { return false; }
	}

	protected override string notationTemplate {
		get { return "x"; }
	}

	public override float Operation(params float[] values)
	{
		return values[0];
	}
}
