using System;
using System.Threading.Tasks;
using AntDesign.ProLayout;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.Server.AntdesignTheme.Themes.Antdesign
{
    public partial class MainLayout : IDisposable
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IMenuManager MenuManager { get; set; }

        private MenuDataItem[] _menuData = { };
        private string _logoUri;
        private bool IsCollapseShown { get; set; }

        protected override void OnInitialized()
        {
            NavigationManager.LocationChanged += OnLocationChanged;
            _logoUri = "/_content/Volo.Abp.AspNetCore.Components.Server.AntdesignTheme/libs/antdesign/assets/logo.svg";
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await IniMainMenu();
        }

        private void ToggleCollapse()
        {
            IsCollapseShown = !IsCollapseShown;
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            IsCollapseShown = false;
            InvokeAsync(StateHasChanged);
        }

        private async Task IniMainMenu()
        {
            ApplicationMenu menu = await MenuManager.GetAsync(StandardMenus.Main);

            if (menu != null)
            {
                _menuData = GetMenuDataItems(menu.Items);
            }
        }
        private MenuDataItem[] GetMenuDataItems(ApplicationMenuItemList menuItems)
        {
            if (menuItems != null && menuItems.Count > 0)
            {
                MenuDataItem[] antMenuItems;
                antMenuItems = new MenuDataItem[menuItems.Count];
                for (int i = 0; i < antMenuItems.Length; i++)
                {
                    MenuDataItem item = new MenuDataItem();
                    item.Path = menuItems[i].Url == null ? "#" : menuItems[i].Url.TrimStart('/', '~');
                    item.Name = menuItems[i].DisplayName;
                    item.Key = menuItems[i].Name;
                    item.Icon = menuItems[i].Icon;
                    antMenuItems[i] = item;

                    if (menuItems[i].Items != null && menuItems[i].Items.Count > 0)
                    {
                        antMenuItems[i].Children = GetMenuDataItems(menuItems[i].Items);
                    }
                }
                return antMenuItems;
            }
            return null;
        }
    }
}
