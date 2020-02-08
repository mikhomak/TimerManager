using System;
using General;
using UnityEngine;
    using System.Collections.Concurrent;


public class TimerManager : MonoBehaviour {
    private ObjectPool<Timer> timerPool;

    public static TimerManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        timerPool = new ObjectPool<Timer>(10);
    }


    void Update() {
        foreach (var timer in timerPool.getAll()) {
            timer.updateTimer(Time.deltaTime);
        }
    }


    public void releaseFromPool(Timer timer) {
        timer.resetTimer();
        timerPool.release(timer);
    }

    public void startTimer(float time, Action timerEndEvent) {
        Timer timer = timerPool.get();
        timer.Action = timerEndEvent;
        timer.EndTime = time;
        timer.TimerManager = this;
    }



    /*
            Timer class
            You can move it to a different file
    */
    public class Timer {
        private float endTime;
        private Action action;
        private TimerManager timerManager;


        public TimerManager TimerManager {
            set => timerManager = value;
        }

        public float EndTime {
            set => endTime = value;
        }

        public Action Action {
            set => action = value;
        }

        public void updateTimer(float time) {
            endTime -= time;
            if (endTime < 0) {
                action?.Invoke();
                if (timerManager != null) {
                    timerManager.releaseFromPool(this);
                }
            }
        }

        public void resetTimer() {
            endTime = 0;
            action = null;
            timerManager = null;
        }
    }




    /*
            Object Pool
            It's generic so you can use for other uses
            You can move it to a different file
    */
    public class ObjectPool<T> where T : new() {
        private readonly ConcurrentBag<T> items = new ConcurrentBag<T>();
        private int counter = 0;
        private readonly int MAX;

        public ObjectPool(int max) {
            MAX = max;
        }

        public void release(T item) {
            if (counter < MAX) {
                items.Add(item);
                counter++;
            }
        }

        public T get() {
            if (items.TryTake(out var item)) {
                counter--;
                return item;
            }
            else {
                T obj = new T();
                items.Add(obj);
                counter++;
                return obj;
            }
        }

        public ConcurrentBag<T> getAll() {
            return items;
        }
    }


}

