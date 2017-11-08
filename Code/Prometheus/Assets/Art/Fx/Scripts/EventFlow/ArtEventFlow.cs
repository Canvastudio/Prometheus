using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtEventFlow : MonoBehaviour {

	public List<ArtEvent> eventlist;
	public float destroy_time = 2.0f;

	private ArtEvent cur_event = null;

	private int k = 0;
	private int n = 0;
	private bool isstart = false;


	private float m_time = 0;
	private Vector3 start_pos;
	private Vector3 end_pos;
	private Callback callback;

	public void Init(Vector3 _start_pos, Vector3 _end_pos, Callback _callback){
	
		k = 0;
		n = eventlist.Count;
		cur_event = eventlist[k];
		isstart = true;
		m_time = 0;
		start_pos = _start_pos;
		end_pos = _end_pos;
		callback = _callback;
	
	}

	void Update() {
	
		if (!isstart)
			return;

		m_time += Time.deltaTime;

		if (m_time > destroy_time)
			OnEnd();

		if (cur_event == null)
			return;
		
		if (m_time > cur_event.time)
			OnEvent();

	}

	void OnEvent() {
	
		if (k == n)
			return;

		GameObject objFx = FxPool.Get(FxEnum.Fx, cur_event.fxname);
		ArtFxBase artbase = objFx.GetComponent<ArtFxBase>();

		if (cur_event.isendpos)
			artbase.Init(end_pos, end_pos, cur_event.ishit ? callback : null);
		else
			artbase.Init(start_pos, end_pos, cur_event.ishit ? callback : null);

		k++;
		cur_event = k < n ? eventlist[k] : null;
	
	}

	void OnEnd() {
	
		FxPool.Recover(this.gameObject);
	
	}

}
