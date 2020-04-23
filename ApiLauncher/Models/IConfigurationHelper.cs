using System.Threading.Tasks;

namespace ApiLauncher.Models
{
    public interface IConfigurationHelper
    {
        Task<MicroservicesConfiguration> GetMicroservicesConfiguration();

        Task WriteOcelotConfigurationFile(string fileName, OcelotConfiguration ocelot);

        Task UpdateOcelotConfiguration(OcelotConfiguration ocelot, Microservice microservice);

        Task UpdateOcelotConfiguration(Microservice microservice);

        void ValidateConfigurationFiles();

        Task<OcelotConfiguration> ReadOcelotConfigurationFile(string microserviceGateway);
    }
}