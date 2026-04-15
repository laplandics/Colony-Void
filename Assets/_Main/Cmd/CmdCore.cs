using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cmd
{
    public interface ICommandHandler<in TCommand> where TCommand : Command
    { public bool Handle(TCommand command); }

    public abstract class Command {}
    public abstract class Command<T> : Command
    { public T Result { get; set; } }

    public class CommandProcessor : IDisposable
    {
        private readonly Dictionary<Type, HandlerRegistration> _handlersMap = new();
        
        public HandlerRegistration Register<TCommand>(ICommandHandler<TCommand> handler) where TCommand : Command
        {
            if (_handlersMap.ContainsKey(typeof(TCommand)))
            {Debug.LogWarning($"Handler of command {typeof(TCommand).Name} was already registered");}
            
            var registration = new HandlerRegistration{Handler = handler};
            _handlersMap[typeof(TCommand)] = registration;
            return registration;
        }

        public bool Process<TCommand>(TCommand command) where TCommand : Command
        {
            if (!_handlersMap.TryGetValue(typeof(TCommand), out var registration))
            { Debug.LogError($"Handler of command {typeof(TCommand).Name} was not registered"); return false; }
            
            var handler = (ICommandHandler<TCommand>)registration.Handler;
            return handler.Handle(command);
        }

        public void Dispose()
        {
            var keysToRemove = new List<Type>();
            foreach (var (key, registration) in _handlersMap)
            { if (registration.Disposable) { keysToRemove.Add(key); } }
            foreach (var key in keysToRemove) _handlersMap.Remove(key);
        }
    }

    public class HandlerRegistration
    {
        public object Handler { get; set; }
        public bool Disposable { get; private set; }
        
        public void AsDisposable() => Disposable = true;
    }
}