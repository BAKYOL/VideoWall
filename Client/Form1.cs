using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using RDPCOMAPILib;

namespace Client
{

    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        // http://msdn.microsoft.com/en-us/library/a5ch4fda(VS.80).aspx
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;

        private TcpListener tcplis;
        private Thread lisThread;
        private TcpClient tcpclient;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lisThread = new Thread(new ThreadStart(listener));
            lisThread.IsBackground = true;
            lisThread.Start();
        }
        private void listener()
        {
            try
            {
                tcplis = new TcpListener(IPAddress.Any, 57000);
                Byte[] bytes = new Byte[1024];
                tcplis.Start();

                while (true)
                {
                    using (tcpclient = tcplis.AcceptTcpClient())
                    {
                        // Get a stream object for reading 					
                        using (NetworkStream stream = tcpclient.GetStream())
                        {
                            int length;
                            // Read incomming stream into byte arrary. 						
                            while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                            {
                                var incommingData = new byte[length];
                                Array.Copy(bytes, 0, incommingData, 0, length);
                                // Convert byte array to string message. 							
                                string clientMessage = Encoding.ASCII.GetString(incommingData);
                                MessageBox.Show(clientMessage);
                                if (clientMessage == "RDPler edir?")
                                {
                                    try
                                    {
                                        // Get a stream object for writing. 			
                                        NetworkStream streamc = tcpclient.GetStream();
                                        if (streamc.CanWrite)
                                        {
                                            
                                            // Convert string message to byte array.                 
                                            byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(textBox1.Text);
                                            // Write byte array to socketConnection stream.               
                                            streamc.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                                            
                                        }
                                    }
                                    catch (SocketException socketException)
                                    {
                                        MessageBox.Show("Socket exception: " + socketException);
                                    }
                                }
                                while (tcpclient.Connected)
                                {
                                    System.Threading.Thread.Sleep(100);
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public static RDPSession currentSession = null;
        private void button1_Click(object sender, EventArgs e)
        {
            Process ps = Process.Start("msedge", @"--new-window --user-data-dir=c:\deneme http://www.gmail.com");
            Thread.Sleep(2000);
            SetWindowPos(ps.MainWindowHandle, IntPtr.Zero, Screen.AllScreens[0].WorkingArea.Left, Screen.AllScreens[0].WorkingArea.Top, 500,500,0);
            createSession();
            RECT rect = new RECT();

            GetWindowRect(ps.MainWindowHandle, ref rect);
            Connect(currentSession,rect);
            float ratio = ((float)rect.Right - rect.Left+1) / ((float)rect.Bottom - rect.Top+1);
            textBox1.Text = getConnectionString(currentSession,
                "Test"+(ratio).ToString(), "Group", "", 5);

        }

        public static void createSession()
        {
            currentSession = new RDPSession();
        }

        public static void Connect(RDPSession session, RECT rect)
        {
            session.OnAttendeeConnected += Incoming;
            session.Properties["PortId"] = 57001;
            session.SetDesktopSharedRect(rect.Left, rect.Top, rect.Right, rect.Bottom);
            session.Open();
        }

        public static void Disconnect(RDPSession session)
        {
            session.Close();
        }

        public static string getConnectionString(RDPSession session, String authString,
            string group, string password, int clientLimit)
        {
            IRDPSRAPIInvitation invitation =
                session.Invitations.CreateInvitation
                (authString, group, password, clientLimit);
            return invitation.ConnectionString;
        }

        private static void Incoming(object Guest)
        {
            IRDPSRAPIAttendee MyGuest = (IRDPSRAPIAttendee)Guest;
            MyGuest.ControlLevel = CTRL_LEVEL.CTRL_LEVEL_INTERACTIVE;
        }
    }
}
