using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.HelperFunctions
{
    using System.Threading;

    using ARK.Model.XML;

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
