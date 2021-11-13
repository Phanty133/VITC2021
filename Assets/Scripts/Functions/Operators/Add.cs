using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Add : Function {
	public override string id {
		get { return "add"; }
	}

	public override string name {
		get { return "Addition"; }
	}

	public override int tier {
		get { return 0; }
	}

	public override int paramCount {
		get { return 2; }
	}

	protected override string notationTemplate {
		get { return "({0} + {1})"; }
	}

	public Add() {}

	public Add(params Function[] funcParams) : base(funcParams) {}

	public override float Operation(params float[] values)
	{
		return values[0] + values[1];
	}
}