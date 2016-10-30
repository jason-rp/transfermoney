namespace TransferMoneyConsole.Test
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class Program
    {
        static void Main(string[] args)
        {
            var t1 = Task.Run(CallRequest(1, 2, 30));
            var t2 = Task.Run(CallRequest(2, 1, 20));

            Task.WaitAll(t1,t2);
            Console.ReadLine();
        }


        public static Action CallRequest(int fromId,int toId,decimal balance) {
            var action = new Action(() =>
            {
                var request = (HttpWebRequest)WebRequest.Create(string.Format("http://localhost:54918/api/Account/Transfer?fromId={0}&toId={1}&balance={2}",fromId,toId,balance));
                request.Method = "POST";
                request.ContentLength = 0;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string returnString = response.StatusCode.ToString();
                Console.WriteLine(returnString);
            }
            );
            return action;
        }
    }
}
