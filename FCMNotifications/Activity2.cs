using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace FCMNotifications
{
    [Activity(Label = "Activity2")]
    public class Activity2 : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            //base.OnCreate(savedInstanceState);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.layout_activity2);
            // Create your application here
        }
    }
}