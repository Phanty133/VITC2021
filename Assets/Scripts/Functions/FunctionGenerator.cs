using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FunctionGenerator
{
	public Dictionary<string, Type> funcTypes = new Dictionary<string, Type>();

	private void LoadFunctions() {
		Type generalType = typeof(Function);
		Type[] types = Assembly.GetAssembly(generalType).GetTypes();
		Type[] inheritingTypes = types.Where(t => t.IsSubclassOf(generalType)).ToArray();

		foreach (Type type in inheritingTypes) {
			Function func = (Function) Activator.CreateInstance(type);
			funcTypes.Add(func.id, type);
		}
	}

	public FunctionGenerator() {
		LoadFunctions();
	}

	public Function Generate(int maxDepth = 3, int curDepth = 0) {
		Type funcType = funcTypes.ElementAt(UnityEngine.Random.Range(0, funcTypes.Count)).Value;
		Function func = (Function)Activator.CreateInstance(funcType);

		if (func.paramCount != 0) {
			if (curDepth == maxDepth) {
				func.children.Insert(0, (Function)Activator.CreateInstance(typeof(Unknown)));

				if (func.paramCount == 2) {
					func.children.Insert(1, (Function)Activator.CreateInstance(typeof(Constant)));
				}
			} else {
				for (int i = 0; i < func.paramCount; i++) {
					func.children.Add(Generate(maxDepth, curDepth + 1));
				}
			}
		}

		return func;
	}
}
