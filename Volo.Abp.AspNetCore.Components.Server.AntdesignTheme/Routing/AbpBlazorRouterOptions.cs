using System.Reflection;

namespace Volo.Abp.AspNetCore.Components.Server.AntdesignTheme.Routing
{
    public class AbpRouterOptions
    {
        public Assembly AppAssembly { get; set; }

        public RouterAssemblyList AdditionalAssemblies { get; }

        public AbpRouterOptions()
        {
            AdditionalAssemblies = new RouterAssemblyList();
        }
    }
}
