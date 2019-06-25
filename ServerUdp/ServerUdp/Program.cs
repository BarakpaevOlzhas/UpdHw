using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerUdp
{
    class Program
    {
        static string ipServer = "127.0.0.1";
        static int port = 12345;

        private static FileStream fs;
        private static Socket serverUpd = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private static EndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipServer), port);
        private static EndPoint clienEndPoint = new IPEndPoint(0, 0);
        static void Main(string[] args)
        {   
            serverUpd.Bind(endPoint);            

            byte[] buf = new byte[64 * 1024]; 
            while (true)
            {
                try
                {
                    int rsize =
                      serverUpd.ReceiveFrom(buf, ref clienEndPoint);
                }
                catch (Exception){}

                Rectangle rect = new Rectangle(0, 0, 720, 480);
                Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(bmp);
                g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
                bmp.Save("screen.jpg", ImageFormat.Jpeg);
                fs = new FileStream("screen.jpg", FileMode.Open, FileAccess.Read);

                Byte[] bytes = new Byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                
                try
                {                    
                    for (int i = 0; i < fs.Length;)
                    {
                        int offset = i;
                        int size = 27 * 1024;
                        i += serverUpd.SendTo(bytes, offset, size, SocketFlags.None, clienEndPoint);
                    }
                }
                catch (Exception){}
                finally
                {                    
                    fs.Close();
                    serverUpd.Close();
                }               

            }
        }
    }
}

