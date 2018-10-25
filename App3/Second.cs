using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App3
{
    [Activity(Label = "Second")]
    public class Second : Activity
    {

        TextView username;
        TextView password;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.second);
            username = FindViewById<TextView>(Resource.Id.userView);
            password = FindViewById<TextView>(Resource.Id.passView);

            username.Text = Intent.GetStringExtra("username");
            password.Text = Intent.GetStringExtra("password");


            // Create your application here
        }
    }
}