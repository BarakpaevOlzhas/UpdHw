using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace clientUpd
{    
    public partial class MainWindow : Window
    {
        static Socket sock = new Socket(
        AddressFamily.InterNetwork,
        SocketType.Dgram, ProtocolType.Udp);
        static string ipSrv = "127.0.0.1";

        static int port = 12345;        

        static EndPoint remoteSrvEP = new IPEndPoint(IPAddress.Parse(ipSrv), port);

        private static Byte[] receiveBytes = new Byte[32 * 1024];

        public MainWindow()
        {
            InitializeComponent();

            string str = "Hello!";

            sock.SendTo(Encoding.UTF8.GetBytes(str), remoteSrvEP);

            ReceiveFile();
        }

        void ReceiveFile()
        {
            sock.Receive(receiveBytes);
         
            using (var ms = new System.IO.MemoryStream(receiveBytes))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; 
                image.StreamSource = ms;
                image.EndInit();
                imageS.Source = image;
            }
        }
    }
}
