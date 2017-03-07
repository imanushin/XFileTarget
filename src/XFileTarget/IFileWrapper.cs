using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace XFileTarget
{
    internal interface IFileWrapper : IDisposable
    {
        long CurrentFileSize { get; }

        void AppendStreamDataFromBeginning([NotNull] MemoryStream eventsCollectingBuffer);

        void CreateNewFile();

        [NotNull]
        Task FlushAsync();
    }
}