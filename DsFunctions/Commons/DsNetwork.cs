using System.Net;
using System.Net.Sockets;

namespace DsFunctions.Commons
{
    public class DsNetwork
    {
        /// <summary>
        /// 첫번째 아이피를 반환함. (1로 끝나는 아이피는 반환하지 않음 = 127.0.0.1)
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && !ip.ToString().EndsWith("1"))
                {
                    return ip.ToString();
                }
            }

            return "";
        }

        /// <summary>
        /// ,를 구분자로 하여 모든 아이피를 반환하는 함수
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddresses()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string strIps = string.Empty;
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && !ip.ToString().EndsWith("1"))
                {
                    strIps += ip.ToString() + ",";
                }
            }
            if (strIps.Length > 0)
            {
                strIps = strIps.Substring(0, strIps.Length - 1);
            }
            return strIps;
        }
    }
}
