using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using CustomDomain.Classes;

namespace CustomDomain.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public void Domain()
        {
            string ipAddress = "*";
            string subDomainName = "loc";
            string hostHeader = string.Empty;
            string hostHeader2 = "www.";
            
            using (ServerManager mgr = new ServerManager())
            {
                try
                {
                    Site site = mgr.Sites["siteName"];
                    if (site != null)
                    {
                        return; // Site bestaat al
                    }

                    string currentSiteName = System.Web.Hosting.HostingEnvironment.SiteName;
                    Site currentSite = mgr.Sites.SingleOrDefault(s => s.Name == currentSiteName);
                    
                    Binding bb = currentSite.Bindings.FirstOrDefault(b => b.BindingInformation.Split(":".ToCharArray())[2] != "localhost");
                    string BindingInformation = bb.BindingInformation;
                    string currentSiteTcpPort = BindingInformation.Split(":".ToCharArray())[1];
                    
                    hostHeader = subDomainName + "." + BindingInformation.Split(":".ToCharArray())[2];
                    hostHeader2 += subDomainName + "." + BindingInformation.Split(":".ToCharArray())[2];

                    string bind = ipAddress + ":" + currentSiteTcpPort + ":" + hostHeader;
                    Binding binding = currentSite.Bindings.CreateElement();
                    binding.Protocol = "http";
                    binding.BindingInformation = bind;
                    currentSite.Bindings.Add(binding);

                    bind = ipAddress + ":" + currentSiteTcpPort + ":" + hostHeader2;
                    binding = currentSite.Bindings.CreateElement();
                    binding.Protocol = "http";
                    binding.BindingInformation = bind;
                    currentSite.Bindings.Add(binding);
                    mgr.CommitChanges();

                    // add url and ip to hosts file
                    System.IO.File.AppendAllText(@"c:\windows\system32\drivers\etc\hosts", Environment.NewLine + "127.0.0.1\t\t" + hostHeader +"\n127.0.0.1\t\t" + hostHeader2);

                    // add url to access control list
                    AccessControlList.AddUrl("http://" + hostHeader + ":" + currentSiteTcpPort + "/");
                    AccessControlList.AddUrl("http://" + hostHeader2 + ":" + currentSiteTcpPort + "/");
                }
                catch (Exception exc)
                {

                }
            }
        }
    }
}