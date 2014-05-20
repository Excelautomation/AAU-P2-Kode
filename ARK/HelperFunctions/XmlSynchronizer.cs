using System;
using System.Threading;
using System.Threading.Tasks;

using ARK.Model.XML;

namespace ARK.HelperFunctions
{
    public static class XmlSynchronizer
    {
        private static Task _task;

        internal static void StartXmlSynchronizerTask(CancellationToken token)
        {
            if (_task == null)
            {
                _task = Task.Factory.StartNew(
                    async () =>
                        {
                            while (true)
                            {
                                XmlParser.UpdateDataFromFtp();
                                await Task.Delay(new TimeSpan(1, 0, 0), token);
                            }
                        }, 
                    token);
            }
            else
            {
                throw new InvalidOperationException("The task has already been started");
            }
        }
    }
}