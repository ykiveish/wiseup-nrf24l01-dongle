using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace sensor_hub_api
{
    public class CloudRequest
    {
        public string DeviceId { get; set; }
        public int Command { get; set; }
        /// <summary>
        /// State of the request, sent or not.
        /// </summary>
        public bool RequestSent { get; set; }
        /// <summary>
        /// State of the response.
        /// </summary>
        public bool ResponseRecieved { get; set; }
        public DateTime Timestamp { get; set; }
        public WUSerialPacketResponse UARTRequest { get; set; }

        public CloudRequest()
        {

        }
    }

    public class Cloud
    {
        private bool _isWorking = true;
        private List<CloudRequest> _requestsToCloud = new List<CloudRequest>();

        public Cloud()
        {

        }

        public void AddRequest(CloudRequest request)
        {
            this._requestsToCloud.Add(request);
        }

        public void CloudHandler()
        {
            while (this._isWorking)
            {
                // TODO
                // 1. Loop over reqest (consider to build producer/consumer methodology)
                // 2. User simple HTTPRequest class to request server.

                if (this._requestsToCloud.Count > 0)
                {
                    foreach (CloudRequest item in this._requestsToCloud)
                    {
                        if (item.RequestSent != true)
                        {
                            // Send the request.
                            break;
                        }

                        if (item.ResponseRecieved != true)
                        {
                            // Check the timestamp than delete or resend.
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }
    }
}
