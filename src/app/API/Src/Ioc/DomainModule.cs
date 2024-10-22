using Autofac;
using Domain;
using Domain.Src;
using Objects.Dto;
using Objects.Src.Dto;
using Objects.Src.Models;

namespace API.Src.Ioc
{
    internal class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Repository<WordDto>>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<Repository<AttemptHistoryDto>>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<Repository<AttemptDto>>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
