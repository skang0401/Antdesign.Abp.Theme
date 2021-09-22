using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Components.Server.AntdesignTheme.Bundling
{
    public class BlazorAntdesignThemeScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/_framework/blazor.server.js");
            context.Files.AddIfNotContains("/_content/AntDesign/js/ant-design-blazor.js");
            context.Files.AddIfNotContains("/_content/AntDesign.Charts/ant-design-charts-blazor.js");
            context.Files.AddIfNotContains("/_content/AntDesign.Charts/g2plot.js");
        }
    }
}