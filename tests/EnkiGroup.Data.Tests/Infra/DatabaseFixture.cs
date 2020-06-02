using AutoMapper;
using AutoMapper.Configuration;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EnkiGroup.Data.Tests.Infra
{
    public sealed class DatabaseFixture
    {
        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=EnkiGroupRecadosTest;Trusted_Connection=True;";
        public EnkiGroupContext Context { get; }
        public IMapper Mapper { get; }

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<EnkiGroupContext>();
            options.UseSqlServer(ConnectionString);
            Context = new EnkiGroupContext(options.Options);
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
            Mapper = GetMapper();
        }

        public static IMapper GetMapper()
        {
            var mce = new MapperConfigurationExpression();
            mce.ConstructServicesUsing(Activator.CreateInstance);

            var profileType = typeof(Profile);
            var profiles = typeof(Repositorio<>).Assembly.ExportedTypes
                .Where(type => !type.IsAbstract && profileType.IsAssignableFrom(type))
                .Select(x => Activator.CreateInstance(x))
                .Cast<Profile>()
                .ToArray();

            mce.AddProfiles(profiles);
            mce.AddExpressionMapping();

            var mc = new MapperConfiguration(mce);
            mc.AssertConfigurationIsValid();

            return new Mapper(mc);
        }
    }
}
