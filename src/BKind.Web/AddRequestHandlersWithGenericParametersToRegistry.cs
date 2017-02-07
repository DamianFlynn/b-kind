﻿using System;
using System.Reflection;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Infrastructure.Persistance.StandardHandlers;
using BKind.Web.Model;
using MediatR;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;

namespace BKind.Web
{
    public class AddRequestHandlersWithGenericParametersToRegistry : IRegistrationConvention
    {
        public void ScanTypes(TypeSet types, Registry registry)
        {
            foreach (var concreteClass in types.FindTypes(TypeClassification.Concretes))
            {
                if (typeof(Entity).IsAssignableFrom(concreteClass))
                {
                    Register(registry, typeof(GetOneQuery<>), typeof(GetOneHandler<>), concreteClass, concreteClass);
                    Register(registry, typeof(GetAllQuery<>), typeof(GetAllHandler<>), concreteClass, typeof(PagedList<>).MakeGenericType(concreteClass));
                }
            }
        }

        private void Register(Registry registry, Type genericRequest, Type genericHandler, Type concreteClass, Type handlerOutputType)
        {
            var genericCommand = genericRequest.MakeGenericType(concreteClass);
            var interfaceHandlerType = typeof(IAsyncRequestHandler<,>).MakeGenericType(genericCommand, handlerOutputType);
            var concreteHandlerType = genericHandler.MakeGenericType(concreteClass);
            registry.For(interfaceHandlerType).Use(concreteHandlerType);
        }
    }
}