using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdentityGuard.Core.Mappers
{
    public class GroupMapper
    {
        private readonly DirectoryObjectMapper _directoryObjectMapper;
        private readonly ApplicationRoleMapper _applicationRoleMapper;

        public GroupMapper(DirectoryObjectMapper directoryObjectMapper,
            ApplicationRoleMapper applicationRoleMapper)
        {
            _directoryObjectMapper = directoryObjectMapper;
            _applicationRoleMapper = applicationRoleMapper;
        }
        public Shared.Models.Group Map(Shared.Models.Directory directory, 
            Microsoft.Graph.Group toMap,
            IEnumerable<Microsoft.Graph.DirectoryObject> owners,
            IEnumerable<Microsoft.Graph.DirectoryObject> members)
        {
            var ownerData = owners?.Select(o => _directoryObjectMapper.Map(directory, o))?.ToList();
            var memberData = members?.Select(o => _directoryObjectMapper.Map(directory, o))?.ToList();
            
            return new Shared.Models.Group
            {
                Id  = toMap.Id,
                DirectoryId = directory.Id,
                DirectoryName = directory.Domain,
                DisplayName = toMap.DisplayName,
                DynamicMembershipRule = toMap.MembershipRule,
                Types = toMap.GroupTypes?.ToList() ?? new List<string>(),
                Owners = ownerData,
                Members = memberData
                
            };
        }
    }
}
