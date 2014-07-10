﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
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
using jinritoutiao.Core.Model;

namespace jinritoutiao
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class FavoritePage : Page
    {
        public ObservableCollection<ReceiveData> ReceiveDatas { get; set; }
        private readonly NavigationHelper _navigationHelper;
        public FavoritePage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this._navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += navigationHelper_LoadState;
            InitStatusBar();

            

        }

        private async void InitConfig()
        {
            List<ReceiveData> receiveDatas = await LocalFileHelper.Read<List<ReceiveData>>(ToutiaoHelper.FILE_NAME);
            if (receiveDatas != null && receiveDatas.Count > 0)
                App.FavoriteDatas = receiveDatas;
            FavoriteListView.ItemsSource = App.FavoriteDatas;
        }

        private void InitStatusBar()
        {
            StatusBar statusBar = StatusBar.GetForCurrentView();
            statusBar.HideAsync();
        }
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
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            InitConfig();
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            if (Frame.CanGoBack)
                Frame.GoBack();
           
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this._navigationHelper.OnNavigatedFrom(e);
        }

        private void FavoritePage_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void FavoriteListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = FavoriteListView.SelectedItem as ReceiveData;
            if (item == null)
            {
                return;
            }
            Frame.Navigate(typeof(ItemBrowsePage), item);
        }


        private void Border_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            
            PopupMenu menu = new PopupMenu();
            menu.Commands.Add(new UICommand("删除", command => Debug.WriteLine(FavoriteListView.SelectedIndex)));
            menu.ShowForSelectionAsync(GetElementRect((FrameworkElement)sender));
        }

        private static Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Point point = buttonTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }
       
    }
}