using System.IO;
using System.Threading.Tasks;

namespace NLogTargets
{
    internal sealed class NullFileWrapper : IFileWrapper
    {
        public static readonly NullFileWrapper Instance = new NullFileWrapper();

        private NullFileWrapper()
        {
            
        }

        public long CurrentFileSize => 0;

        public void AppendStreamDataFromBeginning(MemoryStream eventsCollectingBuffer)
        {
            eventsCollectingBuffer.Dispose();
        }

        public void CreateNewFile()
        {
        }

        public Task FlushAsync()
        {
            return Task.FromResult(true);
        }

        public void Dispose()
        {
        }
    }
}