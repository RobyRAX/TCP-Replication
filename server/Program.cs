using System;  
using System.Collections.Generic;  
using System.Net;  
using System.Net.Sockets;  
using System.IO;  
using StringLibrary;
using System.Threading; 
using System.Runtime.Serialization.Formatters.Binary;
namespace Server  
{
    class HandleClient
    {
        Program program = new Program();
        public void BroadCast(string msg)  
        {  
            foreach (TcpClient client in Program.tcpClientsList)  
            {   
                //broadcast to all client except sender
                StreamWriter sWriter = new StreamWriter(client.GetStream());  
                sWriter.WriteLine(msg);  
                sWriter.Flush();    
            }  
        }

        public void ClientListener(object obj)  
        {  
            TcpClient tcpClient = (TcpClient)obj;  
            //Player player = new Player();
            while (true)  
            {
                
                try
                {
                    Player player = new Player();
                    BinaryFormatter binaryFormatter = new BinaryFormatter(); 
                    player = (Player) binaryFormatter.Deserialize(tcpClient.GetStream());
                    BroadCast("Welcome " + player.name);
                    break;       
                }
                catch (Exception e)  
                {
                    Player player = new Player();
                    Console.WriteLine(e.Message);  
                    program.removeClient(tcpClient);
                    BroadCast(player.name + " has left the server");
                    break;  
                }  
            }
        }
    }  
    class Program  
    {  
        public static TcpListener tcpListener;  
        public static List<TcpClient> tcpClientsList = new List<TcpClient>();  

        static void Main(string[] args)  
        {
            HandleClient handleClient = new HandleClient();

            //start process
            tcpListener = new TcpListener(IPAddress.Any, 5000);  
            tcpListener.Start();  
            Console.WriteLine("Server created");
            while (true)  
            {
                //add clients to list
                TcpClient tcpClient = tcpListener.AcceptTcpClient();  
                tcpClientsList.Add(tcpClient);

                //start listener
                Thread startListen = new Thread(() => handleClient.ClientListener(tcpClient));
                startListen.Start();
            }          
        }

        public void removeClient(TcpClient client)
        {
            tcpClientsList.Remove(client);
        }
    }  
} 