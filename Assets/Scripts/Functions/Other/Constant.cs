using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant : Function
{
	public override string id {
		get { return "const"; }
	}

	public override string name {
		get { return "Constant"; }
	}

	public override int tier {
		get { return 0; }
	}

	public override int paramCount {
		get { return 0; }
	}

	protected override string notationTemplate {
		get { return value.ToString(); }
	}

	private float value;

	public Constant() {
		value = Random.Range(-7, 8);
	}

	public Constant(float defaultVal) {
		value = (float)defaultVal;
	}

	public override float Operation(params float[] values)
	{
		return value;
	}
}
