using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using ThoughtWorks.QRCode.Codec.Util;
using System.Drawing;

namespace BarcodeLib
{
    public class QRCodeLib
    {
       /// <summary>
       /// 
       /// </summary>
       /// <param name="text"></param>
       /// <param name="version">1 to 8</param>
       /// <param name="size">1 to 5 is better</param>
        /// <param name="encoding">Byte,AlphaNumeric,Numeric</param>
        /// <param name="errorCorrection">L,M,Q,H</param>
       /// <returns></returns>
        public static Image GetQRCode(string text, int version = 3, int size = 2,string encoding="Byte",string errorCorrection="M")
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = GetEncodeMode(encoding);
            qrCodeEncoder.QRCodeVersion = version;
            qrCodeEncoder.QRCodeScale = size;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            System.Drawing.Image QrCodeImage = null;
            String data = text;
            QrCodeImage = qrCodeEncoder.Encode(data);
            return QrCodeImage;
            //byte[] QrCodeInBytes = null;
            //QrCodeInBytes = ImageToByte(QrCodeImage);
        }
        /// <summary>
        /// Error Correct To
        /// </summary>
        /// <param name="errorCorrect">L,M,Q,H</param>
        /// <returns></returns>
        private static QRCodeEncoder.ERROR_CORRECTION ErrorCorrectTo(string errorCorrect)
        {
            if (errorCorrect == "L")
                return QRCodeEncoder.ERROR_CORRECTION.L;
            else if (errorCorrect == "M")
                return QRCodeEncoder.ERROR_CORRECTION.M;
            else if (errorCorrect == "Q")
                return QRCodeEncoder.ERROR_CORRECTION.Q;
            else if (errorCorrect == "H")
                return QRCodeEncoder.ERROR_CORRECTION.H;
            return QRCodeEncoder.ERROR_CORRECTION.M;
        }
        /// <summary>
        /// Get Encode Mode
        /// </summary>
        /// <param name="encoding">Byte,AlphaNumeric,Numeric</param>
        /// <returns></returns>
        private static QRCodeEncoder.ENCODE_MODE GetEncodeMode(string encoding)
        {
            if (encoding == "Byte")
            {
                return QRCodeEncoder.ENCODE_MODE.BYTE;
            }
            else if (encoding == "AlphaNumeric")
            {
                return QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
            }
            else if (encoding == "Numeric")
            {
                return QRCodeEncoder.ENCODE_MODE.NUMERIC;
            }
            return QRCodeEncoder.ENCODE_MODE.BYTE;
        }
    }
}
