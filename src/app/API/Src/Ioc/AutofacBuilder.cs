﻿using Autofac;

namespace API.Src.Ioc
{
    internal class AutofacBuilder
    {
        public static ContainerBuilder Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<DomainModule>();

            return builder;
        }
    }
}
