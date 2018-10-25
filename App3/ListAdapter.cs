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
using Bumptech.Glide;

namespace App3
{
    class ListAdapter : BaseAdapter<ListItem>
    {

        private List<ListItem> listData;
        private Activity context;
        public ListAdapter(Activity listActivity, List<ListItem> listData) : base()
        {
            this.context = listActivity;
            this.listData = listData;
        }

        

        public override ListItem this[int position]
        {
            get
            {

                return listData[position];

            }

        }
        public override int Count
        {

            get
            {

                return listData.Count;
            }

        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (convertView == null)
            {

                view = context.LayoutInflater.Inflate(Resource.Layout.list_item, null, false);

            }
            ListItem item = this[position];
            view.FindViewById<TextView>(Resource.Id.textView3).Text = item.Title;
            view.FindViewById<TextView>(Resource.Id.textView2).Text = item.Subtitle;
            view.FindViewById<TextView>(Resource.Id.textView1).Text = item.Date.ToString();

            ImageView myimage = view.FindViewById<ImageView>(Resource.Id.imageView1);
            Glide.With(context).Load(item.Image).Into(myimage);

            return view;

            //var view = convertView;
            //if (convertView == null)
            //{

            //    view = context.LayoutInflater.Inflate(Resource.Layout.list_item, null, false);

            //}
            //ListItem item = this[position];
            //view.FindViewById<TextView>(Resource.Id.textView3).Text = item.Title;
            //view.FindViewById<TextView>(Resource.Id.textView2).Text = item.Subtitle;
            //view.FindViewById<TextView>(Resource.Id.textView1).Text = item.Distance.ToString();

            //ImageView myimage = view.FindViewById<ImageView>(Resource.Id.imageView1);
            //Glide.With(context).Load(item.Image).Into(myimage);

            //return view;

            // ans-2 a.

            //View view = convertView; // re-use an existing view, if one is available
            //if (view == null) // otherwise create a new one
            //    view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            //view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = this[position].Title;
            //return view;

            // ans-2 b.

            //View view = convertView; // re-use an existing view, if one is available
            //if (view == null) // otherwise create a new one
            //    view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItemChecked, null);
            //view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = this[position].Title;
            //return view;
        }


    }
}