using FreeDOW.API.Core.Abstract;
using FreeDOW.API.Core.Entities;

namespace FreeDOW.API.WebHost.Helpers
{
    public static class OrganizationLists
    {
        
        /// <summary>
        /// return list of organiztions Id for the user - user organization and all childs organizations
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Guid>> GetOrgIds(Guid orgId, IOrgRepository<Organization> repoOrg)
        {
            async Task<List<Guid>> GetChildOrg(Guid root)
            {
                List<Guid> rv = new List<Guid>();
                var orgs = await repoOrg.GetByConditionAsync(rec => rec.ParentId == root);
                if (!orgs.Any()) return rv;
                foreach (var org in orgs)
                {
                    var ch = await GetChildOrg(org.Id);
                    if (ch.Any()) rv.AddRange(ch);
                }
                return rv;
            }

            

            List<Guid> orgIds = new List<Guid>();
            orgIds.Add(orgId);
            var ch = await GetChildOrg(orgId);
            orgIds.AddRange(ch);
            return orgIds;
        }

        /// <summary>
        /// Get List of all orgstructs Id for the organization
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public static async Task<List<Guid>> GetOrgStructIds(Guid orgId, IOrgRepository<OrgStruct> repoOrgStr)
        {
            async Task<List<Guid>> GetChildOrgStruct(Guid root)
            {
                List<Guid> rv = new List<Guid>();
                var orgStrs = await repoOrgStr.GetByConditionAsync(rec => rec.ParentId == root);
                if (!orgStrs.Any()) return rv;
                foreach (var os in orgStrs)
                {
                    var ch = await GetChildOrgStruct(os.Id);
                    if (ch.Any()) rv.AddRange(ch);
                }
                return rv;
            }
            List<Guid> osIds = new List<Guid>();
            var rootOS = await repoOrgStr.GetByConditionAsync(rec => rec.OrganizationId == orgId);
            foreach (var os in rootOS)
            {
                osIds.Add(os.Id);
                var ch = await GetChildOrgStruct(os.Id);
                if (ch.Any()) osIds.AddRange(ch);
            }
            return osIds;
        }

        /// <summary>
        /// Get List of all orgstructs Id for the organization
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public static async Task<List<Guid>> GetChildOrgStructIds(Guid root, IOrgRepository<OrgStruct> repoOrgStr)
        {
            async Task<List<Guid>> GetChildOrgStruct(Guid root)
            {
                List<Guid> rv = new List<Guid>();
                var orgStrs = await repoOrgStr.GetByConditionAsync(rec => rec.ParentId == root);
                if (!orgStrs.Any()) return rv;
                foreach (var os in orgStrs)
                {
                    var ch = await GetChildOrgStruct(os.Id);
                    if (ch.Any()) rv.AddRange(ch);
                }
                return rv;
            }
            List<Guid> osIds = new List<Guid>();
            osIds.Add(root);
            var ch = await GetChildOrgStruct(root);
            if (ch.Any()) osIds.AddRange(ch);

            return osIds;
        }
    }
}
