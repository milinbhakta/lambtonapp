using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;


namespace App3
{
    [Activity(Label = "ListActivity", Theme = "@style/AppTheme")]
    public class ListActivity : AppCompatActivity , IOnMapReadyCallback
    {
        ListView mylist;
        ProgressBar progressBar;
        ListAdapter myadapter;
        List<ListItem> dataList;
        private int DetailViewIntentId = 401;
        String Address;
        private GoogleMap GMap;

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
            Address = dataList[e.Position].Subtitle;
            Transfer.Address = Address;
            var intent = new Intent(this, typeof(Map));
            StartActivity(intent);

            //Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
            //Android.App.AlertDialog alert = dialog.Create();
            //alert.SetTitle("Delete");
            //alert.SetMessage("If you want to Delete press OK");
            //alert.SetIcon(Resource.Drawable.Shipping_Alert);
            //alert.SetButton("OK", (c, ev) =>
            //{
            //    Database db = new Database();
            //    var select = dataList[e.Position].Id;
            //    db.DeleteItem(select);
            //    RefreshData();
            //});
            //alert.SetButton2("CANCEL", (c, ev) =>
            //{
            //    RefreshData();
            //});
            //alert.Show();


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
            //for (int i = 1; i <= 30; i++){

            //    ListItem obj = new ListItem();
            //    obj.Id = i;

            //    obj.Title = "Title" + i;
            //    obj.Subtitle = "Address" + i;
            //    obj.Date = DateTime.Now.AddDays(i);
            //    obj.Image = "https://picsum.photos/200/200/?" + i;
            //    data.Add(obj);
            //}

            //dataList = data;
            for (int i = 0; i < 30; i++)
            {

                ListItem obj = new ListItem();
                obj.Id = i;
                obj.Subtitle = CountryArrays.Names[i];
                obj.Title = CountryArrays.Abbreviations[i];
                obj.Distance = i + " km";
                obj.Image = "https://picsum.photos/200/200/?" + i;
                data.Add(obj);
            }
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
                    var intent = new Intent(this, typeof(DetailView));
                    StartActivityForResult(intent, DetailViewIntentId);
                    return true;
                //ans for 8
                //var intent = new Intent(this, typeof(Sorting));
                //StartActivityForResult(intent, DetailViewIntentId);
                //return true;

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
            if ((requestCode == DetailViewIntentId) && (resultCode == Result.Ok))
            {
                RefreshData();
                Toast.MakeText(this, data.GetStringExtra("xyz"), ToastLength.Long).Show();
            }
            //ans for 8
            //if ((requestCode == DetailViewIntentId) && (resultCode == Result.Ok))
            //{
            //    Sortdata(data.GetStringExtra("StartDate"), data.GetStringExtra("EndDate"));
            //    Toast.MakeText(this, data.GetStringExtra("StartDate"), ToastLength.Long).Show();
            //}
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

        private void Sortdata(String sdate, String edate)
        {

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

        public void OnMapReady(GoogleMap googleMap)
        {
            this.GMap = googleMap;
            //var location = GetLocation(address);

            var geo = new Geocoder(this);
            var addresses = geo.GetFromLocationName(Address, 1);
            GMap.UiSettings.ZoomControlsEnabled = true;
            LatLng latlng = new LatLng(Convert.ToDouble(addresses[0].Latitude), Convert.ToDouble(addresses[0].Longitude));
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 5);
            GMap.MoveCamera(camera);
            MarkerOptions options = new MarkerOptions().SetPosition(latlng).SetTitle(Address);
            GMap.AddMarker(options);
        }
    }

