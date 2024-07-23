using System;
using System.Text;

namespace FWBS.OMS
{
    using System.IO;
    using System.Security.Cryptography;

    public static class Branding
    {

        private static byte[] PublicKey = new byte[] { 6, 2, 0, 0, 0, 34, 0, 0, 68, 83, 83, 49, 0, 4, 0, 0, 135, 49, 238, 234, 184, 171, 26, 190, 53, 140, 183, 197, 124, 124, 93, 94, 233, 32, 81, 247, 9, 76, 122, 148, 51, 133, 248, 118, 106, 155, 193, 68, 255, 227, 89, 88, 64, 244, 150, 22, 239, 229, 215, 180, 84, 255, 143, 121, 57, 155, 211, 12, 229, 235, 16, 15, 243, 10, 51, 26, 177, 7, 71, 135, 38, 2, 68, 247, 39, 114, 54, 225, 88, 104, 148, 209, 233, 104, 137, 181, 134, 10, 243, 155, 129, 213, 242, 217, 83, 37, 51, 227, 113, 200, 163, 211, 122, 160, 107, 120, 56, 146, 199, 182, 198, 236, 27, 249, 90, 76, 254, 36, 186, 254, 71, 180, 190, 139, 142, 38, 59, 228, 176, 34, 223, 225, 93, 138, 225, 138, 205, 225, 88, 72, 164, 74, 170, 99, 18, 183, 229, 201, 123, 124, 243, 169, 133, 214, 124, 244, 138, 106, 161, 233, 86, 202, 250, 129, 86, 254, 254, 200, 114, 252, 253, 143, 54, 116, 229, 108, 225, 217, 71, 245, 182, 199, 14, 7, 167, 0, 207, 68, 147, 52, 118, 71, 3, 33, 18, 255, 236, 186, 185, 248, 166, 220, 182, 216, 236, 252, 228, 143, 253, 122, 169, 46, 82, 112, 229, 123, 97, 5, 2, 164, 166, 24, 203, 17, 88, 150, 1, 209, 170, 42, 207, 124, 225, 15, 125, 156, 158, 111, 242, 104, 139, 87, 20, 224, 162, 254, 164, 104, 172, 181, 102, 142, 60, 204, 136, 218, 175, 178, 215, 207, 75, 246, 176, 219, 245, 0, 32, 237, 49, 207, 114, 29, 195, 90, 69, 58, 5, 109, 177, 133, 236, 88, 26, 144, 226, 149, 194, 98, 203, 193, 5, 71, 148, 10, 25, 56, 2, 58, 110, 3, 32, 230, 53, 75, 22, 190, 202, 155, 94, 122, 64, 48, 130, 150, 111, 124, 244, 33, 216, 71, 27, 220, 56, 255, 36, 251, 208, 191, 181, 50, 18, 135, 223, 126, 135, 246, 254, 7, 61, 7, 230, 144, 254, 126, 179, 213, 225, 77, 190, 142, 139, 12, 118, 183, 87, 199, 18, 206, 118, 72, 101, 74, 55, 192, 246, 144, 239, 117, 152, 122, 247, 208, 102, 8, 125, 60, 158, 141, 242, 29, 84, 81, 19, 22, 225, 148, 31, 212, 96, 156, 193, 38, 94, 53, 169, 88, 159, 94, 243, 140, 121, 217, 143, 51, 54, 86, 92, 192, 105, 18, 77, 1, 0, 0, 178, 11, 118, 240, 125, 101, 115, 2, 176, 163, 23, 237, 24, 167, 194, 85, 10, 119, 91, 218 };
        public const string APPLICATION_NAME = "3E MatterSphere";

