using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Server.AntdesignTheme.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.Server.AntdesignTheme
{
    [DependsOn(
        typeof(AbpUiNavigationModule),
        typeof(AbpAspNetCoreComponentsServerModule),
        typeof(AbpAspNetCoreMvcUiPackagesModule),
        typeof(AbpAspNetCoreMvcUiBundlingModule)
    )]
    public class AbpAspNetCoreComponentsServerAntdesignThemeModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBundlingOptions>(options =>
            {
                options
                    .StyleBundles
                    .Add(BlazorAntdesignThemeBundles.Styles.Global, bundle =>
                    {
                         bundle.AddContributors(typeof(BlazorAntdesignThemeStyleContributor));
                    });

                options
                    .ScriptBundles
                    .Add(BlazorAntdesignThemeBundles.Scripts.Global, bundle =>
                    {
                        bundle.AddContributors(typeof(BlazorAntdesignThemeScriptContributor));
                    });
            });

            context.Services.AddAntDesign();
        }
    }
}
