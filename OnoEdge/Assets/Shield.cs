using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
    public bool shieldIsUp;
    private float switchStateDuration = 0.33f;


    #region unity callbacks
    void Start () {

	}
    #endregion

    #region public
    public void SetShieldState(bool state) {
        if (state == true)
            ShielUp();
        else
            ShielDown();
    }
    #endregion

    #region private
    private void ShielUp() {
        iTween.ScaleTo(gameObject, Vector3.one, switchStateDuration);
        StartCoroutine(ChangeState(true, switchStateDuration));

    }
    private void ShielDown() {
        iTween.ScaleTo(gameObject, Vector3.zero, switchStateDuration);
        StartCoroutine(ChangeState(false, switchStateDuration));
    }

    private IEnumerator ChangeState(bool state, float duration) {
        yield return new WaitForSeconds(duration);

        shieldIsUp = state;
    }
    #endregion
}