using UnityEngine;
using System.Collections;

public class Panel_Script : MonoBehaviour {

    public GameObject Panel;

	public bool _InEnum = false;

    public void OnClickCalled()
    {
        if(!_InEnum)StartCoroutine(ChangeSizeToMax());

    }

    IEnumerator ChangeSizeToMax()
    {
		_InEnum = true;

        for(float timer = 0; timer < 1.0f; timer += Time.deltaTime)
        {
            Panel.transform.localScale = Vector3.Lerp(Panel.transform.localScale, new Vector3(1, 1, 1), 0.5f * timer);
            yield return null;
        }

        StartCoroutine(WaitForDecrease());
    }

    IEnumerator ChangeSizeToMin()
    {
        for (float timer = 0; timer < 1.0f; timer += Time.deltaTime)
        {
            Panel.transform.localScale = Vector3.Lerp(Panel.transform.localScale, new Vector3(0,0,0), 0.5f * timer);
            yield return null;
        }
		_InEnum = false;
    }

    IEnumerator WaitForDecrease()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(ChangeSizeToMin());
    }
}
