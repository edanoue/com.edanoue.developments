// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Edanoue.Developments.Internal
{
    [DisallowMultipleComponent]
    internal sealed class EdaDebugConsoleRuntimeDrawer : MonoBehaviour
    {
        [Header("Console Options")]
        [SerializeField]
        [Range(1, 100)]
        [EdaLabel("Max Line Count")]
        private int m_lineCount = 20;

        [Header("Prefab Only")]
        [SerializeField]
        private TextMesh m_text = null!;

        private readonly StringBuilder _stringBuilder = new();
        private          bool          _isDirty;

        private Queue<string> _textQueue = null!;

        private void Awake()
        {
            _textQueue = new Queue<string>(m_lineCount);
            m_text.text = string.Empty;
        }

        private void LateUpdate()
        {
            if (_textQueue.Count <= 0)
            {
                return;
            }

            if (!_isDirty)
            {
                return;
            }

            _isDirty = false;
            UpdateText();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateText()
        {
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

            _isDirty = true;
        }
    }
}