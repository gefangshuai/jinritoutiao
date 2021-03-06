﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using jinritoutiao.Core.Model;

namespace jinritoutiao.Core
{
    public class HtmlParseHelper : HtmlHelper
    {
        private CoreDispatcher _simpleDispatcher;
        public ObservableCollection<ReceiveData> ReceiveDatas { get; set; }
        public Next Next { get; set; }
        public Grid FooterGrid { get; set; }
        public ProgressBar ProgressBar { get; set; }
        public HtmlParseHelper(ObservableCollection<ReceiveData> receiveDatas, Next next, Grid grid, ProgressBar progressBar)
        {
            _simpleDispatcher = Window.Current.Dispatcher;
            ReceiveDatas = receiveDatas;
            Next = next;
            FooterGrid = grid;
            ProgressBar = progressBar;
        }

        protected override void HtmlHandler(string html)
        {
            try
            {
                Debug.WriteLine(html);
                var jsonObject = JsonObject.Parse(html);

                if (html.Contains("message"))
                {
                    var message = jsonObject["message"].GetString();

                    if (message.Equals("success"))  // 返回数据成功！
                    {
                        var dataArray = jsonObject["data"].GetArray();

                        if (html.Contains("next"))
                        {
                            var nextObj = jsonObject["next"].GetObject();

                            Next.MaxBehotTime = ((int)nextObj["max_behot_time"].GetNumber()).ToString();
                            Next.MinBehotTime = ((int)nextObj["min_behot_time"].GetNumber()).ToString();

                        }
                        _simpleDispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                        {
                            try
                            {
                                foreach (var itemValue in dataArray)
                                {
                                    var itemJson = itemValue.Stringify();
                                    JsonObject itemObject = itemValue.GetObject();
                                    // 存在Image_list，可显示多个图片
                                    string[] imageArray = new string[3];
                                    int imageCount = 0;


                                    if (itemJson.Contains("large_image_url"))
                                    {
                                        imageArray[0] = itemObject["large_image_url"].GetString();
                                        imageCount = 1;
                                    }
                                    else if (itemJson.Contains("image_url"))
                                    {
                                        imageArray[0] = itemObject["image_url"].GetString();
                                        imageCount = 1;
                                    }
                                    else if (itemJson.Contains("image_list"))
                                    {
                                        var images = itemObject["image_list"].GetArray();
                                        if (itemObject["image_list"].Stringify().Contains("url"))
                                        {
                                            if (images.Count == 1)  //一张图片
                                            {
                                                imageArray[0] = images[0].GetObject()["url"].GetString();
                                                imageCount = 1;
                                            }
                                            else if (images.Count == 2)
                                            {
                                                imageArray[0] = images[0].GetObject()["url"].GetString();
                                                imageArray[1] = images[1].GetObject()["url"].GetString();
                                                imageCount = 2;
                                            }
                                            else if (images.Count == 3)
                                            {
                                                imageArray[0] = images[0].GetObject()["url"].GetString();
                                                imageArray[1] = images[1].GetObject()["url"].GetString();
                                                imageArray[2] = images[2].GetObject()["url"].GetString();
                                                imageCount = 3;
                                            }
                                        }
                                    }

                                    long id = (long)itemObject["id"].GetNumber();
                                    string title = "";
                                    if (itemJson.Contains("title"))
                                        title = itemObject["title"].GetString();
                                    string _abstract = "";
                                    if(itemJson.Contains("abstract"))
                                        _abstract = itemObject["abstract"].GetString();

                                    string sourceUrl = "";
                                    if (itemJson.Contains("source_url")) 
                                    { 
                                        sourceUrl = itemObject["source_url"].GetString();
                                        if (!sourceUrl.Contains("http://"))
                                            sourceUrl = string.Format("http://toutiao.com{0}", sourceUrl);
                                    }
                                    string source = "";
                                    if (itemJson.Contains("source"))
                                        source = itemObject["source"].GetString();
                                    int commentsCount = 0;
                                    if (itemJson.Contains("comments_count"))
                                        commentsCount = (int)itemObject["comments_count"].GetNumber();
                                    string datetime = "";
                                    if (itemJson.Contains("datetime"))
                                        datetime = itemObject["datetime"].GetString();
                                    string createTime = null;
                                    if (itemJson.Contains("create_time"))
                                        createTime = ((int)itemObject["create_time"].GetNumber()).ToString();
                                    ReceiveDatas.Add(new ReceiveData()
                                    {
                                        Id = id,
                                        Title = title,
                                        Abstract = _abstract,
                                        SourceUrl = sourceUrl,
                                        ImageList = imageArray,
                                        ImageCount = imageCount,
                                        Source = source,
                                        CommentsCount = commentsCount,
                                        Datetime = datetime,
                                        CreateTime = createTime
                                    });
                                    FooterGrid.Visibility = Visibility.Visible;
                                    ProgressBar.Visibility = Visibility.Collapsed;
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(e);
                            }
                        });
                    }
                }
            }
            catch (Exception e)
            {
                _simpleDispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                {
                    MessageDialog dialog = new MessageDialog("服务器错误,请稍后重试！", "错误");
                    dialog.ShowAsync();
                });
            }
        }
    }
}
