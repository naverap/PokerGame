using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Firebase.Firestore;
using System.Threading;

namespace PokerGame
{
    [Service(Enabled = true)]
    public class NotifyService : Service, IOnSuccessListener
    {
        string playerName;
        NotifyHandler handler;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            handler = new NotifyHandler(this);
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            playerName = intent.GetStringExtra("PlayerName");
            var thread = new Thread(Run);
            thread.Start();
            return base.OnStartCommand(intent, flags, startId);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var game = Repository.GetGame(result as DocumentSnapshot);
            var me = game.GetPlayerByName(playerName);
            if (game.CurrentPlayerIndex == me.Id)
            {
                handler.SendMessage(new Message());
            }
        }

        private void Run()
        {
            while (true)
            {
                Thread.Sleep(30000);
                Repository.GameDocument.Get().AddOnSuccessListener(this);
            }
        }
    }
}