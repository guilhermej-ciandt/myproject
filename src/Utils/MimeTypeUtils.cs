using System;
using System.IO;
using System.Runtime.InteropServices;

namespace AG.Framework.Utils
{
    /// <summary>
    /// Utility class contains static method checkType to determine Mime Type
    /// Found at http://blogs.msdn.com/b/bwaldron/archive/2005/01/04/346547.aspx
    /// </summary>
    public class MimeTypeUtil
    {

        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static UInt32 FindMimeFromData(
             UInt32 pBC,
             [MarshalAs(UnmanagedType.LPStr)] String pwzUrl,
             [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
             UInt32 cbSize,
             [MarshalAs(UnmanagedType.LPStr)] String pwzMimeProposed,
             UInt32 dwMimeFlags,
             out UInt32 ppwzMimeOut,
             UInt32 dwReserved);

        private static string GetType(Stream fileStream)
        {
            var buffer = new byte[256];

            // grab the first 256 bytes on the file
            if (fileStream.Length >= 256)
            {
                fileStream.Read(buffer, 0, 256);
            }
            else
            {
                fileStream.Read(buffer, 0, (int)fileStream.Length);
            }

            try
            {
                UInt32 mimeType;
                FindMimeFromData(0, null, buffer, 256, null, 0, out mimeType, 0);
                var mimeTypePointer = new IntPtr(mimeType);
                return Marshal.PtrToStringUni(mimeTypePointer);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string CheckTypeByPath(string filePath)
        {
            return GetType(new FileStream(filePath, FileMode.Open));
        }

        public static string CheckTypeByStream(Stream fileStream)
        {
            return GetType(fileStream);
        }
    }
}
