using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace Utility
{
    public class CheckLicense
    {
        public bool CheckMac(string mac)
        {
            var url = "https://stackapps.000webhostapp.com/MAC.php?Auth=" + mac + "";
            var request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                var response = request.GetResponse();
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        var reader = new StreamReader(responseStream, Encoding.UTF8);
                        var r = reader.ReadToEnd();
                        if (r == "ACTIVATED")
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (WebException ex)
            {
                var errorResponse = ex.Response;
                using (var responseStream = errorResponse.GetResponseStream())
                {
                    if (responseStream == null) throw;
                    var reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    reader.ReadToEnd();
                }
                throw;
            }
            return false;

        }

        public bool mac()
        {
           
            var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces() where nic.OperationalStatus == OperationalStatus.Up select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
            var mac = CheckMac(macAddr);
            if (mac)
            {
                return true;
            }
            return false;

        }

    }
}
