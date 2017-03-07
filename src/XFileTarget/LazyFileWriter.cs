using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.IO;
using NLog;
using NLog.Common;
using NLog.Layouts;

namespace XFileTarget
{
    internal sealed class LazyFileWriter : IDisposable
    {
        private readonly Layout _layout;
        private readonly int _maxFileSize;

        private readonly object _eventsCollectingLock = new object();
        private readonly RecyclableMemoryStreamManager _streamManager = new RecyclableMemoryStreamManager();
        private readonly MemoryStream _eventsCollectingBuffer;
        private readonly StreamWriter _eventsCollectingWriter;
        private readonly IFileWrapper _fileWrapper;

        public LazyFileWriter(Layout layout, int maxFileSize, string filePath)
        {
            _layout = layout;
            _maxFileSize = maxFileSize;
            _eventsCollectingWriter = new StreamWriter(_eventsCollectingBuffer)
            {
                AutoFlush = true
            };

            _fileWrapper = FileWrapper.CreateForPath(filePath);
            _eventsCollectingBuffer = _streamManager.GetStream();
        }

        public void WriteBlockAsync(IList<AsyncLogEventInfo> events)
        {
            lock (_eventsCollectingLock)
            {
                foreach (var eventInfo in events.Enumerate())
                {
                    var fileSize = _fileWrapper.CurrentFileSize;

                    var lineForFile = _layout.Render(eventInfo.LogEvent);
                    
                    _eventsCollectingWriter.Write(lineForFile);

                    var predictedFileSize = _eventsCollectingBuffer.Length + fileSize;

                    if (predictedFileSize >= _maxFileSize)
                    {
                        FlushEvents();
                        FlushAndCreateNewFile();
                    }
                }
            }

            FlushEvents();
        }

        private void FlushEvents()
        {
            lock (_eventsCollectingLock)
            {
                _fileWrapper.AppendStreamDataFromBeginning(_eventsCollectingBuffer);

                _eventsCollectingBuffer.Seek(0, SeekOrigin.Begin);
                _eventsCollectingBuffer.SetLength(0);
            }
        }

        private void FlushAndCreateNewFile()
        {
            lock (_eventsCollectingLock)
            {
                _fileWrapper.CreateNewFile();
            }
        }

        public void Dispose()
        {
            _fileWrapper.Dispose();
            throw new NotImplementedException();
        }

        public async Task FlushAsync()
        {
            await _fileWrapper.FlushAsync();
        }
    }
}