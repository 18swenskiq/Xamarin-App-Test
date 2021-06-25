using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using FaceitAPI.SeachPlayers;
using System.Net;
using Android.Graphics;

namespace FaceitAppTest
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            AppCompatButton searchButton = FindViewById<AppCompatButton>(Resource.Id.button1);
            searchButton.Click += SearchPlayerName;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        private void SearchPlayerName(object sender, EventArgs eventArgs)
        {
            // Grab text in box
            var playerTextRaw = FindViewById<AppCompatEditText>(Resource.Id.editText1);
            string playerText = playerTextRaw.Text;

            // Don't need to actually search if its an invalid name
            if (string.IsNullOrWhiteSpace(playerText)) return;

            // Search for the player
            SearchFaceitPlayers search = new SearchFaceitPlayers();
            var result = search.Search("api token here", playerText);

            // Grab the elements of the screen
            var displayNickname = FindViewById<AppCompatTextView>(Resource.Id.faceitnickname);
            var displayPlayerId = FindViewById<AppCompatTextView>(Resource.Id.faceitplayerid);
            var displayCountry = FindViewById<AppCompatTextView>(Resource.Id.faceitcountry);
            var displayAvatar = FindViewById<AppCompatImageView>(Resource.Id.faceitavatar);
            var displayVerified = FindViewById<AppCompatTextView>(Resource.Id.faceitverified);

            // If this is whitespace, then our player wasn't found
            if(string.IsNullOrWhiteSpace(result.Nickname))
            {
                displayNickname.Text = $"{playerText} was not found through Faceit's API";
                displayNickname.Visibility = ViewStates.Visible;
                displayPlayerId.Visibility = ViewStates.Invisible;
                displayCountry.Visibility = ViewStates.Invisible;
                displayAvatar.Visibility = ViewStates.Invisible;
                displayVerified.Visibility = ViewStates.Invisible;
                return;
            }

            displayNickname.Text = $"Nickname: {result.Nickname}";
            displayPlayerId.Text = $"Player ID: {result.PlayerId}";
            displayCountry.Text = $"Country: {result.Country}";
            displayVerified.Text = $"Verified?: {result.Verified}";
            Bitmap avatarBitmap = GetImageBitmapFromUrl(result.Avatar);
            displayAvatar.SetImageBitmap(avatarBitmap);

            // Make visible
            displayNickname.Visibility = ViewStates.Visible;
            displayPlayerId.Visibility = ViewStates.Visible;
            displayCountry.Visibility = ViewStates.Visible;
            displayAvatar.Visibility = ViewStates.Visible;
            displayVerified.Visibility = ViewStates.Visible;
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
