using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIGRE.Controllers
{
	public static class Correo
	{
		public static SmtpClient SMTP = new SmtpClient
		{
			Host = "mail.inventaperu.com",
			Port = 25,
			Credentials = new NetworkCredential("sigre@inventaperu.com", "sigre2014")
		};

		public static bool sepuede
		{
			get
			{
				return TestConnection(SMTP.Host, SMTP.Port);
			}
		}

		public static void enviar2(string correo, string titulo, string mensaje)
		{
			if (correo != null && correo != "" && IsValidEmail(correo))
			{
				try
				{
					Thread T1 = new Thread(delegate()
					{
						using (var message = new MailMessage("sigre@inventaperu.com", correo)
						{
							Subject = titulo,
							Body = mensaje,
							IsBodyHtml = true
						})
						{
							{
								SMTP.SendAsyncCancel();
								SMTP.Send(message);
							}
						}
					});
					T1.Start();
				}
				catch (Exception e)
				{
				}
			}
		}

		public static bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public static bool TestConnection(string smtpServerAddress, int port)
		{
			Ping pingSender = new Ping();
			PingOptions options = new PingOptions();
			options.DontFragment = true;
			PingReply reply = null;
			try
			{
				reply = pingSender.Send(smtpServerAddress, 12000, Encoding.ASCII.GetBytes("SHIT"), options);
			}
			catch (System.Net.NetworkInformation.PingException)
			{
				return false;
			}
			if (reply.Status == IPStatus.Success)
			{
				IPHostEntry hostEntry = Dns.GetHostEntry(smtpServerAddress);
				IPEndPoint endPoint = new IPEndPoint(hostEntry.AddressList[0], port);
				using (Socket tcpSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
				{
					tcpSocket.Connect(endPoint);
					if (!CheckResponse(tcpSocket, 220))
						return false;
					SendData(tcpSocket, string.Format("HELO {0}\r\n", Dns.GetHostName()));
					if (!CheckResponse(tcpSocket, 250))
						return false;
					return true;
				}
			}
			else
				return false;
		}

		private static void SendData(Socket socket, string data)
		{
			byte[] dataArray = Encoding.ASCII.GetBytes(data);
			socket.Send(dataArray, 0, dataArray.Length, SocketFlags.None);
		}

		private static bool CheckResponse(Socket socket, int expectedCode)
		{
			while (socket.Available == 0)
				System.Threading.Thread.Sleep(100);
			byte[] responseArray = new byte[1024];
			socket.Receive(responseArray, 0, socket.Available, SocketFlags.None);
			string responseData = Encoding.ASCII.GetString(responseArray);
			int responseCode = Convert.ToInt32(responseData.Substring(0, 3));
			if (responseCode == expectedCode)
				return true;
			return false;
		}


	}
}
