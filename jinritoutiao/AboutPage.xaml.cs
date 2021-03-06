﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍
using jinritoutiao.Common;
using jinritoutiao.Core;

namespace jinritoutiao
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        private readonly NavigationHelper _navigationHelper;
        public AboutPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this._navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += navigationHelper_LoadState;
            InitStatusBar();
        }
        private void InitStatusBar()
        {
            StatusBar statusBar = StatusBar.GetForCurrentView();
            statusBar.HideAsync();
        }
        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this._navigationHelper.OnNavigatedTo(e);

            if (SettingsHelper.GetYejianState())
            {
                ChangeToYejian();
            }
            else
            {
                ChangeToBaitian();
            }

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Launcher.LaunchUriAsync(new Uri("http://wincn.net", UriKind.Absolute));
        }

      

        private async void RateButton_OnClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }

        private void FeedbackButton_OnClick(object sender, RoutedEventArgs e)
        {
            var uriMailTo = "mailto:gefangshuai@163.com?subject=关于《今日头条》的反馈";
            Launcher.LaunchUriAsync(new Uri(uriMailTo, UriKind.Absolute));
        }
        private void ChangeToYejian()
        {
            YejianGrid.Opacity = 0.75;

        }

        private void ChangeToBaitian()
        {
            YejianGrid.Opacity = 0;
        }
        private void DonateButton_OnClick(object sender, RoutedEventArgs e)
        {
            Launcher.LaunchUriAsync(new Uri("http://wincn.net/donate/", UriKind.Absolute));
        }
    }
}
