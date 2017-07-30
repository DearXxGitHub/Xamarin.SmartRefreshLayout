﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Com.Scwang.Smartrefresh.Layout.Api;
using Com.Scwang.Smartrefresh.Layout.Header;
using static Android.Widget.AdapterView;
using AdapterView = Android.Widget.AdapterView;
using Android.Support.V4.Content;
using Sample_SmartRefreshLayout.Adapters;
using Java.Text;
using Java.Util;
using Com.Scwang.Smartrefresh.Layout.Listener;
using Com.Scwang.Smartrefresh.Layout.Constant;
using Sample_SmartRefreshLayout.Common;
using Sample_SmartRefreshLayout.Utils;

namespace Sample_SmartRefreshLayout.Activities.Style
{
    [Activity(Label = "@string/title_activity_style_classics")]
    public class ClassicsStyleActivity : AppCompatActivity, IOnItemClickListener
    {
        private class Item
        {
            // switch case
            public const string 尺寸拉伸 = "尺寸拉伸";
            public const string 位置平移 = "位置平移";
            public const string 背后固定 = "背后固定";
            public const string 默认主题 = "默认主题";
            public const string 橙色主题 = "橙色主题";
            public const string 红色主题 = "红色主题";
            public const string 绿色主题 = "绿色主题";
            public const string 蓝色主题 = "蓝色主题";
            public const string 加载更多 = "加载更多";
            public static List<Item> List = new List<Item>{
                new Item(尺寸拉伸, "下拉的时候Header的高度跟随变大"),
                new Item(位置平移, "下拉的时候Header的位置向下偏移"),
                new Item(背后固定, "下拉的时候Header固定在背后"),
                new Item(橙色主题, "更改为橙色主题颜色"),
                new Item(红色主题, "更改为红色主题颜色"),
                new Item(绿色主题, "更改为绿色主题颜色"),
                new Item(蓝色主题, "更改为蓝色主题颜色"),
                new Item(加载更多, "上啦加载更多"),
            };
            public Item(string title, string name)
            {
                Title = title;
                Name = name;
            }
            public string Title;
            public string Name;
        }

        private Toolbar mToolbar;
        private IRefreshLayout mRefreshLayout;
        private RecyclerView mRecyclerView;
        private ClassicsHeader mClassicsHeader;
        private static bool isFirstEnter = true;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_style_classics);

            mToolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            mToolbar.NavigationClick += (sender, e) => { Finish(); };

            mRefreshLayout = FindViewById(Resource.Id.refreshLayout) as IRefreshLayout;

            int deta = new System.Random().Next(7 * 24 * 60 * 60 * 1000);
            mClassicsHeader = (ClassicsHeader)mRefreshLayout.RefreshHeader;
            mClassicsHeader.SetLastUpdateTime(new Date(Java.Lang.JavaSystem.CurrentTimeMillis() - deta));
            mClassicsHeader.SetTimeFormat(new SimpleDateFormat("更新于 MM-dd HH:mm", Locale.China));
            mClassicsHeader.SetTimeFormat(new DynamicTimeFormat("更新于 %s"));

            var recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            recyclerView.SetItemAnimator(new DefaultItemAnimator());
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));
            recyclerView.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Vertical));
            recyclerView.SetAdapter(new CustomBaseRecyclerAdapter(Item.List, Android.Resource.Layout.SimpleListItem2, this));

            if (isFirstEnter)
            {
                isFirstEnter = false;
                //触发上啦加载
                mRefreshLayout.AutoLoadmore();
                //通过多功能监听接口实现 在第一次加载完成之后 自动刷新
                mRefreshLayout.SetOnMultiPurposeListener(new CustomSimpleMultiPurposeListener());
            }
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            switch (Item.List[position].Title)
            {
                case Item.蓝色主题:
                    setThemeColor(Resource.Color.colorPrimary, Resource.Color.colorPrimaryDark);
                    break;
                case Item.绿色主题:
                    setThemeColor(Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloGreenDark);
                    break;
                case Item.红色主题:
                    setThemeColor(Android.Resource.Color.HoloRedLight, Android.Resource.Color.HoloRedDark);
                    break;
                case Item.橙色主题:
                    setThemeColor(Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloOrangeDark);
                    break;
            }
            mRefreshLayout.AutoRefresh();
        }

        private void setThemeColor(int colorPrimary, int colorPrimaryDark)
        {
            mToolbar.SetBackgroundResource(colorPrimary);
            mRefreshLayout.SetPrimaryColorsId(colorPrimary, Android.Resource.Color.White);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop) //21
            {
                Window.SetStatusBarColor(new Android.Graphics.Color(ContextCompat.GetColor(this, colorPrimaryDark)));
            }
        }

        private class CustomBaseRecyclerAdapter : BaseRecyclerAdapter<Item>
        {
            public CustomBaseRecyclerAdapter(List<Item> list, int layoutId, IOnItemClickListener listener)
                : base(list, layoutId, listener)
            {

            }

            protected override void onBindViewHolder(SmartViewHolder holder, Item model, int position)
            {
                holder.Text(Android.Resource.Id.Text1, model.Title);
                holder.Text(Android.Resource.Id.Text2, model.Name);
                holder.TextColorId(Android.Resource.Id.Text2, Resource.Color.colorTextAssistant);
            }
        }
    }
}