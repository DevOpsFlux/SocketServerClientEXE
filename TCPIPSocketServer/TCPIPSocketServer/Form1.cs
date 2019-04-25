/*------------------------------------------------------------------------
'	프로젝트		: 소켓통신 테스트
'	작성자			: DevOpsFlux
'	작성일			: 2013-09-05
'	설명			: TCP/IP Server
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
using System.Threading;
using System.Collections;

namespace TCPIPSocketServer
{
    public partial class Form1 : Form
    {
        private TcpListener ServerTCPListner;
        private static long ConnectedSockedCount = 0;
        private Hashtable HTSocketHolder = new Hashtable();
        private Hashtable HTThreadHolder = new Hashtable();

        private int MaximumConnected = 50;
        private Socket TCPListnerAcceptSocket;
        private int PORT;
        
        public Form1()
        {
            InitializeComponent();

            SetInit();
        }

        private void SetInit()
        {
            lstMsg.Items.Clear();
            txtServerPort.Text = "4001";
            txtSendMsg.Text = "[Server] Server TEST Msg Send~!!!";
            lstMsg.Items.Add("[Server] 소켓 서버 준비중...");            
                                    
        }

        public void WaitingFromClientConnect()
        {
            while (true)
            {
                TCPListnerAcceptSocket = ServerTCPListner.AcceptSocket();

                if (ConnectedSockedCount < 5000)
                {
                    Interlocked.Increment(ref ConnectedSockedCount);
                }
                else
                {
                    ConnectedSockedCount = 1;
                }

                if (HTSocketHolder.Count < MaximumConnected)
                {
                    while (HTSocketHolder.Contains(ConnectedSockedCount))
                    {
                        Interlocked.Increment(ref ConnectedSockedCount);
                    }

                    Thread myServerThread = new Thread(new ThreadStart(ReadingServerSocket));

                    lock (this)
                    {
                        HTSocketHolder.Add(ConnectedSockedCount, TCPListnerAcceptSocket);
                        HTThreadHolder.Add(ConnectedSockedCount, myServerThread);
                    }

                    myServerThread.Start();
                }
            }
                        
        }

        public void ReadingServerSocket()
        {
            long RealConnectedSocket = ConnectedSockedCount;
            Socket ReadServerSocket = (Socket)HTSocketHolder[RealConnectedSocket];
            
            // # 포트재사용(SO_REUSEADDR)
            //ReadServerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            
            lblCnt.Text = ConnectedSockedCount.ToString();

            while (true)
            {
                if (ReadServerSocket.Connected)
                {
                    Byte[] ReceiveByte = new Byte[1024];
                    try
                    {                        
                        int nVlaue = ReadServerSocket.Receive(ReceiveByte, ReceiveByte.Length, 0);

                        if (nVlaue > 0)
                        {
                            string ReceiveData = null;

                            ReceiveData = System.Text.Encoding.Unicode.GetString(ReceiveByte);
                            lstMsg.Items.Add(ReceiveData);
                            if (ReceiveData.IndexOf("SYN") >= 0)
                            {
                                Byte[] SendBuffer;
                                string strBuffer;
                                strBuffer = String.Format("{0}", "ACK");
                                SendBuffer = System.Text.Encoding.Unicode.GetBytes(strBuffer);
                                ReadServerSocket.Send(SendBuffer, 0, SendBuffer.Length, 0);                                
                            }
                            else if (ReceiveData.IndexOf("ACK") >= 0)
                            {
                                lstMsg.Items.Add("[Client] -> [Server] : 연결설정 (ACK=1)");
                                ConnectedSockedCount -= 1;
                            }
                            else if (ReceiveData.IndexOf("[FIN_ATK]") >= 0)
                            {
                                lstMsg.Items.Add("[Client] -> [Server] : 연결종료요청 (FIN_ACK)");
                                Byte[] SendBuffer;
                                string strBuffer;
                                strBuffer = String.Format("{0}", "[ATK]");
                                SendBuffer = System.Text.Encoding.Unicode.GetBytes(strBuffer);
                                ReadServerSocket.Send(SendBuffer, 0, SendBuffer.Length, 0);
                                lstMsg.Items.Add("[Client] <- [Server] : 연결종료확인 (ACK)");
                                
                                strBuffer = String.Format("{0}", "[FIN_ATK]");
                                SendBuffer = System.Text.Encoding.Unicode.GetBytes(strBuffer);
                                ReadServerSocket.Send(SendBuffer, 0, SendBuffer.Length, 0);                                
                                lstMsg.Items.Add("[Client] <- [Server] : 연결종료요청 (FIN_ACK)");
                            }
                            else if (ReceiveData.IndexOf("[LAST_ATK]") >= 0)
                            {
                                lstMsg.Items.Add("[Client] -> [Server] : 연결종료완료요청 (LAST_ACK)");
                                ReadServerSocket.Shutdown(SocketShutdown.Both);
                                ReadServerSocket.Close();
                                lstMsg.Items.Add("[Server] : 연결종료완료 (CLOSED)");
                                break;
                            }
                            else if (ReceiveData.IndexOf("[HC_LAST_ATK]") >= 0)
                            {
                                ReadServerSocket.Shutdown(SocketShutdown.Send);
                                lstMsg.Items.Add("[Server] : Half Close - SocketShutdown.Send Stop");
                                lstMsg.Items.Add("[Client] -> [Server] : 연결종료완료요청 (LAST_ACK)");
                                ReadServerSocket.Shutdown(SocketShutdown.Both);
                                ReadServerSocket.Close();
                                lstMsg.Items.Add("[Server] : 연결종료완료 (CLOSED)");
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!ReadServerSocket.Connected)
                        {
                            ConnectedSockedCount -= 1;
                            lblCnt.Text = ConnectedSockedCount.ToString();
                            lstMsg.Items.Add("[Server] 클라이언트가 종료했습니다.");
                            break;
                        }
                    }
                }
            }

            StopTheThread(RealConnectedSocket);

        }

        private void StopTheThread(long RealConnectedSocket)
        {
            Thread RemoveThread = (Thread)HTThreadHolder[RealConnectedSocket];

            lock (this)
            {
                HTSocketHolder.Remove(RealConnectedSocket);
                HTThreadHolder.Remove(RealConnectedSocket);
            }
            RemoveThread.Abort();
        }

        private void btnServerStart_Click(object sender, EventArgs e)
        {
            lstMsg.Items.Add("[Server] 소켓 서버 시작.");

            if (txtServerPort.Text.Trim() == "")
            {
                MessageBox.Show("사용할 서버 포트 번호를 입력하세요!!");
                txtServerPort.Focus();
                return;
            }

            PORT = Int32.Parse(txtServerPort.Text.Trim());
            ServerTCPListner = new TcpListener(PORT);
            ServerTCPListner.Start();

            Thread TCPServerThread = new Thread(new ThreadStart(WaitingFromClientConnect));

            HTThreadHolder.Add(ConnectedSockedCount, TCPServerThread);

            TCPServerThread.Start();

            this.btnServerStart.Enabled = false;
            this.btnServerSend.Enabled = true;
        }

        private void btnServerEnd_Click(object sender, EventArgs e)
        {
            try
            {
                lstMsg.Items.Add("[Server] 소켓 서버 종료.");

                if (ServerTCPListner != null)
                {
                    ServerTCPListner.Stop();
                }

                MessageBox.Show("서버가 종료되었습니다.!!");

                foreach (Socket SocketStart in HTSocketHolder.Values)
                {
                    if (SocketStart.Connected)
                    {
                        SocketStart.Close();
                    }
                }

                foreach (Thread ThreadStart in HTThreadHolder.Values)
                {
                    if (ThreadStart.IsAlive)
                    {
                        ThreadStart.Abort();
                    }
                }
            }
            catch (Exception ex)
            {
                lstMsg.Items.Add("[Server] 소켓 서버 종료.");
                MessageBox.Show("서버가 종료되었습니다.!!");
            }

            lblCnt.Text = "0";
            Application.Exit();
        }

        private void btnServerSend_Click(object sender, EventArgs e)
        {
            //txtSendMsg.Text = "소켓 서버 전송";


            if (TCPListnerAcceptSocket.Connected)
            {
                Byte[] SendBuffer;
                string strBuffer;

                strBuffer = String.Format("{0}", txtSendMsg.Text.Trim());

                SendBuffer = System.Text.Encoding.Unicode.GetBytes(strBuffer);
                TCPListnerAcceptSocket.Send(SendBuffer, 0, SendBuffer.Length, 0);
                lstMsg.Items.Add("[Server] 클라이언트로 데이터 전송.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lstMsg.Items.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
