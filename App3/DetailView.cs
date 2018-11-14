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
    [Activity(Label = "DetailView")]
    public class DetailView : AppCompatActivity
    {
        EditText title, subtitle, distance;
        ImageButton Button;
        Android.Net.Uri uri;

        ListItem item1;
        private int PickImageId = 205;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.detailview);
            Android.Support.V7.App.ActionBar actionBar = this.SupportActionBar;
            actionBar.SetHomeButtonEnabled(true);
            actionBar.SetDisplayHomeAsUpEnabled(true);


            title = FindViewById<EditText>(Resource.Id.title);
            subtitle = FindViewById<EditText>(Resource.Id.subtitle);
            distance = FindViewById<EditText>(Resource.Id.distance);

            Button = FindViewById<ImageButton>(Resource.Id.imageButton1);

            Button.Click += delegate
            {


                Intent = new Intent();
                Intent.SetType("image/*");
                Intent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
            };


        }



        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && data != null)
            {
                uri = data.Data;
                Button.SetImageURI(uri);

            }
        }






        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Database db = new Database();
                    if(String.IsNullOrEmpty(title.Text)){
                        title.Error = "Title should not be Empty";
                    }
                    if (String.IsNullOrEmpty(subtitle.Text))
                    {
                        subtitle.Error = "SubTitle should not be Empty";
                    }
                    if (String.IsNullOrEmpty(distance.Text))
                    {
                        distance.Error = "Distance should not be Empty";
                    }

                    item1 = new ListItem();
                    item1.Title = title.Text;
                    item1.Subtitle = subtitle.Text;
                    item1.Distance = distance.Text;
                    item1.Image = uri.ToString();
                    db.CreateDatabase();
                    db.InsertIntoTable(item1);
                    var intent = new Intent();
                    intent.PutExtra("xyz", "Title:" + title.Text + "\n Subtitle" + subtitle.Text + "\n Distance " + distance.Text);
                    SetResult(Result.Ok, intent);
                    Finish();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

       


    }
}