using AntDesign;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.Server.AntdesignTheme.Themes.Antdesign
{
    public partial class AvatarDropdown
    {
        [Inject] IStringLocalizer<AbpUiResource> L { get; set; }
        [Inject] protected IMenuManager MenuManager { get; set; }

        [Parameter] public string Avatar { get; set; }
        [Parameter] public string Name { get; set; }
        [Parameter] public EventCallback<MenuItem> OnItemSelected { get; set; }
        private ApplicationMenu UserMenu { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await IniUserMenu();
        }

        private async Task IniUserMenu()
        {
            UserMenu = await MenuManager.GetAsync(StandardMenus.User);
        }
    }
}