        /// <summary>
        /// Application Name.
        /// </summary>
        private static string ApplicationName
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(Global.ApplicationKey, String.Empty, "Branding", "ApplicationName", APPLICATION_NAME);
                return Convert.ToString(reg.GetSetting());
            }
        }

        private static byte[] ApplicationNameSignature
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(Global.ApplicationKey, String.Empty, "Branding", "ApplicationNameSignature", null);
                return reg.GetSetting() as byte[];
            }
        }

        public static string GetApplicationName()
        {
            string appname = ApplicationName;
            if (Verify(appname, ApplicationNameSignature))
                return appname;
            else
                return APPLICATION_NAME;
        }

        private static byte[] GenericLogo
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(Global.ApplicationKey, String.Empty, "Branding", "Logo", null);
                return reg.GetSetting() as byte[];
            }
        }

        private static byte[] GenericLogoSignature
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(Global.ApplicationKey, String.Empty, "Branding", "LogoSignature", null);
                return reg.GetSetting() as byte[];
            }
        }

        public static System.Drawing.Image GetGenericLogo()
        {
            return ValidateLogo(GenericLogo, GenericLogoSignature);
        }

        private static byte[] AboutLogo
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(Global.ApplicationKey, String.Empty, "Branding", "LogoAbout", null);
                return reg.GetSetting() as byte[];
            }
        }
        private static byte[] AboutLogoSignature
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(Global.ApplicationKey, String.Empty, "Branding", "LogoAboutSignature", null);
                return reg.GetSetting() as byte[];
            }
        }
        public static System.Drawing.Image GetAboutLogo()
        {
            System.Drawing.Image img =  ValidateLogo(AboutLogo, AboutLogoSignature);
            if (img == null)
                return GetGenericLogo();
            else
                return img;
        }

        private static byte[] SplashLogo
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(Global.ApplicationKey, String.Empty, "Branding", "LogoSplash", null);
                return reg.GetSetting() as byte[];
            }
        }
        public static System.Drawing.Image GetSplashLogo()
        {
            System.Drawing.Image img = ValidateLogo(SplashLogo, SplashLogoSignature);
            if (img == null)
                return GetGenericLogo();
            else
                return img;
        }

        private static byte[] SplashLogoSignature
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(Global.ApplicationKey, String.Empty, "Branding", "LogoSplashSignature", null);
                return reg.GetSetting() as byte[];
            }
        }

        private static byte[] LoginLogo
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(Global.ApplicationKey, String.Empty, "Branding", "LogoLogin", null);
                return reg.GetSetting() as byte[];
            }
        }

        private static byte[] LoginLogoSignature
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(Global.ApplicationKey, String.Empty, "Branding", "LogoLoginSignature", null);
                return reg.GetSetting() as byte[];
            }
        }
        public static System.Drawing.Image GetLoginLogo()
        {
            System.Drawing.Image img = ValidateLogo(LoginLogo, LoginLogoSignature);
            if (img == null)
                return GetGenericLogo();
            else
                return img;

        }

        private static byte[] WizardLogo
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(Global.ApplicationKey, String.Empty, "Branding", "LogoWizard", null);
                return reg.GetSetting() as byte[];
            }
        }
        private static byte[] WizardLogoSignature
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(Global.ApplicationKey, String.Empty, "Branding", "LogoWizardSignature", null);
                return reg.GetSetting() as byte[];
            }
        }
        public static System.Drawing.Image GetWizardLogo()
        {
            return ValidateLogo(WizardLogo, WizardLogoSignature);
        }


        #region Verification

        private static System.Drawing.Image ValidateLogo(byte[] image, byte[] signature)
        {
            if (signature == null || signature.Length == 0)
                return null;

            if (image == null || image.Length == 0)
                return null;

            if (Verify(image, signature))
            {
                using (MemoryStream mem = new MemoryStream(image))
                {
                    return System.Drawing.Image.FromStream(mem);
                }
            }
            else
                return null;
        }

        private static bool Verify(string data, byte[] signature)
        {
            if (signature == null || signature.Length == 0)
                return false;

            if (data == null)
                return false;


            byte[] HashValue;

            using (MemoryStream mem = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(data)))
            {
                HashValue = Hash(mem);
            }

            return DSAVerifyHash(HashValue, signature);
        }

        private static bool Verify(byte[] data, byte[]  signature)
        {
            if (signature == null || signature.Length == 0)
                return false;

            if (data == null || data.Length == 0)
                return false;

            byte[] HashValue;

            using (MemoryStream mem = new MemoryStream(data))
            {
                HashValue = Hash(mem);
            }

            return DSAVerifyHash(HashValue, signature);
        }

        private static bool DSAVerifyHash(byte[] HashValue, byte[] SignedHashValue)
        {
            try
            {
                //Create a new instance of DSACryptoServiceProvider.
                DSACryptoServiceProvider DSA = new DSACryptoServiceProvider();

                //Import the key information. 
                DSA.ImportCspBlob(PublicKey);

                //Create an DSASignatureDeformatter object and pass it the 
                //DSACryptoServiceProvider to transfer the private key.
                DSASignatureDeformatter DSADeformatter = new DSASignatureDeformatter(DSA);

                //Set the hash algorithm to the passed value.
                DSADeformatter.SetHashAlgorithm("SHA1");

                //Verify signature and return the result. 
                return DSADeformatter.VerifySignature(HashValue, SignedHashValue);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }

        }

        #endregion

        #region Hashing

        private static byte[] Hash(string data)
        {
            using (MemoryStream mem = new MemoryStream(UnicodeEncoding.Unicode.GetBytes(data)))
            {
                return Hash(mem);
            }
        }

        private static byte[] Hash(System.IO.FileInfo file)
        {
            using (System.IO.FileStream fs = file.Open(System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
            {
                return Hash(fs);
            }
        }

        private static byte[] Hash(Stream strm)
        {
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                return sha1.ComputeHash(strm);
            }
        }

        #endregion
    }
}
