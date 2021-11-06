using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ArrayStuff
{
	public static T[] Flatten<T>(this Array data) {
		var list = new List<T>();
		var stack = new Stack<IEnumerator>();
		stack.Push(data.GetEnumerator());
		do {
			for (var iterator = stack.Pop(); iterator.MoveNext();) {
				if (iterator.Current is Array) {
					stack.Push(iterator);
					iterator = (iterator.Current as IEnumerable).GetEnumerator();
				}
				else
					list.Add((T)iterator.Current);
			}
		}
		while (stack.Count > 0);
		return list.ToArray();
	}
}
