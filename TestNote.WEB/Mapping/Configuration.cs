using AutoMapper;
using System;

namespace TestNote.WEB.Mapping
{
    public class Configuration
    {
        private static Lazy<IConfigurationProvider> _defaultConfiguration =
            new Lazy<IConfigurationProvider>(() =>
                 new MapperConfiguration(config =>
                 {
                     config.AddProfile(new DataProfile());
                     config.AddProfile(new ServiceProfile());
                 })
            );

        public static IConfigurationProvider DefaultConfiguration => _defaultConfiguration.Value;

        public static IMapper CreateDefaultMapper() => new Mapper(DefaultConfiguration);
    }
}
