using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace VideoWallSiniflar
{





[StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public partial class frmYerlesim : Form
    {


        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        List<Rectangle> dikdortgenler = new List<Rectangle>();
        private RECT frmEbat;
        public const int SRCCOPY = 13369376;

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

            AxRDPCOMAPILib.AxRDPViewer ctrl = (AxRDPCOMAPILib.AxRDPViewer)Owner.Controls.Find("AxRDPViewer1", true)[0];

            IntPtr hdcSrc = GetWindowDC(ctrl.Handle);

            RECT windowRect = new RECT();
            GetWindowRect(ctrl.Handle, ref windowRect);

            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;

            IntPtr hdcDest = CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = CreateCompatibleBitmap(hdcSrc, width, height);

            IntPtr hOld = SelectObject(hdcDest, hBitmap);
            BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, SRCCOPY);
            SelectObject(hdcDest, hOld);
            DeleteDC(hdcDest);
            ReleaseDC(ctrl.Handle, hdcSrc);

            Image image = Image.FromHbitmap(hBitmap);
            DeleteObject(hBitmap);

            



            pnlfrmTb.Controls.Add(new Panel()
            {
                Name = "pic1",
            Width = Convert.ToInt32(ctrl.Width*olcek),
            Height = Convert.ToInt32(ctrl.Height * olcek)
            
            }
                
                );
            Panel pnl = (Panel)pnlfrmTb.Controls.Find("pic1", true)[0];
            pnl.BackgroundImage = image;
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

        private void ReverseControlZIndex(Control parentControl)
        {
            var list = new List<Control>();
            foreach (Control i in parentControl.Controls)
            {
                list.Add(i);
            }
            var total = list.Count;
            for (int i = 0; i < total / 2; i++)
            {
                var left = parentControl.Controls.GetChildIndex(list[i]);
                var right = parentControl.Controls.GetChildIndex(list[total - 1 - i]);

                parentControl.Controls.SetChildIndex(list[i], right);
                parentControl.Controls.SetChildIndex(list[total - 1 - i], left);
            }
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
