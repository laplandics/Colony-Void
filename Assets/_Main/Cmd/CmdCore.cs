using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cmd
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    { public bool Handle(TCommand command); }
    public interface ICommand { }

    public class CommandProcessor
    {
        private readonly Dictionary<Type, object> _handlersMap = new();
        
        public void Register<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand
        {
            if (_handlersMap.ContainsKey(typeof(TCommand)))
            {Debug.LogWarning($"CommandHandler of command {typeof(TCommand).Name} was already registered");}
            _handlersMap[typeof(TCommand)] = handler;
        }

        public bool Process<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (!_handlersMap.TryGetValue(typeof(TCommand), out var handlerObject))
            { Debug.LogError($"CommandHandler of command {typeof(TCommand).Name} was not registered"); return false; }
            
            var handler = (ICommandHandler<TCommand>)handlerObject;
            return handler.Handle(command);
        }
    }
}