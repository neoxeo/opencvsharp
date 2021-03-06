﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using OpenCvSharp.Utilities;

namespace OpenCvSharp.CPlusPlus.Prototype
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class CvCpp
    {
        #region NamedWindow
        /// <summary>
        /// Creates a window.
        /// </summary>
        /// <param name="winname">Name of the window in the window caption that may be used as a window identifier.</param>
        public static void NamedWindow(string winname)
        {
            NamedWindow(winname, WindowMode.None);
        }
        /// <summary>
        /// Creates a window.
        /// </summary>
        /// <param name="winname">Name of the window in the window caption that may be used as a window identifier.</param>
        /// <param name="flags">
        /// Flags of the window. Currently the only supported flag is CV WINDOW AUTOSIZE. If this is set, 
        /// the window size is automatically adjusted to fit the displayed image (see imshow ), and the user can not change the window size manually.
        /// </param>
        public static void NamedWindow(string winname, WindowMode flags)
        {
            if (string.IsNullOrEmpty(winname))
                throw new ArgumentNullException("winname");
            try
            {
                CppInvoke.highgui_namedWindow(winname, flags);
            }
            catch (BadImageFormatException ex)
            {
                throw PInvokeHelper.CreateException(ex);
            }
        }
        #endregion
        #region ImShow
        /// <summary>
        /// Displays the image in the specified window
        /// </summary>
        /// <param name="winname">Name of the window.</param>
        /// <param name="mat">Image to be shown.</param>
        public static void ImShow(string winname, Mat mat)
        {
            if (string.IsNullOrEmpty(winname))
                throw new ArgumentNullException("winname");
            if (mat == null)
                throw new ArgumentNullException("mat");
            try
            {
                CppInvoke.highgui_imshow(winname, mat.CvPtr);
            }
            catch (BadImageFormatException ex)
            {
                throw PInvokeHelper.CreateException(ex);
            }
        }
        #endregion
        #region ImRead
        /// <summary>
        /// Loads an image from a file.
        /// </summary>
        /// <param name="filename">Name of file to be loaded.</param>
        /// <returns></returns>
        public static Mat ImRead(string filename)
        {
            return ImRead(filename, LoadMode.Color);
        }
        /// <summary>
        /// Loads an image from a file.
        /// </summary>
        /// <param name="filename">Name of file to be loaded.</param>
        /// <param name="flags">Specifies color type of the loaded image</param>
        /// <returns></returns>
        public static Mat ImRead(string filename, LoadMode flags = LoadMode.Color)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");
            try
            {
                IntPtr matPtr = CppInvoke.highgui_imread(filename, flags);
                return new Mat(matPtr);
            }
            catch (BadImageFormatException ex)
            {
                throw PInvokeHelper.CreateException(ex);
            }
        }
        #endregion
        #region ImWrite
        /// <summary>
        /// Saves an image to a specified file.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="img"></param>
        /// <param name="prms"></param>
        /// <returns></returns>
        public static bool ImWrite(string filename, Mat img, int[] prms = null)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");
            if (img == null)
                throw new ArgumentNullException("img");
            if (prms == null)
                prms = new int[0];
            try
            {
                return CppInvoke.highgui_imwrite(filename, img.CvPtr, prms, prms.Length) != 0;
            }
            catch (BadImageFormatException ex)
            {
                throw PInvokeHelper.CreateException(ex);
            }
        }
        /// <summary>
        /// Saves an image to a specified file.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="img"></param>
        /// <param name="prms"></param>
        /// <returns></returns>
        public static bool ImWrite(string filename, Mat img, params ImageEncodingParam[] prms)
        {
            if (prms != null)
            {
                List<int> p = new List<int>();
                foreach (ImageEncodingParam item in prms)
                {
                    p.Add((int)item.EncodingID);
                    p.Add(item.Value);
                }
                return ImWrite(filename, img, p.ToArray());
            }
            else
            {
                return ImWrite(filename, img, (int[])null);
            }
        }
        #endregion
        #region ImDecode
        /// <summary>
        /// Reads image from the specified buffer in memory.
        /// </summary>
        /// <param name="buf">The input array of vector of bytes.</param>
        /// <param name="flags">The same flags as in imread</param>
        /// <returns></returns>
        public static Mat ImDecode(Mat buf, LoadMode flags)
        {
            if (buf == null)
                throw new ArgumentNullException("buf");
            try
            {
                IntPtr matPtr = CppInvoke.highgui_imdecode(buf.CvPtr, flags);
                return new Mat(matPtr);
            }
            catch (BadImageFormatException ex)
            {
                throw PInvokeHelper.CreateException(ex);
            }
        }
        #endregion
        #region ImEncode
        /// <summary>
        /// Compresses the image and stores it in the memory buffer
        /// </summary>
        /// <param name="ext">The file extension that defines the output format</param>
        /// <param name="img">The image to be written</param>
        /// <param name="buf"></param>
        /// <param name="prms"></param>
        public static void ImEncode(string ext, Mat img, out byte[] buf, int[] prms = null)
        {
            if (string.IsNullOrEmpty(ext))
                throw new ArgumentNullException("ext");
            if (img == null)
                throw new ArgumentNullException("img");
            if (prms == null)
                prms = new int[0];
            try
            {
                IntPtr bufMatPtr;
                CppInvoke.highgui_imencode(ext, img.CvPtr, out bufMatPtr, prms, prms.Length);
                {
                    CvMat bufMat = new CvMat(bufMatPtr, false);
                    buf = new byte[bufMat.Rows * bufMat.Cols];
                    Marshal.Copy(bufMat.Data, buf, 0, buf.Length);
                }
                CvInvoke.cvReleaseMat(ref bufMatPtr);
            }
            catch (BadImageFormatException ex)
            {
                throw PInvokeHelper.CreateException(ex);
            }
        }

        /// <summary>
        /// Compresses the image and stores it in the memory buffer
        /// </summary>
        /// <param name="ext">The file extension that defines the output format</param>
        /// <param name="img">The image to be written</param>
        /// <param name="buf"></param>
        /// <param name="prms"></param>
        public static void ImEncode(string ext, Mat img, out byte[] buf, params ImageEncodingParam[] prms)
        {
            if (prms != null)
            {
                List<int> p = new List<int>();
                foreach (ImageEncodingParam item in prms)
                {
                    p.Add((int)item.EncodingID);
                    p.Add(item.Value);
                }
                ImEncode(ext, img, out buf, p.ToArray());
            }
            else
            {
                ImEncode(ext, img, out buf, (int[])null);
            }
        }
        #endregion
        #region WaitKey
        /// <summary>
        /// Waits for a pressed key.
        /// </summary>
        /// <param name="delay">Delay in milliseconds. 0 is the special value that means ”forever”</param>
        /// <returns>Returns the code of the pressed key or -1 if no key was pressed before the specified time had elapsed.</returns>
        public static int WaitKey(int delay = 0)
        {
            try
            {
                return CppInvoke.highgui_waitKey(delay);
            }
            catch (BadImageFormatException ex)
            {
                throw PInvokeHelper.CreateException(ex);
            }
        }

        #endregion
    }
}
