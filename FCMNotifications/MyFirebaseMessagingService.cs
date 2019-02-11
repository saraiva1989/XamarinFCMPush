using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;

namespace FCMNotifications
{
    [Service(Exported = false)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;

        //método responsavel pelo recebimento da mensagem do push
        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);

            var body = "Não possui a estrutura Notification no push";
            if (message.GetNotification() != null)
            {
                body = message.GetNotification().Body;
            }
            var data = message.Data;
            SendNotification(body, message.Data);
        }

        //metodo responsavel por montar o push notification (se vibra, titulo, descrição, som, etc)
        void SendNotification(string messageBody, IDictionary<string, string> data)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }

            var pendingIntent = PendingIntent.GetActivity(this, NOTIFICATION_ID, intent, PendingIntentFlags.UpdateCurrent);
            NotificationCompat.Builder notificationBuilder = null;

            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                notificationBuilder = new NotificationCompat.Builder(this);
                notificationBuilder.SetDefaults(Convert.ToInt32(NotificationDefaults.All));
            }
            else
            {
                CreateNotificationChannel();
                notificationBuilder = new NotificationCompat.Builder(this, CHANNEL_ID);
                notificationBuilder.SetChannelId(CHANNEL_ID);
            }

            notificationBuilder.SetSmallIcon(Resource.Drawable.ic_stat_ic_notification);
            notificationBuilder.SetContentTitle(data["Tit"]);
            notificationBuilder.SetContentText(data["Msg"]);
            notificationBuilder.SetAutoCancel(true);
            notificationBuilder.SetPriority(Convert.ToInt32(NotificationCompat.PriorityMax));
            notificationBuilder.SetContentIntent(pendingIntent);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(NOTIFICATION_ID, notificationBuilder.Build());

        }

        //metodo responsavel por criar o canal do push, pois apartir do android 8 é necessário criar as
        //categorias do push
        void CreateNotificationChannel()
        {
            try
            {
                if (Build.VERSION.SdkInt < BuildVersionCodes.O)
                {
                    // Notification channels are new in API 26 (and not a part of the
                    // support library). There is no need to create a notification 
                    // channel on older versions of Android.
                    return;
                }


                var channel = new NotificationChannel(CHANNEL_ID, "FCM Notifications", NotificationImportance.High)
                {
                    Description = "Firebase Cloud Messages appear in this channel"
                };
                channel.CanBypassDnd();
                channel.SetBypassDnd(true);
                channel.SetShowBadge(true);
                channel.Importance = NotificationImportance.High;

                channel.EnableVibration(true);
                channel.LockscreenVisibility = NotificationVisibility.Public;
                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }

            catch (System.Exception ex)
            {
                ex.Message.ToString();
            }
        }


    }
}
