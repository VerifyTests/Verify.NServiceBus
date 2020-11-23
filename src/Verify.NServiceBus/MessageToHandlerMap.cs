using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NServiceBus;

namespace VerifyTests.NServiceBus
{
    public class MessageToHandlerMap
    {
        internal HashSet<Type> Messages = new();
        internal HashSet<Type> HandledMessages = new();

        public void AddMessagesFromAssembly(Assembly assembly)
        {
            Guard.AgainstNull(assembly, nameof(assembly));
            foreach (var type in assembly.GetTypes().Where(x => x.IsMessage()))
            {
                AddMessage(type);
            }
        }

        public void AddMessagesFromAssembly<T>()
        {
            AddMessagesFromAssembly(typeof(T).Assembly);
        }

        public void AddMessage(Type type)
        {
            Guard.AgainstNull(type, nameof(type));
            Messages.Add(type);
        }

        public void AddMessage<T>()
        {
            Messages.Add(typeof(T));
        }

        public void AddHandler(Type handlerType)
        {
            Guard.AgainstNull(handlerType, nameof(handlerType));

            foreach (var interfaceType in handlerType
                .GetInterfaces()
                .Where(x =>
                {
                    if (!x.IsGenericType)
                    {
                        return false;
                    }

                    var typeDefinition = x.GetGenericTypeDefinition();
                    return typeDefinition == typeof(IHandleMessages<>);
                }))
            {
                HandledMessages.Add(interfaceType.GenericTypeArguments.Single());
            }
        }

        public void AddHandler<T>()
        {
            AddHandler(typeof(T));
        }

        public void AddHandlersFromAssembly(Assembly assembly)
        {
            Guard.AgainstNull(assembly, nameof(assembly));
            foreach (var type in assembly.GetTypes().Where(x=>x.IsHandler()))
            {
                AddHandler(type);
            }
        }

        public void AddHandlersFromAssembly<T>()
        {
            AddHandlersFromAssembly(typeof(T).Assembly);
        }
    }
}