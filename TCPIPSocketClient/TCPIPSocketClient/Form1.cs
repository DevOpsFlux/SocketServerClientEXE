/*------------------------------------------------------------------------
'	프로젝트		: 소켓통신 테스트
'	작성자			: DevOpsFlux
'	작성일			: 2013-09-05
'	설명			: TCP/IP Client
' ------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TCPIPSocketClient
{
    public partial class Form1 : Form
    {
        private System.Threading.Thread ThreadClient;
        private Socket SocketClient;        
        private IPAddress ServerIPAddress;
        private int PORT;
        private IPEndPoint ServerIPEndPoint;
        private IPEndPoint ClientIPEndPoint;

        public Form1()
        {
            InitializeComponent();

            SetInit();
        }

        private void SetInit()
        {
            lstMsg.Items.Clear();
            txtServerIP.Text = "127.0.0.1";
            txtServerPort.Text = "4001";
            txtSendMsg.Text = "[Client] Client TEST Msg Send~!!! ";
            lstMsg.Items.Add("[Client] 소켓 클라이언트 실행 중 입니다.");
        }

        private void RecieveFromServer()
        {
            string ReceiveData = null;

            while (true)
            {
                try
                {
                    Byte[] ReceiveByte = new Byte[1024];

                    int nValue = SocketClient.Receive(ReceiveByte, ReceiveByte.Length, 0);

                    if (nValue > 0)
                    {
                        //ReceiveData = System.Text.Encoding.Unicode.GetString(ReceiveByte);
                        ReceiveData = System.Text.Encoding.UTF8.GetString(ReceiveByte);
                        Console.WriteLine(ReceiveData);
                        lstMsg.Items.Add(ReceiveData);
                    }
                }
                catch (Exception ex)
                {
                    if (!SocketClient.Connected)
                    {                        
                        lstMsg.Items.Add("[Client] " + ex.Message.ToString());
                        break;
                    }
                }
            }
            ThreadClient.Abort();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (SocketClient.Connected)
            {
                Byte[] SendBuffer;
                string strBuffer;
                strBuffer = String.Format("{0}", txtSendMsg.Text.Trim());

                SendBuffer = System.Text.Encoding.Unicode.GetBytes(strBuffer);
                SocketClient.Send(SendBuffer, 0, SendBuffer.Length, 0);                
                lstMsg.Items.Add("[Client] 서버로 데이터를 전송.");
            }
        }

        private void btnConn_Click(object sender, EventArgs e)
        {
            if (txtServerIP.Text.Trim() == "") {
                MessageBox.Show("연결할 서버 IP주소를 입력하세요!!", "TCP/IP 소켓 클라이언트");
                txtServerIP.Focus();
                return;
            }
            if (txtServerIP.Text.IndexOf(".") == -1) {
                MessageBox.Show("IP 주소를 정확히 입력하세요 !!", "TCP/IP 소켓 클라이언트");
                txtServerIP.Focus();
                return;
            }
            if (txtServerPort.Text.Trim() == "") {
                MessageBox.Show("연결할 서버 포트 번호를 입력하세요 !!");
                txtServerPort.Focus();
                return;
            }

            //ClientIPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4650);
            //SocketClient = new Socket(ClientIPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // # 포트재사용(SO_REUSEADDR)
            //SocketClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);


            ServerIPAddress = IPAddress.Parse(txtServerIP.Text.Trim());
            PORT = Int32.Parse(txtServerPort.Text.Trim());
            ServerIPEndPoint = new IPEndPoint(ServerIPAddress, PORT);

            try
            {
                SocketClient.Connect(ServerIPEndPoint);
                //SocketClient.Connect(ServerIPAddress, PORT);
                
                if (SocketClient.Connected)
                {
                    ThreadClient = new System.Threading.Thread(new System.Threading.ThreadStart(RecieveFromServer));
                    ThreadClient.Start();                    
                    lstMsg.Items.Add("[Client] 서버로부터 데이터를 받기 위해 대기중.");
                    btnConn.Enabled = false;
                    btnSend.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lstMsg.Items.Add("[Client] 서버연결 오류");
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnConnEnd_Click(object sender, EventArgs e)
        {
            if (SocketClient != null)
            {
                SocketClient.Close();
            }
            if (ThreadClient != null)
            {
                if (ThreadClient.IsAlive)
                {
                    ThreadClient.Abort();
                }
            }

            btnConn.Enabled = true;
            lstMsg.Items.Add("[Client] 클라이언트가 종료되었습니다.");
            //MessageBox.Show("클라이언트가 종료되었습니다.");
            //Application.Exit();
            
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            lstMsg.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TCP3WayHandshake();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            TCP4WayHandshake();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TCPHalfClose();
        }

        private void TCP3WayHandshake()
        {
            if (txtServerIP.Text.Trim() == "")            {
                MessageBox.Show("연결할 서버 IP주소를 입력하세요!!", "TCP/IP 소켓 클라이언트");
                txtServerIP.Focus();
                return;
            }
            if (txtServerIP.Text.IndexOf(".") == -1)            {
                MessageBox.Show("IP 주소를 정확히 입력하세요 !!", "TCP/IP 소켓 클라이언트");
                txtServerIP.Focus();
                return;
            }
            if (txtServerPort.Text.Trim() == "")            {
                MessageBox.Show("연결할 서버 포트 번호를 입력하세요 !!");
                txtServerPort.Focus();
                return;
            }

            ClientIPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4650);
            SocketClient = new Socket(ClientIPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            ServerIPAddress = IPAddress.Parse(txtServerIP.Text.Trim());
            PORT = Int32.Parse(txtServerPort.Text.Trim());

            // # Send 타임아웃
            //SocketClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 1000);

            // # SO_Linger
            //LingerOption myOpts = new LingerOption(true, 1);
            //SocketClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, myOpts);
                        
            // # 포트재사용(SO_REUSEADDR)
            //SocketClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);

            ServerIPEndPoint = new IPEndPoint(ServerIPAddress, PORT);
            try
            {
                SocketClient.Connect(ServerIPEndPoint);

                if (SocketClient.Connected)
                {
                    Byte[] SendBuffer;
                    string strBuffer;
                    strBuffer = String.Format("{0}", "SYN");
                    SendBuffer = System.Text.Encoding.Unicode.GetBytes(strBuffer);                    
                    SocketClient.Send(SendBuffer, 0, SendBuffer.Length, 0);

                    lstMsg.Items.Add("");
                    lstMsg.Items.Add("=========== TCP Three-Way Handshake TEST Start ===========");
                    lstMsg.Items.Add("[Client] -> [Server] : 연결요청 (SYN=1, ACK=0)");

                    string ReceiveData = null;
                    Byte[] ReceiveByte = new Byte[1024];
                    int nValue = SocketClient.Receive(ReceiveByte, ReceiveByte.Length, 0);
                    if (nValue > 0)
                    {
                        ReceiveData = System.Text.Encoding.Unicode.GetString(ReceiveByte);
                        lstMsg.Items.Add(ReceiveData);
                        if (ReceiveData.IndexOf("ACK") >= 0)
                        {
                            lstMsg.Items.Add("[Client] <- [Server] : 연결허락 (SYN=1, ACK=1)");
                            strBuffer = String.Format("{0}", "ACK");
                            SendBuffer = System.Text.Encoding.Unicode.GetBytes(strBuffer);
                            SocketClient.Send(SendBuffer, 0, SendBuffer.Length, 0);
                            lstMsg.Items.Add("[Client] -> [Server] : 연결설정 (ACK=1)");
                            lstMsg.Items.Add("=========== TCP Three-Way Handshake TEST END ===========");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lstMsg.Items.Add("[Client] 서버연결 오류");
                MessageBox.Show(ex.ToString());
            }
        }

        private void TCP4WayHandshake()
        {
            if (SocketClient.Connected)
            {
                lstMsg.Items.Add("");
                lstMsg.Items.Add("=========== TCP Four-Way Handshake TEST Start ===========");
                lstMsg.Items.Add("[Client] -> [Server] : 연결종료요청 (FIN_ACK)");
                Byte[] SendBuffer;
                string strBuffer;
                strBuffer = String.Format("{0}", "[FIN_ATK]");
                SendBuffer = System.Text.Encoding.Unicode.GetBytes(strBuffer);
                SocketClient.ReceiveTimeout = 3000;
                SocketClient.Send(SendBuffer, 0, SendBuffer.Length, 0);
            }

            while (true)
            {
                if (SocketClient.Connected)
                {
                    string ReceiveData = null;
                    Byte[] ReceiveByte = new Byte[1024];
                    int nValue = SocketClient.Receive(ReceiveByte, ReceiveByte.Length, 0);
                    if (nValue > 0)
                    {
                        ReceiveData = System.Text.Encoding.Unicode.GetString(ReceiveByte);
                        //lstMsg.Items.Add(ReceiveData);
                        if (ReceiveData.IndexOf("[ATK]") >= 0)
                        {
                            lstMsg.Items.Add("[Client] <- [Server] : 연결종료확인 (FIN_WAIT_2 <- ACK)");
                        }
                        else if (ReceiveData.IndexOf("[FIN_ATK]") >= 0)
                        {
                            lstMsg.Items.Add("[Client] <- [Server] : 연결종료요청 (FIN_ACK)");
                            Byte[] SendBuffer;
                            string strBuffer;
                            strBuffer = String.Format("{0}", "[LAST_ATK]");
                            SendBuffer = System.Text.Encoding.Unicode.GetBytes(strBuffer);
                            //SocketClient.Shutdown(SocketShutdown.Receive);
                            //lstMsg.Items.Add("[Client] : Half Close - SocketShutdown.Receive Stop");
                            SocketClient.Send(SendBuffer, 0, SendBuffer.Length, 0);
                            lstMsg.Items.Add("[Client] -> [Server] : 연결종료완료요청 (LAST_ACK)");
                            SocketClient.Close();
                            lstMsg.Items.Add("[Client] : 연결종료완료 (CLOSED)");
                            lstMsg.Items.Add("=========== TCP Four-Way Handshake TEST END ===========");
                            break;
                        }
                    }

                }
            }
            //SocketClient.Shutdown(SocketShutdown.Both);
                //SocketClient.ReceiveTimeout = 3000;
            //SocketClient.Shutdown(SocketShutdown.Send);
                //SocketClient.Shutdown(SocketShutdown.Receive);
            //SocketClient.Disconnect(false);
            //SocketClient.Close();
        }

        private void TCPHalfClose()
        {
            if (SocketClient.Connected)
            {
                lstMsg.Items.Add("");
                lstMsg.Items.Add("=========== TCP Half Close TEST Start ===========");
                lstMsg.Items.Add("[Client] -> [Server] : 연결종료요청 (FIN_ACK)");
                Byte[] SendBuffer;
                string strBuffer;
                strBuffer = String.Format("{0}", "[FIN_ATK]");
                SendBuffer = System.Text.Encoding.Unicode.GetBytes(strBuffer);
                SocketClient.ReceiveTimeout = 3000;
                SocketClient.Send(SendBuffer, 0, SendBuffer.Length, 0);
            }

            while (true)
            {
                if (SocketClient.Connected)
                {
                    string ReceiveData = null;
                    Byte[] ReceiveByte = new Byte[1024];
                    int nValue = SocketClient.Receive(ReceiveByte, ReceiveByte.Length, 0);
                    if (nValue > 0)
                    {
                        ReceiveData = System.Text.Encoding.Unicode.GetString(ReceiveByte);
                        //lstMsg.Items.Add(ReceiveData);
                        if (ReceiveData.IndexOf("[ATK]") >= 0)
                        {
                            lstMsg.Items.Add("[Client] <- [Server] : 연결종료확인 (FIN_WAIT_2 <- ACK)");
                        }
                        else if (ReceiveData.IndexOf("[FIN_ATK]") >= 0)
                        {
                            lstMsg.Items.Add("[Client] <- [Server] : 연결종료요청 (FIN_ACK)");
                            Byte[] SendBuffer;
                            string strBuffer;
                            strBuffer = String.Format("{0}", "[HC_LAST_ATK]");
                            SendBuffer = System.Text.Encoding.Unicode.GetBytes(strBuffer);
                            SocketClient.Shutdown(SocketShutdown.Receive);
                            lstMsg.Items.Add("[Client] : Half Close - SocketShutdown.Receive Stop");
                            SocketClient.Send(SendBuffer, 0, SendBuffer.Length, 0);                            
                            lstMsg.Items.Add("[Client] -> [Server] : 연결종료완료요청 (LAST_ACK)");
                            SocketClient.Shutdown(SocketShutdown.Both);
                            SocketClient.Disconnect(false);
                            SocketClient.Close();
                            lstMsg.Items.Add("[Client] : 연결종료완료 (CLOSED)");
                            lstMsg.Items.Add("=========== TCP Four-Way Handshake TEST END ===========");
                            break;
                        }
                    }

                }
            }

        }


    }
}
