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



namespace Server
{
    public partial class frmDisplay : Form
    {


        public frmDisplay()
        {
            InitializeComponent();
        }
        private TcpClient tcpclient; 
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                tcpclient = new TcpClient("localhost", 57000);
                Byte[] bytes = new Byte[1024];
                
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
                            string serverMessage = Encoding.ASCII.GetString(incommingData);
                            MessageBox.Show(serverMessage);
                            break;
                            //Debug.Log("server message received as: " + serverMessage);
                        }
                    }
                
            }
            catch { }
        }
    }
}