    class CountryArrays
    {
        /// <summary>
        /// Country names
        /// </summary>
        public static string[] Names = new string[]
        {
    "Afghanistan",
    "Albania",
    "Algeria",
    "American Samoa",
    "Andorra",
    "Angola",
    "Anguilla",
    "Antarctica",
    "Antigua and Barbuda",
    "Argentina",
    "Armenia",
    "Aruba",
    "Australia",
    "Austria",
    "Azerbaijan",
    "Bahamas",
    "Bahrain",
    "Bangladesh",
    "Barbados",
    "Belarus",
    "Belgium",
    "Belize",
    "Benin",
    "Bermuda",
    "Bhutan",
    "Bolivia",
    "Bosnia and Herzegovina",
    "Botswana",
    "Bouvet Island",
    "Brazil",
    "British Indian Ocean Territory",
    "Brunei Darussalam",
    "Bulgaria",
    "Burkina Faso",
    "Burundi",
    "Cambodia",
    "Cameroon",
    "Canada",
    "Cape Verde",
    "Cayman Islands",
    "Central African Republic",
    "Chad",
    "Chile",
    "China",
    "Christmas Island",
    "Cocos (Keeling) Islands",
    "Colombia",
    "Comoros",
    "Congo",
    "Congo, the Democratic Republic of the",
    "Cook Islands",
    "Costa Rica",
    "Cote D'Ivoire",
    "Croatia",
    "Cuba",
    "Cyprus",
    "Czech Republic",
    "Denmark",
    "Djibouti",
    "Dominica",
    "Dominican Republic",
    "Ecuador",
    "Egypt",
    "El Salvador",
    "Equatorial Guinea",
    "Eritrea",
    "Estonia",
    "Ethiopia",
    "Falkland Islands (Malvinas)",
    "Faroe Islands",
    "Fiji",
    "Finland",
    "France",
    "French Guiana",
    "French Polynesia",
    "French Southern Territories",
    "Gabon",
    "Gambia",
    "Georgia",
    "Germany",
    "Ghana",
    "Gibraltar",
    "Greece",
    "Greenland",
    "Grenada",
    "Guadeloupe",
    "Guam",
    "Guatemala",
    "Guinea",
    "Guinea-Bissau",
    "Guyana",
    "Haiti",
    "Heard Island and Mcdonald Islands",
    "Holy See (Vatican City State)",
    "Honduras",
    "Hong Kong",
    "Hungary",
    "Iceland",
    "India",
    "Indonesia",
    "Iran, Islamic Republic of",
    "Iraq",
    "Ireland",
    "Israel",
    "Italy",
    "Jamaica",
    "Japan",
    "Jordan",
    "Kazakhstan",
    "Kenya",
    "Kiribati",
    "Korea, Democratic People's Republic of",
    "Korea, Republic of",
    "Kuwait",
    "Kyrgyzstan",
    "Lao People's Democratic Republic",
    "Latvia",
    "Lebanon",
    "Lesotho",
    "Liberia",
    "Libyan Arab Jamahiriya",
    "Liechtenstein",
    "Lithuania",
    "Luxembourg",
    "Macao",
    "Macedonia, the Former Yugoslav Republic of",
    "Madagascar",
    "Malawi",
    "Malaysia",
    "Maldives",
    "Mali",
    "Malta",
    "Marshall Islands",
    "Martinique",
    "Mauritania",
    "Mauritius",
    "Mayotte",
    "Mexico",
    "Micronesia, Federated States of",
    "Moldova, Republic of",
    "Monaco",
    "Mongolia",
    "Montserrat",
    "Morocco",
    "Mozambique",
    "Myanmar",
    "Namibia",
    "Nauru",
    "Nepal",
    "Netherlands",
    "Netherlands Antilles",
    "New Caledonia",
    "New Zealand",
    "Nicaragua",
    "Niger",
    "Nigeria",
    "Niue",
    "Norfolk Island",
    "Northern Mariana Islands",
    "Norway",
    "Oman",
    "Pakistan",
    "Palau",
    "Palestinian Territory, Occupied",
    "Panama",
    "Papua New Guinea",
    "Paraguay",
    "Peru",
    "Philippines",
    "Pitcairn",
    "Poland",
    "Portugal",
    "Puerto Rico",
    "Qatar",
    "Reunion",
    "Romania",
    "Russian Federation",
    "Rwanda",
    "Saint Helena",
    "Saint Kitts and Nevis",
    "Saint Lucia",
    "Saint Pierre and Miquelon",
    "Saint Vincent and the Grenadines",
    "Samoa",
    "San Marino",
    "Sao Tome and Principe",
    "Saudi Arabia",
    "Senegal",
    "Serbia and Montenegro",
    "Seychelles",
    "Sierra Leone",
    "Singapore",
    "Slovakia",
    "Slovenia",
    "Solomon Islands",
    "Somalia",
    "South Africa",
    "South Georgia and the South Sandwich Islands",
    "Spain",
    "Sri Lanka",
    "Sudan",
    "Suriname",
    "Svalbard and Jan Mayen",
    "Swaziland",
    "Sweden",
    "Switzerland",
    "Syrian Arab Republic",
    "Taiwan, Province of China",
    "Tajikistan",
    "Tanzania, United Republic of",
    "Thailand",
    "Timor-Leste",
    "Togo",
    "Tokelau",
    "Tonga",
    "Trinidad and Tobago",
    "Tunisia",
    "Turkey",
    "Turkmenistan",
    "Turks and Caicos Islands",
    "Tuvalu",
    "Uganda",
    "Ukraine",
    "United Arab Emirates",
    "United Kingdom",
    "United States",
    "United States Minor Outlying Islands",
    "Uruguay",
    "Uzbekistan",
    "Vanuatu",
    "Venezuela",
    "Viet Nam",
    "Virgin Islands, British",
    "Virgin Islands, US",
    "Wallis and Futuna",
    "Western Sahara",
    "Yemen",
    "Zambia",
    "Zimbabwe",
        };

