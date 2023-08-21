// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using System;
using UnityEngine;

namespace Edanoue
{
    /// <summary>
    /// </summary>
    [Serializable]
    public struct EdaSceneObject
    {
        [SerializeField]
        private string m_sceneName;

        public static implicit operator string(EdaSceneObject edaSceneObject)
        {
            return edaSceneObject.m_sceneName;
        }

        public static implicit operator EdaSceneObject(string sceneName)
        {
            return new EdaSceneObject { m_sceneName = sceneName };
        }
    }
}