using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtFxBase : MonoBehaviour {

	public Vector3 startPos;
	public Vector3 endPos;

	public Transform tran;
	public Callback OnHit = null;

	public float total_time = 2f;
	public float m_time = 0;

	public string fxend = "";

    public IEnumerator Show(Vector3 _starPos, Vector3 _endPos)
    {
        startPos = _starPos;
        endPos = _endPos;

        tran = this.transform;
        tran.position = startPos;

        m_time = 0;


        while (true)
        {
            m_time += Time.deltaTime;

            if (m_time > total_time + 100)
            {
                yield return OnCoroEnd();
                yield break;
            }

            yield return 0;
        }
    }

	public virtual void Init(Vector3 _starPos, Vector3 _endPos, Callback _OnHit = null) {
	
		startPos = _starPos;
		endPos = _endPos;

		tran = this.transform;
		tran.position = startPos;

		OnHit = _OnHit;
		m_time = 0;
	
	}

    bool init = false;

	void Update() {

        if (!init) return;

		m_time += Time.deltaTime;

		if (m_time > total_time)
			OnEnd();	
	
	}

	public void OnEnd() {

		GameObject obj = FxPool.Get(FxEnum.Fx, fxend);

		if (obj != null)
			obj.GetComponent<ArtFxBase>().Init(this.tran.position, this.tran.position);

		if (OnHit != null)
			OnHit();
	
		FxPool.Recover(this.gameObject);
	}

    public IEnumerator OnCoroEnd()
    {

        GameObject obj = FxPool.Get(FxEnum.Fx, fxend);

        if (obj != null)
            yield return obj.GetComponent<ArtFxBase>().Show(this.tran.position, this.tran.position);

        if (OnHit != null)
            OnHit();

        FxPool.Recover(this.gameObject);
    }

}
