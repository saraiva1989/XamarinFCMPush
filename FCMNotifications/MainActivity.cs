using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Util;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Gms.Common;
using Android.Content;
using System;

namespace FCMNotifications
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        const string TAG = "MainActivity";
        TextView msgText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main);
            msgText = FindViewById<TextView>(Resource.Id.msgText);
            var texto = "";

            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    texto = texto + " " + value;
                    Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                }
                Toast.MakeText(this, texto, ToastLength.Long).Show();
            }

            //CreateNotificationChannel();

            IsPlayServicesAvailable();
            Firebase.FirebaseApp.InitializeApp(this);


            var logTokenButton = FindViewById<Button>(Resource.Id.logTokenButton);
            logTokenButton.Click += delegate { Log.Debug(TAG, "InstanceID token: " + FirebaseInstanceId.Instance.Token); };
            Console.WriteLine("InstanceID token: " + FirebaseInstanceId.Instance.Token);
            var subscribeButton = FindViewById<Button>(Resource.Id.subscribeButton);
            subscribeButton.Click += delegate
            {
                try
                {
                    //FirebaseMessaging.Instance.SubscribeToTopic("news");
                    //Log.Debug(TAG, "Subscribed to remote notifications");
                    Intent intent = new Intent(this, typeof(Activity2));
                    StartActivity(intent);
                    Finish();
                }
                catch (System.Exception ex)
                {

                    Log.Debug("fire error ", ex.Message);
                }
            };
        }

        protected override void OnResume()
        {
            base.OnResume();
            var texto = "";
            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    texto = texto + " " + value;
                    Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                }
                Toast.MakeText(this, texto, ToastLength.Long).Show();

            }
        }

        public bool IsPlayServicesAvailable()
        {
            var resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                }
                else
                {
                    msgText.Text = "This device is not supported";
                    Finish();
                }

                return false;
            }

            msgText.Text = "Google Play Services is available.";
            return true;
        }
    }
}