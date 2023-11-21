using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CallbackTimer 
{
    public static CallbackTimer Create(Action action, float timer)
    {
        GameObject gameObject = new ("CallbackTimer", typeof(MonoBehaviourHook));
        CallbackTimer callbackTimer = new (action, timer, gameObject);
        gameObject.GetComponent<MonoBehaviourHook>().onUpdate = callbackTimer.Update;
        return callbackTimer;
    }

    private class MonoBehaviourHook : MonoBehaviour
    {
        public Action onUpdate;
        private void Update()
        {
            onUpdate?.Invoke();
        }
    }

    private Action action;
    private float timer;
    private GameObject gameObject;
    private bool isDestroyed;

    private CallbackTimer(Action action, float timer, GameObject gameObject)
    {
        this.action = action;
        this.timer = timer;
        this.gameObject = gameObject;
        isDestroyed = false;
    }
    public void Update()
    {
        if(!isDestroyed)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                action();
                DestroySelf();
            }
        }
    }

    private void DestroySelf()
    {
        isDestroyed = true;
        UnityEngine.Object.Destroy(gameObject);
    }
}
