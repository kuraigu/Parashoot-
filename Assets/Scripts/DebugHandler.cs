#define ALLOW_DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHandler
{
    public static void Log(object message)
    {
        #if UNITY_EDITOR
            #if ALLOW_DEBUG
                Debug.Log(message);
            #endif
        #endif
    }

    public static void Log(object message, Object context)
    {
        #if UNITY_EDITOR
            #if ALLOW_DEBUG
                Debug.Log(message, context);
            #endif
        #endif
    }
}
