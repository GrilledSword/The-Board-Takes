using System;
using System.Collections.Generic;

namespace BoardTakes.Core
{
    /// <summary>
    /// Main-thread in-process bus. No third-party, no reflection.
    /// Subscribe from OnEnable, unsubscribe from OnDisable. Always.
    /// </summary>
    public sealed class GameEventBus
    {
        readonly Dictionary<Type, Delegate> _map = new();

        public void Publish<T>(T evt)
        {
            if (evt == null) throw new ArgumentNullException(nameof(evt));
            if (_map.TryGetValue(typeof(T), out var d) && d is Action<T> typed)
                typed.Invoke(evt);
        }

        public void Subscribe<T>(Action<T> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            var type = typeof(T);
            if (_map.TryGetValue(type, out var existing))
                _map[type] = Delegate.Combine(existing, handler);
            else
                _map[type] = handler;
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            if (handler == null) return;
            var type = typeof(T);
            if (!_map.TryGetValue(type, out var existing)) return;
            var next = Delegate.Remove(existing, handler);
            if (next == null) _map.Remove(type);
            else _map[type] = next;
        }

        public void Clear() => _map.Clear();
    }

    public readonly struct FlowStateChanged
    {
        public FlowStateChanged(FlowState previous, FlowState current)
        {
            Previous = previous;
            Current = current;
        }

        public FlowState Previous { get; }
        public FlowState Current { get; }
    }

    public readonly struct LocaleChanged
    {
        public LocaleChanged(string code) => Code = code;
        public string Code { get; }
    }

    public readonly struct SettingsApplied
    {
        public SettingsApplied(bool saved) => Saved = saved;
        public bool Saved { get; }
    }
}
