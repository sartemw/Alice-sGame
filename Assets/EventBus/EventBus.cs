using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Assets.EventBus
{
    /*
     *   EventBus.Subscribe<T>(IEvent<T> callback, bool toFirst = false) where T : class, IEventSignal
     *   EventBus.Subscribe<T>(IEvent<T> callback, int toNumber)         where T : class, IEventSignal
     *   EventBus.Subscribe(Type signalType, object eventCallback, bool toFirst = false) 
     *   EventBus.Subscribe(Type signalType, object eventCallback, int toNumber) 
     *   EventBus.Invoke<T>(T signal = null)                             where T : class, IEventSignal
     *   EventBus.Unsubscribe<T>(IEvent<T> callback)                     where T : class, IEventSignal
     *   EventBus.Unsubscribe(Type signalType, object eventCallback)
     *   EventBus.UnsubscribeAll<T>()                                    where T : class, IEventSignal
     *   EventBus.UnsubscribeAll()
     * ____________________________
     *   EventBus.PropertyInit(string propertyName, object property)
     *   ReactiveProperty<T>   EventBus.Property<T>(string propertyName)
     *   ReactiveCollection<T> Collection<T>(string propertyName)
     *   EventBus.PropertyRemove(string propertyName)
     *   EventBus.PropertyRemoveAll()
     * ----------------------------
     * List<string> EventBus.DebugInfoEvents()
     * List<string> EventBus.DebugInfoProperties()
     */

    public interface IEventSignal { }

    public interface IEvent
    {
        public void Invoke(object signal);
    }

    public interface IEvent<T> : IEvent where T : class, IEventSignal
    {
    }

    public class BaseOnEvent<T> : IEvent<T> where T : class, IEventSignal
    {
        public Action<T> onInvoke;

        public BaseOnEvent<T> SetOnInvoke(Action<T> setInvoke)
        {
            onInvoke = setInvoke;
            return this;
        }

        public void Invoke(object signal)
        {
            if(onInvoke != null)
                onInvoke(signal as T);
        }
    }

    public static class EventBus
    {
        public static bool isDebug = false;
        private struct QueueItem
        {
            public WeakReference<object> obj;
            public int toNumber;
        }

        static private Dictionary<int, List<QueueItem>> tempQueueLocked = new Dictionary<int,List<QueueItem>>();


        static private int lastNumType = 0;
        static private Dictionary<Type, int> _eventHash = new Dictionary<Type, int>();
        static private Dictionary<int, List<WeakReference<object>>> _eventCallbacks = new Dictionary<int, List<WeakReference<object>>>();

        static private Dictionary<string, WeakReference<object>> _propertyCallbacks = new Dictionary<string, WeakReference<object>>();

        static private int _TestTypeUseOrCreate(Type eventName)
        {
            if (!_eventHash.ContainsKey(eventName))
                _eventHash.Add(eventName, lastNumType++);
            return _eventHash[eventName];
        }

        static public void Subscribe<T>(IEvent<T> callback, bool toFirst = false) where T : class, IEventSignal
        {
            Subscribe(typeof(T), callback, toFirst);
        }

        static public void Subscribe<T>(IEvent<T> callback, int toNumber) where T : class, IEventSignal
        {
            Subscribe(typeof(T), callback, toNumber);
        }

        static public void Subscribe(Type signalType, object eventCallback, bool toFirst = false)
        {
            Subscribe(signalType, eventCallback, toFirst ? 0 : -1);
        }
        static public void Subscribe(Type signalType, object eventCallback, int toNumber)
        {
            int eventID = _TestTypeUseOrCreate(signalType);
            //Debug.LogFormat("Subscribe initial EVENT! >> {0}#{1} ", signalType, eventID);
            WeakReference<object> refCallback = new WeakReference<object>(eventCallback);

            if (tempQueueLocked.ContainsKey(eventID))
            {
                tempQueueLocked[eventID].Add(new QueueItem { obj = refCallback, toNumber = toNumber });
            }
            else if (_eventCallbacks.ContainsKey(eventID))
            {
                if (toNumber < 0)
                    _eventCallbacks[eventID].Add(refCallback);
                else
                    _eventCallbacks[eventID].Insert(toNumber, refCallback);
            }
            else
                _eventCallbacks.Add(eventID, new List<WeakReference<object>>() { refCallback });
        }



        static public void Invoke<T>(T signal) where T : class, IEventSignal
        {
            InvokeRealization(signal);
        }
        
        static public void Invoke<T>(T signal, double delay) where T : class, IEventSignal
        {
            Task.Delay(TimeSpan.FromSeconds(delay));

            InvokeRealization(signal);
        }

        private static void InvokeRealization<T>(T signal) where T : class, IEventSignal
        {
            int eventID = _TestTypeUseOrCreate(signal.GetType());
            if (tempQueueLocked.ContainsKey(eventID))
            {
                if (isDebug)
                    Debug.LogFormat("Recursive Invoke EVENT! >> {0}#{1} ", signal.GetType(), eventID);
                return;
            }

            if (isDebug)
                Debug.LogFormat("Invoke EVENT! >> {0}#{1} ", signal.GetType(), eventID);

            if (_eventCallbacks.ContainsKey(eventID))
            {
                tempQueueLocked[eventID] = new List<QueueItem>();
                foreach (WeakReference<object> refObj in _eventCallbacks[eventID].ToArray())
                {
                    object obj;
                    if (refObj.TryGetTarget(out obj))
                    {
                        if (obj is IEvent baseEvent)
                            baseEvent.Invoke(signal);
                    }
                }

                while (tempQueueLocked[eventID].Count > 0)
                {
                    List<QueueItem> list = tempQueueLocked[eventID];
                    tempQueueLocked[eventID] = new List<QueueItem>();
                    foreach (QueueItem item in list)
                    {
                        if (item.toNumber < 0)
                            _eventCallbacks[eventID].Add(item.obj);
                        else
                            _eventCallbacks[eventID].Insert(item.toNumber, item.obj);

                        object obj;
                        if (item.obj.TryGetTarget(out obj))
                        {
                            if (obj is IEvent baseEvent)
                                baseEvent.Invoke(signal);
                        }
                    }
                }

                tempQueueLocked.Remove(eventID);
            }
        }

        static public void Unsubscribe(Type signalType, object eventCallback)
        {
            int eventID = _TestTypeUseOrCreate(signalType);
            if (_eventCallbacks.ContainsKey(eventID))
            {
                if(isDebug)
                    Debug.LogFormat("Unsubscribe EVENT! >> {0}#{1} ", signalType, eventID);
                var callbackToDelete = _eventCallbacks[eventID].FirstOrDefault(action => {
                    object obj;
                    action.TryGetTarget(out obj);
                    if(obj != null)
                        return obj.Equals(eventCallback);
                    return false;
                });

                if (callbackToDelete != null)
                    _eventCallbacks[eventID].Remove(callbackToDelete);
                
            }
            else if(isDebug)
                Debug.LogErrorFormat("Trying to unsubscribe for not existing EVENT! {0}#{1} ", signalType, eventID);
        }

        static public void Unsubscribe<T>(IEvent<T> callback) where T : class, IEventSignal
        {
            Unsubscribe(typeof(T), callback);
        }

        static public void UnsubscribeAll(Type type)
        {
            if (_eventHash.ContainsKey(type))
            {
                int eventID = _eventHash[type];
                if(isDebug)
                    Debug.LogFormat("Unsubscribe All EVENT! >> {0}#{1} ", type, eventID);
                if (_eventCallbacks.ContainsKey(eventID))
                    _eventCallbacks[eventID].Clear();
                _eventCallbacks.Remove(eventID);
                _eventHash.Remove(type);
            }
        }

        static public void UnsubscribeAll<T>() where T : IEventSignal
        {
            UnsubscribeAll(typeof(T));
        }

        static public void UnsubscribeAll()
        {
            _eventCallbacks.Clear();
        }

        //---------------------------------------------------------------------------------------------------------

        static public void PropertyInit(string propertyName, object property)
        {
            if (isDebug && _propertyCallbacks.ContainsKey(propertyName))
                Debug.LogErrorFormat("This property already exists! {0} ", propertyName);

            _propertyCallbacks[propertyName] = new WeakReference<object>(property);
        }

        static public ReactiveProperty<T> Property<T>(string propertyName)
        {
            if (_propertyCallbacks.ContainsKey(propertyName))
            {
                object target;
                if (_propertyCallbacks[propertyName].TryGetTarget(out target))
                    return target as ReactiveProperty<T>;
                else if(isDebug)
                    Debug.LogErrorFormat("Attempt to subscribe to an unavailable variable! {0} ", propertyName);
                return null;
            }
            if(isDebug)
                Debug.LogErrorFormat("This property does not exist, to SUBSCRIBE the value! {0} ", propertyName);
            return null;
        }

        static public ReactiveCollection<T> Collection<T>(string propertyName)
        {
            if (_propertyCallbacks.ContainsKey(propertyName))
            {
                object target;
                if (_propertyCallbacks[propertyName].TryGetTarget(out target))
                    return target as ReactiveCollection<T>;
                else if(isDebug)
                    Debug.LogErrorFormat("Attempt to subscribe to an unavailable variable! {0} ", propertyName);
                return null;
            }
            if(isDebug)
                Debug.LogErrorFormat("This property does not exist, to SUBSCRIBE the value! {0} ", propertyName);
            return null;
        }

        //Unsubscribe
        static public void PropertyRemove(string propertyName)
        {
            _propertyCallbacks.Remove(propertyName);
        }

        static public void PropertyRemoveAll()
        {
            _propertyCallbacks.Clear();
        }

        //---------------------------------------------------------------------------------------------------------
        static public List<string> DebugInfoEvents()
        {
            List<string> result = new List<string>();
            foreach (Type eventName in _eventHash.Keys)
                foreach (int eventID in _eventCallbacks.Keys)
                    result.Add($"{eventName}#{eventID} ({_eventCallbacks[eventID].Count()})");
            return result;
        }

        static public List<string> DebugInfoProperties()
        {
            List<string> result = new List<string>();
            foreach (string propertyName in _propertyCallbacks.Keys)
                result.Add(propertyName);
            return result;
        }
    }
}