
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

namespace App3
{
    [Activity(Label = "Sorting")]
    public class Sorting : AppCompatActivity
    {
        DatePicker startdate, enddate;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.sorting);
            Android.Support.V7.App.ActionBar actionBar = this.SupportActionBar;
            actionBar.SetHomeButtonEnabled(true);
            actionBar.SetDisplayHomeAsUpEnabled(true);

            // Create your application here

            startdate = FindViewById<DatePicker>(Resource.Id.datePicker1);
            enddate = FindViewById<DatePicker>(Resource.Id.datePicker2);

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:

                    var intent = new Intent();
                    intent.PutExtra("StartDate",startdate.DateTime.ToString());
                    intent.PutExtra("EndDate", enddate.DateTime.ToString());
                    SetResult(Result.Ok, intent);
                    Finish();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}
