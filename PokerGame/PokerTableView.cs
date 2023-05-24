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

            // Calculate the size of the table
            var margin = 20;
            var cardWidth = 180;
            var cardHeight = 261;

            var cardsWidth = cardWidth * 5 + margin * 4;

            var width = cardsWidth + margin * 2;
            var height = cardHeight + margin * 2;
            //var width = canvas.Width / 2;
            //var height = canvas.Height / 2;

            // Calculate the position of the table
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