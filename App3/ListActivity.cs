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
    [Activity(Label = "ListActivity", Theme = "@style/AppTheme")]
    public class ListActivity : AppCompatActivity
    {
        ListView mylist;
        ProgressBar progressBar;
        ListAdapter myadapter;
        List<ListItem> dataList;
        private int DetailViewIntentId = 401;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.list);
            mylist = FindViewById<ListView>(Resource.Id.listView1);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            showData();

            mylist.ItemClick += Mylist_ItemClick;

        }

        void Mylist_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog alert = dialog.Create();
            alert.SetTitle("Delete");
            alert.SetMessage("If you want to Delete press OK");
            alert.SetIcon(Resource.Drawable.Shipping_Alert);
            alert.SetButton("OK", (c, ev) =>
            {
                Database db = new Database();
                var select = dataList[e.Position].Id;
                db.DeleteItem(select);
                RefreshData();
            });
            alert.SetButton2("CANCEL", (c, ev) => {
                RefreshData();
            });
            alert.Show();

        }




        private List<ListItem> GenerateListData()
        {
            List<ListItem> data = new List<ListItem>();

            //ans 6
            //for (int j = 1; j <= 4; j++)
            //{
            //    for (int i = 0; i < 2; i++)
            //    {

            //        ListItem obj = new ListItem();
            //        obj.Id = i;
            //        obj.Title = "Title" + j;
            //        obj.Subtitle = "Address" + j;
            //        obj.Distance = j + " km";
            //        obj.Image = "https://picsum.photos/200/200/?" + j;
            //        data.Add(obj);
            //    }
            //}
            // ans for 8
            for (int i = 1; i <= 30; i++){

                ListItem obj = new ListItem();
                obj.Id = i;

                obj.Title = "Title" + i;
                obj.Subtitle = "Address" + i;
                obj.Date = DateTime.Now.AddDays(i);
                obj.Image = "https://picsum.photos/200/200/?" + i;
                data.Add(obj);
            }

            dataList = data;
            //for (int i = 0; i < 30; i++)
                    //{

                    //    ListItem obj = new ListItem();
                    //    obj.Id = i;
                    //    obj.Title = "Title" + i;
                    //    obj.Subtitle = "Address" + i;
                    //    obj.Distance = i + " km";
                    //    obj.Image = "https://picsum.photos/200/200/?" + i;
                    //    data.Add(obj);
                    //}
                return data;
        }

        private List<ListItem> Getlistdata()
        {
            Database db = new Database();
            List<ListItem> listItems = new List<ListItem>();
            listItems = db.Getallitems();
            return listItems;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {

            MenuInflater.Inflate(Resource.Menu.menu1, menu);
            return base.OnCreateOptionsMenu(menu);
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_insert:
                    //var intent = new Intent(this, typeof(DetailView));
                    //StartActivityForResult(intent, DetailViewIntentId);
                    //return true;
                    //ans for 8
                    var intent = new Intent(this, typeof(Sorting));
                    StartActivityForResult(intent, DetailViewIntentId);
                    return true;

                case Resource.Id.action_refresh:
                    RefreshData();
                    Toast.MakeText(this, "Refresh is clicked", ToastLength.Short).Show();

                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            //if ((requestCode == DetailViewIntentId) && (resultCode == Result.Ok))
            //{
            //    RefreshData();
            //    Toast.MakeText(this, data.GetStringExtra("xyz"), ToastLength.Long).Show();
            //}
            //ans for 8
            if ((requestCode == DetailViewIntentId) && (resultCode == Result.Ok))
            {
                Sortdata(data.GetStringExtra("StartDate"), data.GetStringExtra("EndDate"));
                Toast.MakeText(this, data.GetStringExtra("StartDate"), ToastLength.Long).Show();
            }
        }


        private void showData()
        {

            progressBar.Visibility = ViewStates.Visible;
            dataList = GenerateListData();
            progressBar.Visibility = ViewStates.Gone;
            myadapter = new ListAdapter(this, dataList);
            mylist.Adapter = myadapter;


        }

        private void RefreshData()
        {

            progressBar.Visibility = ViewStates.Visible;
            dataList = Getlistdata();
            progressBar.Visibility = ViewStates.Gone;
            myadapter = new ListAdapter(this, dataList);
            mylist.Adapter = myadapter;
        }

        private void Sortdata(String sdate,String edate){

            progressBar.Visibility = ViewStates.Visible;
            List<ListItem> temp = new List<ListItem>();
            DateTime startdate = Convert.ToDateTime(sdate);
            DateTime enddate = Convert.ToDateTime(edate);
            DateTime RightNow = DateTime.Now;
            temp = (from a in dataList
                    where (a.Date.Date >= startdate.Date) && (a.Date.Date <= enddate.Date)
                    select a).ToList();
            dataList = temp;
            progressBar.Visibility = ViewStates.Gone;
            myadapter = new ListAdapter(this, dataList);
            mylist.Adapter = myadapter;

        }


    }
}