using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoWallSiniflar
{
    

    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public partial class frmYerlesim : Form
    {

        List<Rectangle> dikdortgenler = new List<Rectangle>();
        private RECT frmEbat;

        private float olcek;
        public frmYerlesim(RECT frmEbat)
        {
            this.frmEbat = frmEbat;
            InitializeComponent();
        }

        private RECT withInMargins(Screen ekran)
        {
            RECT temp = new RECT();

            if ((((ekran.Bounds.Left >= frmEbat.left) && (ekran.Bounds.Left < frmEbat.right))
                &&
                ((ekran.Bounds.Top >= frmEbat.top) && (ekran.Bounds.Top < frmEbat.bottom)))
                ||
                (((ekran.Bounds.Right <= frmEbat.right) && (ekran.Bounds.Right > frmEbat.left))
                &&
                ((ekran.Bounds.Bottom <= frmEbat.bottom) && (ekran.Bounds.Bottom > frmEbat.top))))
            {
                temp.left = (ekran.Bounds.Left > frmEbat.left) ? ekran.Bounds.Left : frmEbat.left;
                temp.top = (ekran.Bounds.Top > frmEbat.top) ? ekran.Bounds.Top : frmEbat.top;
                temp.right = (ekran.Bounds.Right < frmEbat.right) ? ekran.Bounds.Right : frmEbat.right;
                temp.bottom = (ekran.Bounds.Bottom < frmEbat.bottom) ? ekran.Bounds.Bottom : frmEbat.bottom;
            }
            return temp;
        }
        private void frmYerlesim_Load(object sender, EventArgs e)
        {
            
            pnlfrmTb.Width = pnlYerlesim.Width - 20;
            float pnlolcek = (float)(frmEbat.right - frmEbat.left) / (frmEbat.bottom - frmEbat.top);

            pnlfrmTb.Height = Convert.ToInt32(pnlfrmTb.Width / pnlolcek);

            olcek = (float)pnlfrmTb.Width / (frmEbat.right - frmEbat.left+1); 

            pnlfrmTb.Location = new Point(((pnlYerlesim.Width-pnlfrmTb.Width)/2), ((pnlYerlesim.Height-pnlfrmTb.Height)/2));

            foreach (Screen sc in Screen.AllScreens)
            {
                RECT girdi = withInMargins(sc);
                if (!girdi.Equals(default(RECT)))
                {
                    dikdortgenler.Add(new Rectangle(Convert.ToInt32(girdi.left*olcek), Convert.ToInt32(girdi.top*olcek), Convert.ToInt32((girdi.right - girdi.left + 1)*olcek)-1, Convert.ToInt32((girdi.bottom - girdi.top + 1)*olcek)-1));
                }
            }

            pnlfrmTb.Refresh();
            Bitmap bm = new Bitmap(Owner.Width, Owner.Height);

            Owner.DrawToBitmap(bm, new Rectangle(0, 0, Owner.Width, Owner.Height));
            
            pnlfrmTb.Controls.Add(new Panel()
            {
                Name = "pic1",
            Width = 100,
            Height = 100,
            
            }
                
                );
            Panel pnl = (Panel)pnlfrmTb.Controls.Find("pic1", true)[0];
            pnl.BackgroundImage = bm;
            pnl.BackgroundImageLayout = ImageLayout.Stretch;
            Label lb = new Label()
            {
                Text = "1",
                Font = new Font("Times New Roman", 10, FontStyle.Bold),
                ForeColor = Color.Red,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter

            };
            pnl.Controls.Add(lb);
            
            pnl.Paint += Pnl_Paint;
            pnl.Refresh();
            ToolTip tt = new ToolTip();
            tt.ShowAlways = true;
            tt.AutoPopDelay = 3000;
            tt.InitialDelay = 500;
            tt.ReshowDelay = 500;
            tt.SetToolTip(lb, "XXX Demo");

        }

        private void Pnl_Paint(object sender, PaintEventArgs e)
        {
            Control ctrl = (Control)sender;
            Pen p = new Pen(Color.Red,3);
            e.Graphics.DrawEllipse(p, ctrl.Location.X + (ctrl.Width / 2)-15, ctrl.Location.Y + (ctrl.Height / 2)-15, 30, 30);
            p.Color = Color.Black;
            e.Graphics.DrawRectangle(p, 0, 0, ctrl.Width, ctrl.Height);
            base.OnPaint(e);
        }

        private void pnlfrmTb_Paint(object sender, PaintEventArgs e)
        {
            Pen br = new Pen(Color.Red);
            foreach (Rectangle rct in dikdortgenler)
            {
                e.Graphics.DrawRectangle(br, rct);
            }
            base.OnPaint(e);
        }
    }
}
