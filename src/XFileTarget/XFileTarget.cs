using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Common;
using NLog.Layouts;
using NLog.Targets;

namespace XFileTarget
{
    [Target("XFile")]
    public sealed class XFileTarget : Target
    {
        //InternalLogger.Trace("AsyncWrapper '{0}': Throttled timer scheduled", this.Name);

        private LazyFileWriter _fileWriter;

        public XFileTarget()
        {
            OptimizeBufferReuse = true;
        }

        public Layout Layout { get; set; }

        protected override void InitializeTarget()
        {
            base.InitializeTarget();

            _fileWriter = new LazyFileWriter(Layout);
        }

        protected override void Write(AsyncLogEventInfo logEvent)
        {
            Write(new[] {logEvent});
        }

        protected override void Write(IList<AsyncLogEventInfo> logEvents)
        {
            foreach (var logEvent in logEvents.Enumerate())
            {
                var eventInfo = logEvent.LogEvent;

                MergeEventProperties(eventInfo);
                PrecalculateVolatileLayouts(eventInfo);
            }

            _fileWriter.WriteBlockAsync(logEvents);
        }

        protected override void Write(LogEventInfo logEvent)
        {
            throw new NotSupportedException("Sync writing way is not supported");
        }

        protected override void FlushAsync(AsyncContinuation asyncContinuation)
        {
            _fileWriter.FlushAsync().ContinueWith(t => asyncContinuation(t.Exception?.Flatten()));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _fileWriter?.Dispose();
                _fileWriter = null;
            }

            base.Dispose(disposing);
        }

        protected override void CloseTarget()
        {
            _fileWriter?.Dispose();

            _fileWriter = null;
        }
    }
}
