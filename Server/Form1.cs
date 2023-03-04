using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using AxRDPCOMAPILib;


namespace Server
{
    public partial class frmDisplay : Form
    {


        public frmDisplay()
        {
            InitializeComponent();
        }
        private TcpClient tcpclient;

        public static void Connect(string invitation, AxRDPViewer display, string userName, string password)
        {
            display.Connect(invitation, userName, password);
        }

        public static void disconnect(AxRDPViewer display)
        {
            display.Disconnect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string serverMessage="";
            try
            {
                tcpclient = new TcpClient("localhost", 57000);
                Byte[] bytes = new Byte[100000];
                
                    try
                    {
                        // Get a stream object for writing. 			
                        NetworkStream stream = tcpclient.GetStream();
                        if (stream.CanWrite)
                        {
                            string clientMessage = "RDPler edir?";
                            // Convert string message to byte array.                 
                            byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                            // Write byte array to socketConnection stream.                 
                            stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                            
                        }
                    }
                    catch { }

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
                            serverMessage = Encoding.ASCII.GetString(incommingData);
                            //MessageBox.Show(serverMessage);
                            break;
                            //Debug.Log("server message received as: " + serverMessage);
                        }
                    }
                
            }
            catch { }

            try
            {
                
                Connect(serverMessage, this.axRDPViewer1, "", "");

                double size = Convert.ToDouble(serverMessage.Split(new string[] { "Test" }, StringSplitOptions.None)[1].Substring(0, serverMessage.Split(new string[] { "Test" }, StringSplitOptions.None)[1].IndexOf('"') - 1));
                if (panel2.Width > (int)(panel2.Height*size))
                {
                    axRDPViewer1.Height = panel2.Height;
                    axRDPViewer1.Width = Convert.ToInt32(axRDPViewer1.Height * size);
                }
                else
                {
                    axRDPViewer1.Width = panel2.Width;
                    axRDPViewer1.Height = Convert.ToInt32(axRDPViewer1.Width / size);
                }
                axRDPViewer1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                axRDPViewer1.SmartSizing = true;
                
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to connect to the Server");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VideoWallSiniflar.RECT frmTb = new VideoWallSiniflar.RECT();

            frmTb.left = 0;
            frmTb.top = 0;
            frmTb.right = 3000;
            frmTb.bottom = 1079;



            VideoWallSiniflar.frmYerlesim frmLayout = new VideoWallSiniflar.frmYerlesim(frmTb);
            frmLayout.ShowDialog(this);
        }
    }
}
