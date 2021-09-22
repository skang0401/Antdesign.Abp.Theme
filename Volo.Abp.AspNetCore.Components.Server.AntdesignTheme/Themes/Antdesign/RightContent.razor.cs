using AntDesign;
using AntDesign.ProLayout;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Localization;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Components.Server.AntdesignTheme.Themes.Antdesign
{
    public partial class RightContent
    {
        //[Inject] IOptions<AntDesignSettings> SettingState { get; set; }
        [Inject] public ILogger<RightContent> Logger { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] ICurrentUser CurrentUser { get; set; }
        //[Inject] protected IProjectService ProjectService { get; set; }
        [Inject] protected MessageService MessageService { get; set; }
        [Inject] ILanguageProvider LanguageProvider { get; set; }
        [Inject] IStringLocalizer<AbpUiResource> L { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] IAbpRequestLocalizationOptionsProvider RequestLocalizationOptionsProvider { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }


        private NoticeIconData[] _notifications = { };
        private NoticeIconData[] _messages = { };
        private NoticeIconData[] _events = { };
        private int _count = 0;
        private string UserAvatar { get; set; }

        private bool IsMultiLanguage { get { return Locales != null && Locales.Length > 1; } }
        private string[] Locales { get; set; }
        private IDictionary<string, string> LanguageLabels { get; set; }
        private IDictionary<string, string> LanguageIcons { get; set; }
        private IReadOnlyList<LanguageInfo> _otherLanguages;
        private LanguageInfo _currentLanguage;

        [Parameter] public EventCallback<MenuItem> OnUserItemSelected { get; set; }
        private List<AutoCompleteDataItem<string>> DefaultOptions { get; set; } = new List<AutoCompleteDataItem<string>>
        {
            new AutoCompleteDataItem<string>
            {
                Label = "umi ui",
                Value = "umi ui"
            },
            new AutoCompleteDataItem<string>
            {
                Label = "Pro Table",
                Value = "Pro Table"
            },
            new AutoCompleteDataItem<string>
            {
                Label = "Pro Layout",
                Value = "Pro Layout"
            }
        };


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await IniLanguages();
            IniUserInformation();
            SetClassMap();
            //_currentUser = await UserService.GetCurrentUserAsync();
            //var notices = await ProjectService.GetNoticesAsync();
            //_notifications = notices.Where(x => x.Type == "notification").Cast<NoticeIconData>().ToArray();
            //_messages = notices.Where(x => x.Type == "message").Cast<NoticeIconData>().ToArray();
            //_events = notices.Where(x => x.Type == "event").Cast<NoticeIconData>().ToArray();
            //_count = notices.Length;
        }

        protected void SetClassMap()
        {
            ClassMapper
                .Clear()
                .Add("right");
        }
        private void BeginSignOut()
        {
            Navigation.NavigateTo("account/logout", true);
        }


        public void HandleSelectUser(MenuItem item)
        {
            switch (item.Key)
            {
                case "center":
                    //NavigationManager.NavigateTo("/account/center");
                    break;
                case "setting":
                    //NavigationManager.NavigateTo("/account/settings");
                    break;
                case "logout":
                    BeginSignOut();
                    break;
            }
        }
        private void IniUserInformation()
        {
            UserAvatar = string.Empty;
            if (CurrentUser.IsAuthenticated)
            {
                string avatar = CurrentUser.FindClaimValue("Avatar");
                Logger.LogDebug("avatar =>{0}", avatar);
                if (avatar != null)
                {
                    UserAvatar = avatar;
                }
                else
                {
                    UserAvatar = "/_content/Volo.Abp.AspNetCore.Components.Server.AntdesignTheme/libs/antdesign/assets/avatar.png";
                }
            }
        }
        private async Task IniLanguages()
        {
            _otherLanguages = await LanguageProvider.GetLanguagesAsync();

            if (!_otherLanguages.Any())
            {
                return;
            }

            _currentLanguage = _otherLanguages.FirstOrDefault(l => l.UiCultureName == CultureInfo.CurrentUICulture.Name);

            if (_currentLanguage == null)
            {
                var localizationOptions = await RequestLocalizationOptionsProvider.GetLocalizationOptionsAsync();
                if (localizationOptions.DefaultRequestCulture != null)
                {
                    _currentLanguage = new LanguageInfo(
                    localizationOptions.DefaultRequestCulture.Culture.Name,
                    localizationOptions.DefaultRequestCulture.UICulture.Name,
                    localizationOptions.DefaultRequestCulture.UICulture.DisplayName);
                }
                else
                {
                    _currentLanguage = new LanguageInfo(
                   CultureInfo.CurrentCulture.Name,
                   CultureInfo.CurrentUICulture.Name,
                   CultureInfo.CurrentUICulture.DisplayName);
                }
            }

            //language menu
            List<string> _locales = new List<string>();
            LanguageLabels = new Dictionary<string, string>();
            LanguageIcons = new Dictionary<string, string>();
            foreach (LanguageInfo language in _otherLanguages)
            {
                Logger.LogDebug("language:{0}", language.CultureName);
                _locales.Add(language.CultureName);
                LanguageLabels.Add(language.CultureName, language.FlagIcon);
                LanguageIcons.Add(language.CultureName, language.DisplayName);
            }
            this.Locales = _locales.ToArray();
            _otherLanguages = _otherLanguages.Where(l => l != _currentLanguage).ToImmutableList();
        }


        private void HandleSelectLang(MenuItem item)
        {
            LanguageInfo language = null;
            if (_currentLanguage.CultureName != item.Key)
            {
                language = _otherLanguages.FirstOrDefault(i => i.CultureName == item.Key);

                var relativeUrl = NavigationManager.Uri.RemovePreFix(NavigationManager.BaseUri).EnsureStartsWith('/');

                NavigationManager.NavigateTo(
                    $"/Abp/Languages/Switch?culture={language.CultureName}&uiCulture={language.UiCultureName}&returnUrl={relativeUrl}",
                    forceLoad: true
                );
            }
        }

        public async Task HandleClear(string key)
        {
            switch (key)
            {
                case "notification":
                    _notifications = new NoticeIconData[] { };
                    break;
                case "message":
                    _messages = new NoticeIconData[] { };
                    break;
                case "event":
                    _events = new NoticeIconData[] { };
                    break;
            }
            await MessageService.Success($"清空了{key}");
        }

        public async Task HandleViewMore(string key)
        {
            await MessageService.Info("Click on view more");
        }

        private void OnClickLogin()
        {
            bool isWebAssembly = JSRuntime is IJSInProcessRuntime;
            if (isWebAssembly)
            {
                Navigation.NavigateTo($"authentication/login?returnUrl={Uri.EscapeDataString(Navigation.Uri)}");
            }
            else
            {
                Navigation.NavigateTo($"account/login?returnUrl={Uri.EscapeDataString(Navigation.Uri)}", true);
            }
        }

    }
}