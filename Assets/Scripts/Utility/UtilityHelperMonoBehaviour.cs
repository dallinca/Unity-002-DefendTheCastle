using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityHelperMonoBehaviour : MonoBehaviour {

    public delegate bool MyFunction(string x);

    public void StartCoroutineWaitToRunFunction(float secondsUntilFunctionCall, MyFunction functionToRun, string parameter1) {
        StartCoroutine(WaitToRunFunction(secondsUntilFunctionCall, functionToRun, parameter1));
    }

    public IEnumerator WaitToRunFunction(float secondsUntilFunctionCall, MyFunction functionToRun, string parameter1) {
        yield return new WaitForSeconds(secondsUntilFunctionCall);
        functionToRun(parameter1);
    }
}
