// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Edanoue.Developments.Internal
{
    [DisallowMultipleComponent]
    internal sealed class EdaDebugConsoleRuntimeDrawer : MonoBehaviour
    {
        [Header("Console Options")]
        [SerializeField]
        [Range(1, 100)]
        private int m_lineCount = 20;

        [Header("Prefab Only")]
        [SerializeField]
        private Text m_text = null!;

        private readonly StringBuilder _stringBuilder = new();
        private          bool          _isNeedRebuild;

        private Queue<string> _textQueue = null!;

        private void Awake()
        {
            _textQueue = new Queue<string>(m_lineCount);
        }

        private void LateUpdate()
        {
            if (_textQueue.Count <= 0)
            {
                return;
            }

            if (!_isNeedRebuild)
            {
                return;
            }

            _isNeedRebuild = false;

            _stringBuilder.Clear();
            foreach (var text in _textQueue.Reverse())
            {
                _stringBuilder.Append(text);
                _stringBuilder.Append("\n");
            }

            m_text.text = _stringBuilder.ToString();
        }

        internal void RegisterText(string text)
        {
            _textQueue.Enqueue(text);
            if (_textQueue.Count > m_lineCount)
            {
                _textQueue.Dequeue();
            }

            _isNeedRebuild = true;
        }
    }
}