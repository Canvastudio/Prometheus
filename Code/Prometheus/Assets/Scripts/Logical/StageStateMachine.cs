using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStateMachine : MonoBehaviour {


}

public enum StageState
{
	NIL,
	LOADING,
	INIT,
	PLAYER,
	MONSTER,
	MAP,
	END,
	PAUSE,
}