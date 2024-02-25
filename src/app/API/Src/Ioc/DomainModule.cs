using Autofac;
using Domain.Src;
using Objects.Src.Dto;

namespace API.Src.Ioc
{
    internal class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Repository<WordDto>>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
