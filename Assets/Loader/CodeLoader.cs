using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CodeLoader : MonoBehaviour
{
    private void Awake()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.FullName.IndexOf("GameModel") != -1)
            {
                var type = assembly.GetType("GameModel");
                type.GetMethod("Initialize").Invoke(null, new object[] { });
            }
        }
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.FullName.IndexOf("GameView") != -1)
            {
                var type = assembly.GetType("GameView");
                type.GetMethod("Initialize").Invoke(null, new object[] { });
            }
        }
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.FullName.IndexOf("GameModel") != -1)
            {
                var type = assembly.GetType("GameModel");
                type.GetMethod("Run").Invoke(null, new object[] { });
            }
        }
    }
}
