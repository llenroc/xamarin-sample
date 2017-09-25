﻿
using System;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Children
{
    public partial class ChildrenPage : MvxContentPage<ChildrenViewModel>
    {
        public ChildrenPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
