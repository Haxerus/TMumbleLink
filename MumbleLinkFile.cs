using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace TMumbleLink
{
    /*
     * Wrapper class for a Memory Mapped File shared by TMumbleLink and Mumble 
     */
    public class MumbleLinkFile : IDisposable
    {

        private const string FILE_NAME = "MumbleLink";
        private MemoryMappedFile file;
        private bool disposed = false;

        public MumbleLinkFile()
        {
            this.file = MemoryMappedFile.CreateOrOpen(FILE_NAME, Marshal.SizeOf(typeof(LinkedMem)), MemoryMappedFileAccess.ReadWrite);
        }

        public void Write(LinkedMem lm)
        {
            if (disposed)
                throw new ObjectDisposedException(GetType().FullName);

            using (var accessor = file.CreateViewAccessor())
            {
                byte[] bytes = getBytes(lm);
                accessor.WriteArray<byte>(0, bytes, 0, bytes.Length);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    file.Dispose();
                    file = null;
                }

                disposed = true;
            }
        }

        private byte[] getBytes(LinkedMem lm)
        {
            int size = Marshal.SizeOf(lm);
            byte[] buf = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(lm, ptr, true);
            Marshal.Copy(ptr, buf, 0, size);
            Marshal.FreeHGlobal(ptr);

            return buf;
        }
    }
}
