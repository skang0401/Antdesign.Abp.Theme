using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Components.Server.AntdesignTheme.Bundling
{
    public class BlazorAntdesignThemeStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/_content/AntDesign/css/ant-design-blazor.css");
            context.Files.AddIfNotContains("/_content/AntDesign.ProLayout/css/ant-design-pro-layout-blazor.css");
        }
    }
}