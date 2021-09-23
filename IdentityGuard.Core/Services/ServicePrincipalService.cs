using IdentityGuard.Core.Factories;
using IdentityGuard.Core.Mappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityGuard.Core.Services
{
    public class ServicePrincipalService
    {
        private readonly IGraphClientFactory _graphClientFactory;
        private readonly ServicePrincipalMapper _servicePrincipalMapper;

        public ServicePrincipalService(IGraphClientFactory graphClientFactory,
            ServicePrincipalMapper servicePrincipalMapper)
        {
            _graphClientFactory = graphClientFactory;
            _servicePrincipalMapper = servicePrincipalMapper;
        }

        public async Task<Shared.Models.ServicePrincipal> Get(Shared.Models.Directory directory, string id, bool includeOwners = false)
        {
            var client = await _graphClientFactory.CreateAsync(directory);

            var data = await client
                .ServicePrincipals[id]
                .Request()
                .GetAsync();

            var result = _servicePrincipalMapper.Map(directory, data);

            return result;
        }
    }
}
