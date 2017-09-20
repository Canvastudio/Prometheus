using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroCore : SingleGameObject<CoroCore>{

	public Coroutine StartCoro(IEnumerator coro)
	{
		return StartCoroutine(coro);
	}

	public IEnumerator StartInnerCoro(IEnumerator coro)
	{
		if (coro != null)
		{
			yield return StartCoroutine(coro);
		}
		else
		{
			yield return null;
		}
	}

	public void StopCoro(IEnumerator coro)
	{
		StopCoroutine(coro);
	}

	public void StopCoro(Coroutine coro)
	{
		StopCoroutine(coro);
	}

	public WaitForSeconds waitForOneSecone = new WaitForSeconds(1f);
	public WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
}
