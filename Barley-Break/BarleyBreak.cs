using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace Barley_Break
{
    class BarleyBreak : Button
    {
        public int SizeBarleyBreak = 65;

        public BarleyBreak()
        {
            Visible = true;
            Size = new Size(SizeBarleyBreak, SizeBarleyBreak);
            UseVisualStyleBackColor = true;
        }

        protected override void OnMouseClick(MouseEventArgs mevent)
        {
            Point Cell = new Point((Location.X + SizeBarleyBreak) / SizeBarleyBreak, (Location.Y + SizeBarleyBreak) / SizeBarleyBreak);
            Map.MoveCell(Cell);
        }

        public void Animation(Point position)
        {
            int x = position.X - (Location.X + SizeBarleyBreak) / SizeBarleyBreak;
            int y = position.Y - (Location.Y + SizeBarleyBreak) / SizeBarleyBreak;
            Point startLocation = Location;
            while (Location.X != startLocation.X + Size.Width * x || Location.Y != startLocation.Y + Size.Height * y)
            {
                Location = new Point(Location.X + x, Location.Y + y);
                System.Threading.Thread.Sleep(1);      
            }
        }

    }
    
}
