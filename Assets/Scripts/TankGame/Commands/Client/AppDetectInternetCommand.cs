using SHLibrary.StateMachine;
using SHLibrary.Utils;
using System;
using System.IO;
using System.Net;

namespace Commands.Client
{
    public class AppDetectInternetCommand : ScheduledCommandBase<AppController>
    {
        private const float internetCheckPeriod = 1f;
        
        public event Action Disconected;

        public override void Start()
        {
            base.Start();
            //TODO use corutine - when internet off it`s detecting blocking game
            //ScheduledUpdate(internetCheckPeriod, true);
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();
            if (!IsHaveInternet())
            {
                Disconected.SafeRaise();
                Terminate();
            }
        }

        private bool IsHaveInternet()
        {
            var resource = "http://google.com";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
            try
            {
                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                {
                    bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                    if (isSuccess)
                    {
                        var testString = string.Empty;
                        using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                        {
                            char[] cs = new char[2];
                            reader.Read(cs, 0, cs.Length);
                            foreach (char ch in cs)
                            {
                                testString += ch;
                            }
                        }
                        return testString.Length > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}