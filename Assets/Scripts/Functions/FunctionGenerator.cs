using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FunctionGenerator
{
	public static Dictionary<string, Type> funcTypes = new Dictionary<string, Type>();
	public static Dictionary<int, List<string>> funcTiers = new Dictionary<int, List<string>>();

	private static void LoadFunctions() {
		Type generalType = typeof(Function);
		Type[] types = Assembly.GetAssembly(generalType).GetTypes();
		Type[] inheritingTypes = types.Where(t => t.IsSubclassOf(generalType)).ToArray();

		foreach (Type type in inheritingTypes) {
			Function func = (Function) Activator.CreateInstance(type);
			funcTypes.Add(func.id, type);

			if (!funcTiers.ContainsKey(func.tier)) {
				funcTiers[func.tier] = new List<string>();
			}

			funcTiers[func.tier].Add(func.id);
		}
	}

	private static List<string> GetFunctionsInTier(int tier) {
		List<string> output = new List<string>();

		for (int i = 0; i < tier; i++) {
			if (funcTiers.ContainsKey(i)) {
				output.AddRange(funcTiers[i]);
			}
		}

		return output;
	}

	public static Function Generate(int tier = 100, int maxDepth = 3, int curDepth = 0) {
		if (funcTypes.Count == 0) {
			LoadFunctions();
		}

		List<string> funcsInTier = GetFunctionsInTier(tier);
		string funcId = funcsInTier.ElementAt(UnityEngine.Random.Range(0, funcsInTier.Count));
		Type funcType = funcTypes[funcId];
		Function func = (Function)Activator.CreateInstance(funcType);

		if (func.paramCount != 0) {
			if (curDepth == maxDepth) {
				func.children.Insert(0, (Function)Activator.CreateInstance(typeof(Unknown)));

				if (func.paramCount == 2) {
					func.children.Insert(1, (Function)Activator.CreateInstance(typeof(Constant)));
				}
			} else {
				for (int i = 0; i < func.paramCount; i++) {
					func.children.Add(FunctionGenerator.Generate(tier, maxDepth, curDepth + 1));
				}
			}
		}

		return func;
	}
}
