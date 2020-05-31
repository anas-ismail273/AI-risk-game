using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_game_AI
{
    class game_controller
    {

        public void draw_all_circles (Bitmap myBitmap , int frame_width , int frame_height)
        {
            var d = USA_City_details.Instance;
            Graphics gr = Graphics.FromImage(myBitmap);
            
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen pr = new Pen(Color.Red, 4);
            Pen pb = new Pen(Color.Blue, 4);

            Rectangle r = new Rectangle();
            FontFamily fontFamily = new FontFamily("Arial");
            Font font = new Font(
               fontFamily,
               24,
               FontStyle.Regular,
               GraphicsUnit.Pixel);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            r.Width = 50;
            r.Height = 50;

            for (int i=0; i<50; i++)
            {
                r.X = (myBitmap.Width * d.GetCity(i).Get_circle_center().X / frame_width) - 25;
                r.Y = (myBitmap.Height * d.GetCity(i).Get_circle_center().Y /frame_height) - 25;

                if (d.with_P1[i])
                gr.DrawEllipse(pr, r);
                else if (d.with_P2[i])
                    gr.DrawEllipse(pb, r);
                gr.DrawString(d.GetCity(i).No_of_Soliders().ToString(), font, drawBrush,r.X+15,r.Y+12);
            }

        }

        public int CalculateDeployTroops ()
        {
            int n = Game_state.Instance.no_of_map_cities();
            int count = 0;
            for (int i = 0; i < n; i++)
                if (Game_state.Instance.WithMe(i))
                    count++;

            return count / 3;
            
        }

        public bool Player_WIN ()
        {
            var d = USA_City_details.Instance;
            var n = Game_state.Instance ;
            for (int i=0; i< n.no_of_map_cities(); i++)
            {
                if (!n.WithMe(i))
                    return false;
            }
            return true;
        }

    }
}
