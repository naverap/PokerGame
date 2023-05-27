using Android.Content;
using Android.OS;
using Android.Widget;

namespace PokerGame
{
    public class NotifyHandler : Handler
    {
        Context _context;

        public NotifyHandler(Context context)
        {
            _context = context;
        }

        public override void HandleMessage(Message msg)
        {
            Toast.MakeText(_context, "waiting for your action", ToastLength.Long).Show();
        }
    }
}