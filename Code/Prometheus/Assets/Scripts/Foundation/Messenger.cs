// Messenger.cs v0.1 (20090925) by Rod Hyde (badlydrawnrod).
//
// This is a C# messenger (notification center) for Unity. It uses delegates
// and generics to provide type-checked messaging between event producers and
// event consumers, without the need for producers or consumers to be aware of
// each other.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Callback();
public delegate void Callback<T>(T arg1);
public delegate IEnumerator CoroutineCallback<T>(T arg1);
public delegate void Callback<T, U>(T arg1, U arg2);
public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);
/**
 * A messenger for events that have no parameters.
 */
static public class Messenger
{
    private static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

    static public void AddListener(string eventType, Callback handler)
    {
        // Obtain a lock on the event table to keep this thread-safe.
        lock (eventTable)
        {
            // Create an entry for this event type if it doesn't already exist.
            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }
            // Add the handler to the event.
            eventTable[eventType] = (Callback)eventTable[eventType] + handler;
        }
    }

    static public void RemoveListener(string eventType, Callback handler)
    {
        // Obtain a lock on the event table to keep this thread-safe.
        lock (eventTable)
        {
            // Only take action if this event type exists.
            if (eventTable.ContainsKey(eventType))
            {
                // Remove the event handler from this event.
                eventTable[eventType] = (Callback)eventTable[eventType] - handler;

                // If there's nothing left then remove the event type from the event table.
                if (eventTable[eventType] == null)
                {
                    eventTable.Remove(eventType);
                }
            }
        }
    }

    static public void Invoke(string eventType)
    {
        Delegate d;
        // Invoke the delegate only if the event type is in the dictionary.
        if (eventTable.TryGetValue(eventType, out d))
        {
            // Take a local copy to prevent a race condition if another thread
            // were to unsubscribe from this event.
            Callback callback = (Callback)d;

            // Invoke the delegate if it's not null.
            if (callback != null)
            {
                callback();
            }
        }
    }
}


/**
 * A messenger for events that have one parameter of type T.
 */
static public class Messenger<T>
{
    private static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

    static public void AddListener(string eventType, Callback<T> handler)
    {
        // Obtain a lock on the event table to keep this thread-safe.
        lock (eventTable)
        {
            // Create an entry for this event type if it doesn't already exist.
            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }
            // Add the handler to the event.
            eventTable[eventType] = (Callback<T>)eventTable[eventType] + handler;
        }
    }

    static public void RemoveListener(string eventType, Callback<T> handler)
    {
        // Obtain a lock on the event table to keep this thread-safe.
        lock (eventTable)
        {
            // Only take action if this event type exists.
            if (eventTable.ContainsKey(eventType))
            {
                // Remove the event handler from this event.
                eventTable[eventType] = (Callback<T>)eventTable[eventType] - handler;

                // If there's nothing left then remove the event type from the event table.
                if (eventTable[eventType] == null)
                {
                    eventTable.Remove(eventType);
                }
            }
        }
    }

    static public void Invoke(string eventType, T arg1)
    {
        Delegate d;
        // Invoke the delegate only if the event type is in the dictionary.
        if (eventTable.TryGetValue(eventType, out d))
        {
            // Take a local copy to prevent a race condition if another thread
            // were to unsubscribe from this event.
            Callback<T> callback = (Callback<T>)d;

            // Invoke the delegate if it's not null.
            if (callback != null)
            {
                callback(arg1);
            }
        }
    }

    /// <summary>
    /// 用在协程中等待并执行，执行完之后携程继续
    /// </summary>
    public class WaitForMsg : CustomYieldInstruction
    {
        bool finish_call;

        IEnumerator DoHanlder(CoroutineCallback<T> handler, T arg)
        {
            yield return CoroCore.Instance.StartCoro(handler(arg));

            finish_call = true;

        }
        public WaitForMsg(string msg)
        {
            Messenger<T>.AddListener(msg, (T arg) =>
            {
                finish_call = true;  
            });
        }

        public WaitForMsg(string msg, Callback<T> handler)
        {
            Messenger<T>.AddListener(msg, (T arg) =>
            {
                finish_call = true;

                if (handler != null)
                {
                    handler.Invoke(arg);
                }
            });
        }

        public WaitForMsg(string msg, CoroutineCallback<T> handler)
        {
            Messenger<T>.AddListener(msg, (T arg) =>
            {
                if (handler != null)
                {
                    CoroCore.Instance.StartCoroutine(DoHanlder(handler, arg));
                }
                else
                {
                    finish_call = true;
                }
            });
        }

        public override bool keepWaiting
        {
            get
            {
                return !finish_call;
            }
        }
    }

}


