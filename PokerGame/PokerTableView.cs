using Android.Content;
using Android.Graphics;
using Android.Views;

namespace PokerGame
{
    public class PokerTableView : View
    {
        public PokerTableView(Context context) : base(context)
        {
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            // Calculate the size and position of the table
            var width = canvas.Width / 2;
            var height = canvas.Height / 2;
            var left = (canvas.Width - width) / 2;
            var top = (canvas.Height - height) / 2;

            // Set up the paint for the table
            var paint = new Paint
            {
                Color = new Color(0x77, 0x6E, 0x65)
            };

            // Draw the table
            canvas.DrawRect(left, top, left + width, top + height, paint);

            Invalidate();
        }
    }
}