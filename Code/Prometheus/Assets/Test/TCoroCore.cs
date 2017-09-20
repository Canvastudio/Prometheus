using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCoroCore : SingleGameObject<TCoroCore> {

	public Coroutine StartCoro(IEnumerator coro)
	{
		return StartCoroutine(coro);
	}

	public IEnumerator StartInnerCoro(IEnumerator coro)
	{
		yield return StartCoroutine(coro);
	}

	public void StopCoro(IEnumerator coro)
	{
		StopCoroutine(coro);
	}

	public void StopCoro(Coroutine coro)
	{
		StopCoroutine(coro);
	}
}
