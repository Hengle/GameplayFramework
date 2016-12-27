﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 用于面板显示属性
/// </summary>
/// <typeparam name="T">Type that this editor can edit.</typeparam>
public abstract class ShowPropertyEditor<T>:Editor
    where T:MonoBehaviour
{
    protected static bool IsGetProp = false;

    /// <summary>
    /// 需要显示在面板上的属性
    /// </summary>
    protected static List<PropertyInfo> PropertyCollection { get; set; } = new List<PropertyInfo>();

    public override void OnInspectorGUI()
    {
        lock (PropertyCollection)
        {
            if (!IsGetProp)
            {
                var collection = typeof(T).GetProperties();

                foreach (var item in collection)
                {
                    if (!MonoBehaviourExtention.PropertiesNames.Contains(item.Name))
                    {
                        PropertyCollection.Add(item);
                    }
                }

                IsGetProp = true;
            }
        }

        foreach (var item in PropertyCollection)
        {
            if (item.CanRead)
            {
                var res = item.GetValue(target, null);
                EditorGUILayout.LabelField($"{item.Name}: {res.ToString()}");
            }
        }

        EditorGUILayout.Space();
        base.OnInspectorGUI();
    }
}