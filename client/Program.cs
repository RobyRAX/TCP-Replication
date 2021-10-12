using System;  
using System.Net.Sockets;  
using System.IO;  
using StringLibrary;
using System.Threading;
using System.Runtime.Serialization;  
using System.Runtime.Serialization.Formatters.Binary; 
namespace Client
{
	class Reader
		{
			public void ReadMessage(object obj)
			{
				TcpClient tcpClient = (TcpClient)obj;  
				StreamReader streamReader = new StreamReader(tcpClient.GetStream());  
	
				while (true)  
				{  
					try  
					{ 
						//read incoming message 
						string message = streamReader.ReadLine();  
						Console.WriteLine(message);  
					}  
					catch (Exception e)  
					{  
						Console.WriteLine(e.Message);  
						break;  
					}  
				}             
			}
		}
	class Program
	{
		static void Main(string[] args)
		{
			Reader reader = new Reader();
			try
			{
				TcpClient tcpClient = new TcpClient("127.0.0.1", 5000);

				Thread thread = new Thread(reader.ReadMessage);
				thread.Start(tcpClient);
				NetworkStream networkStream = tcpClient.GetStream();
				Player player = new Player();
				while (true)
				{
					if(tcpClient.Connected)
					{
						Console.WriteLine("Welcome Player");
						BinaryFormatter binaryFormatter = new BinaryFormatter();					
						string input;
						//reply here
						Console.WriteLine("What's your name?");
						input = Console.ReadLine();
						player.name = input;
						Console.WriteLine("What's your class?");
						input = Console.ReadLine();
						player.className = input;
						//confirmation
						binaryFormatter.Serialize(networkStream, player);
						
						Console.ReadLine();
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}