using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerGame
{
    public class PokerTableView : View
    {
        public PokerTableView(Context context) : base(context)
        {
        }
        int tableWidth;
        int tableHeight;
        int left;
        int top;
        protected override void OnDraw(Canvas canvas)
        {
            // Calculate the size and position of the table
       
            tableWidth = canvas.Width / 2;
            tableHeight = canvas.Height / 2;
            left = (canvas.Width - tableWidth) / 2;
            top = (canvas.Height - tableHeight) / 2;
            base.OnDraw(canvas);

            // Draw the table
            DrawTable(canvas);


            Invalidate();
        }


        void DrawTable(Canvas canvas)
        {
            // Set up the paint for the table
            Paint paint = new Paint
            {
                Color = new Color(0x77, 0x6E, 0x65)
            };
            
            // Draw the table
            canvas.DrawRect(left, top, left + tableWidth, top + tableHeight, paint);
        }
    }
}