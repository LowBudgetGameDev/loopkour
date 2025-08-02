using System;
using UnityEngine;

public class FunctionTimer
{
    public static FunctionTimer Create(Action action, float timer)
    {
        GameObject gameObject = new GameObject("FunctionTimer", typeof(MonobehaviorHook));

        FunctionTimer functionTimer = new FunctionTimer(action, timer, gameObject, false);

        gameObject.GetComponent<MonobehaviorHook>().onUpdate = functionTimer.Update;

        return functionTimer;
    }

    public static FunctionTimer Create(Action action, float timer, bool timeScaleDependent)
    {
        GameObject gameObject = new GameObject("FunctionTimer", typeof(MonobehaviorHook));

        FunctionTimer functionTimer = new FunctionTimer(action, timer, gameObject, timeScaleDependent);

        gameObject.GetComponent<MonobehaviorHook>().onUpdate = functionTimer.Update;

        return functionTimer;
    }

    private class MonobehaviorHook : MonoBehaviour
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
    private bool timeScaleDependent;

    private FunctionTimer(Action action, float timer, GameObject gameObject, bool timeScaleDependent)
    {
        this.action = action;
        this.timer = timer;
        this.gameObject = gameObject;
        isDestroyed = false;
        this.timeScaleDependent = timeScaleDependent;
    }

    public void Update()
    {
        if (isDestroyed) return;

        timer -= timeScaleDependent ? Time.deltaTime : Time.unscaledDeltaTime;

        if (timer < 0f)
        {
            action();
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        isDestroyed = true;
        UnityEngine.Object.Destroy(gameObject);
    }
}