        /// <summary>
        /// Country abbreviations
        /// </summary>
        public static string[] Abbreviations = new string[]
        {
    "AF",
    "AL",
    "DZ",
    "AS",
    "AD",
    "AO",
    "AI",
    "AQ",
    "AG",
    "AR",
    "AM",
    "AW",
    "AU",
    "AT",
    "AZ",
    "BS",
    "BH",
    "BD",
    "BB",
    "BY",
    "BE",
    "BZ",
    "BJ",
    "BM",
    "BT",
    "BO",
    "BA",
    "BW",
    "BV",
    "BR",
    "IO",
    "BN",
    "BG",
    "BF",
    "BI",
    "KH",
    "CM",
    "CA",
    "CV",
    "KY",
    "CF",
    "TD",
    "CL",
    "CN",
    "CX",
    "CC",
    "CO",
    "KM",
    "CG",
    "CD",
    "CK",
    "CR",
    "CI",
    "HR",
    "CU",
    "CY",
    "CZ",
    "DK",
    "DJ",
    "DM",
    "DO",
    "EC",
    "EG",
    "SV",
    "GQ",
    "ER",
    "EE",
    "ET",
    "FK",
    "FO",
    "FJ",
    "FI",
    "FR",
    "GF",
    "PF",
    "TF",
    "GA",
    "GM",
    "GE",
    "DE",
    "GH",
    "GI",
    "GR",
    "GL",
    "GD",
    "GP",
    "GU",
    "GT",
    "GN",
    "GW",
    "GY",
    "HT",
    "HM",
    "VA",
    "HN",
    "HK",
    "HU",
    "IS",
    "IN",
    "ID",
    "IR",
    "IQ",
    "IE",
    "IL",
    "IT",
    "JM",
    "JP",
    "JO",
    "KZ",
    "KE",
    "KI",
    "KP",
    "KR",
    "KW",
    "KG",
    "LA",
    "LV",
    "LB",
    "LS",
    "LR",
    "LY",
    "LI",
    "LT",
    "LU",
    "MO",
    "MK",
    "MG",
    "MW",
    "MY",
    "MV",
    "ML",
    "MT",
    "MH",
    "MQ",
    "MR",
    "MU",
    "YT",
    "MX",
    "FM",
    "MD",
    "MC",
    "MN",
    "MS",
    "MA",
    "MZ",
    "MM",
    "NA",
    "NR",
    "NP",
    "NL",
    "AN",
    "NC",
    "NZ",
    "NI",
    "NE",
    "NG",
    "NU",
    "NF",
    "MP",
    "NO",
    "OM",
    "PK",
    "PW",
    "PS",
    "PA",
    "PG",
    "PY",
    "PE",
    "PH",
    "PN",
    "PL",
    "PT",
    "PR",
    "QA",
    "RE",
    "RO",
    "RU",
    "RW",
    "SH",
    "KN",
    "LC",
    "PM",
    "VC",
    "WS",
    "SM",
    "ST",
    "SA",
    "SN",
    "CS",
    "SC",
    "SL",
    "SG",
    "SK",
    "SI",
    "SB",
    "SO",
    "ZA",
    "GS",
    "ES",
    "LK",
    "SD",
    "SR",
    "SJ",
    "SZ",
    "SE",
    "CH",
    "SY",
    "TW",
    "TJ",
    "TZ",
    "TH",
    "TL",
    "TG",
    "TK",
    "TO",
    "TT",
    "TN",
    "TR",
    "TM",
    "TC",
    "TV",
    "UG",
    "UA",
    "AE",
    "GB",
    "US",
    "UM",
    "UY",
    "UZ",
    "VU",
    "VE",
    "VN",
    "VG",
    "VI",
    "WF",
    "EH",
    "YE",
    "ZM",
    "ZW"
        };
    };
}