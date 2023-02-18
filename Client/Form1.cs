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

namespace Client
{
    public partial class Form1 : Form
    {
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
                                if (clientMessage == "RDPler nedir?")
                                {
                                    try
                                    {
                                        // Get a stream object for writing. 			
                                        NetworkStream streamc = tcpclient.GetStream();
                                        if (streamc.CanWrite)
                                        {
                                            string serverMessage = "This is a message from your server.";
                                            // Convert string message to byte array.                 
                                            byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
                                            // Write byte array to socketConnection stream.               
                                            streamc.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                                            
                                        }
                                    }
                                    catch (SocketException socketException)
                                    {
                                        MessageBox.Show("Socket exception: " + socketException);
                                    }
                                }
                                tcpclient.Close();
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
    }
}
