using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace App3
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        String username = "lambton";
        String password = "123456";
        TextView topText;
        EditText nameText;
        EditText passText;
        Button submit;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            topText= FindViewById<TextView>(Resource.Id.topText);
            nameText = FindViewById<EditText>(Resource.Id.EditText1);
            nameText.TextChanged += changeTopText;
            passText = FindViewById<EditText>(Resource.Id.EditText2);
            submit = FindViewById<Button>(Resource.Id.button1);
            submit.Click += validateUser;
        }

        private void changeTopText(object sender, TextChangedEventArgs e)
        {
            if (nameText.Text.Length < 10) {

                topText.Text = "Welcome: "+ nameText.Text;
            }
        }

        private void validateUser(object sender, EventArgs e)
        {
            if (nameText.Text.Equals(username) && passText.Text.Equals(password))
            {

                var intent = new Intent(this, typeof(ListActivity));

                intent.PutExtra("username", nameText.Text);
                intent.PutExtra("password", passText.Text);
                StartActivity(intent);
            }
        }
    }
}

