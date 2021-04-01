using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slipstream.Backend
{
    public static class LifetimeScopeExtensions
    {
        public static IEnumerable<Type> GetImplementingTypes<T>(this ILifetimeScope scope)
        {
            return scope.ComponentRegistry
                .RegistrationsFor(new TypedService(typeof(T)))
                .Select(x => x.Activator)
                .OfType<ReflectionActivator>()
                .Select(x => x.LimitType);
        }
    }
}