/**
 * A messenger for events that have two parameters of types T and U.
 */
static public class Messenger<T, U>
{
    private static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

    static public void AddListener(string eventType, Callback<T, U> handler)
    {
        // Obtain a lock on the event table to keep this thread-safe.
        lock (eventTable)
        {
            // Create an entry for this event type if it doesn't already exist.
            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }
            // Add the handler to the event.
            eventTable[eventType] = (Callback<T, U>)eventTable[eventType] + handler;
        }
    }

    static public void RemoveListener(string eventType, Callback<T, U> handler)
    {
        // Obtain a lock on the event table to keep this thread-safe.
        lock (eventTable)
        {
            // Only take action if this event type exists.
            if (eventTable.ContainsKey(eventType))
            {
                // Remove the event handler from this event.
                eventTable[eventType] = (Callback<T, U>)eventTable[eventType] - handler;

                // If there's nothing left then remove the event type from the event table.
                if (eventTable[eventType] == null)
                {
                    eventTable.Remove(eventType);
                }
            }
        }
    }

    static public void Invoke(string eventType, T arg1, U arg2)
    {
        Delegate d;
        // Invoke the delegate only if the event type is in the dictionary.
        if (eventTable.TryGetValue(eventType, out d))
        {
            // Take a local copy to prevent a race condition if another thread
            // were to unsubscribe from this event.
            Callback<T, U> callback = (Callback<T, U>)d;

            // Invoke the delegate if it's not null.
            if (callback != null)
            {
                callback(arg1, arg2);
            }
        }
    }
}


/**
 * A messenger for events that have two parameters of types T and U.
 */
static public class Messenger<T, U, V>
{
	private static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();
	
	static public void AddListener(string eventType, Callback<T, U, V> handler)
	{
		// Obtain a lock on the event table to keep this thread-safe.
		lock (eventTable)
		{
			// Create an entry for this event type if it doesn't already exist.
			if (!eventTable.ContainsKey(eventType))
			{
				eventTable.Add(eventType, null);
			}
			// Add the handler to the event.
			eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] + handler;
		}
	}
	
	static public void RemoveListener(string eventType, Callback<T, U, V> handler)
	{
		// Obtain a lock on the event table to keep this thread-safe.
		lock (eventTable)
		{
			// Only take action if this event type exists.
			if (eventTable.ContainsKey(eventType))
			{
				// Remove the event handler from this event.
				eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] - handler;
				
				// If there's nothing left then remove the event type from the event table.
				if (eventTable[eventType] == null)
				{
					eventTable.Remove(eventType);
				}
			}
		}
	}

	static public void Invoke(string eventType, T arg1, U arg2, V arg3)
	{
		Delegate d;
		// Invoke the delegate only if the event type is in the dictionary.
		if (eventTable.TryGetValue(eventType, out d))
		{
			// Take a local copy to prevent a race condition if another thread
			// were to unsubscribe from this event.
			Callback<T, U, V> callback = (Callback<T, U, V>)d;
			
			// Invoke the delegate if it's not null.
			if (callback != null)
			{
				callback(arg1, arg2, arg3);
			}
		}
	}
}