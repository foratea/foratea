/*
This program uses Azure Function (Cloud/Local) and .Net Core 3.1
Every minute, data is fetched from given URL;
time and success/failure of the attempt is logged,
a full payload is printed out on the console.
Azure Storage is not used because the company did not gave an
existing account, but the applicant do not want to register an
account to her name.
*/

using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;

namespace foratea
{
    public static class fetchData
    {

        [FunctionName("fetchData")]

        public static void Run([TimerTrigger("0 * * * * *", RunOnStartup = false)]TimerInfo myTimer, ILogger log)
        {
            // the RunOnStartup parametter does not work properly, so
            // we should control manually not to have two executions during the first minute
            if (DateTime.Now.Second == 0)
            {
                // the URL we should connect to
                var myUrl = " https://api.publicapis.org/random?auth=null";
                HttpClient Client = new HttpClient();
                var response = Client.GetAsync(myUrl);
                // was the connection succesfull or not
                var status = response.Result.IsSuccessStatusCode;
                // the full JSON payload
                var body = response.Result.Content.ReadAsStringAsync().Result;
                // current date and time
                var dtid = DateTime.Now;
                // logging
                log.LogInformation($"{dtid} | {status}");
                // output to the console
                Console.WriteLine("The full payload: " + body);
            }
        }
    }
}
