using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class ExtensionMethod
{
    public static Coroutine DelayToInvoke(this MonoBehaviour behaviour, Action action, float delaySeconds)
    {
        return behaviour.StartCoroutine(DelayToInvokeDo(action, delaySeconds));
    }

    public static IEnumerator DelayToInvokeDo(Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        action?.Invoke();
    }

}
