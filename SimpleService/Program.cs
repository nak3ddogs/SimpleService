using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SimpleService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //netsh http add urlacl url=http://+:80/MyUri user=DOMAIN\user cmd command
            WebServiceHost serviceHost = new WebServiceHost(typeof(Service));
            ServiceEndpoint endpoint = serviceHost.AddServiceEndpoint(typeof(IService), new WebHttpBinding(), "");

            ServiceDebugBehavior stp = serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();
            stp.HttpHelpPageEnabled = false;

            try
            {
                serviceHost.Open();

                Console.WriteLine("Service is running");
                Console.WriteLine("Press enter to quit...");
                Console.ReadLine();

                serviceHost.Close();
            }
            catch (CommunicationException cex)
            {
                Console.WriteLine(cex.Message);
                serviceHost.Abort();
            }
        }
    }
}
