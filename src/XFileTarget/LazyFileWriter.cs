using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;
using NLog.Common;
using NLog.Layouts;

namespace XFileTarget
{
    internal sealed class LazyFileWriter : IDisposable
    {
        private Layout layout;

        public LazyFileWriter(Layout layout)
        {
            this.layout = layout;
        }

        public void WriteBlockAsync(IList<AsyncLogEventInfo> events)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task FlushAsync()
        {
            throw new NotImplementedException();
        }
    }
}