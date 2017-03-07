using System;
using System.Collections.Generic;
using NLog;
using NLog.Common;
using NLog.Layouts;
using NLog.Targets;
using NLogTargets.Properties;

namespace NLogTargets
{
    [Target("XFile")]
#pragma warning disable VSD0025 // Implements the most common configuration of naming conventions.
    public sealed class XFileTarget : Target
#pragma warning restore VSD0025 // Implements the most common configuration of naming conventions.
    {
        //InternalLogger.Trace("AsyncWrapper '{0}': Throttled timer scheduled", this.Name);

        private LazyFileWriter _fileWriter;

        public XFileTarget()
        {
            OptimizeBufferReuse = true;
        }

        [UsedImplicitly]
        public Layout Layout { get; set; }

        protected override void InitializeTarget()
        {
            base.InitializeTarget();

            _fileWriter = new LazyFileWriter(Layout, 100000, "%TEMP%\\XFileTarget\\DraftFile.txt");
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

            _fileWriter.ScheduleBlockWriting(logEvents);
        }

        protected override void Write(LogEventInfo logEvent)
        {
            throw new NotSupportedException("Sync writing way is not supported");
        }

        protected override void FlushAsync(AsyncContinuation asyncContinuation)
        {
            _fileWriter.FlushAsync().ContinueWith(t => asyncContinuation(t.Exception?.Flatten()));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_fileWriter")]
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
