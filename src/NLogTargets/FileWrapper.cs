using System;
using System.IO;
using System.Threading.Tasks;
using NLog.Common;
using NLogTargets.Properties;

namespace NLogTargets
{
    internal sealed class FileWrapper : IFileWrapper
    {
        private readonly string _filePath;

        private FileWrapper(string filePath)
        {
            _filePath = filePath;

            if (File.Exists(filePath))
            {
                using (var testStream = File.OpenRead(filePath))
                {
                    CurrentFileSize = testStream.Length;
                }
            }
            else
            {
                CurrentFileSize = 0;
            }
        }

        public long CurrentFileSize { get; }

        public void AppendStreamDataFromBeginning(MemoryStream eventsCollectingBuffer)
        {
            throw new NotImplementedException();
        }

        public void CreateNewFile()
        {
            throw new NotImplementedException();
        }

        public Task FlushAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [NotNull]
        public static IFileWrapper CreateForPath([NotNull] string filePath)
        {
            try
            {
                return new FileWrapper(filePath);
            }
            catch (Exception ex)
            {
                InternalLogger.Fatal(ex, "Unable to use file {0}", filePath);
            }

            return NullFileWrapper.Instance;
        }
    }

}