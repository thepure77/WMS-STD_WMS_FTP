using FTPWebApplicationBusiness;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WinSCP;

namespace FTPWebApplication
{
    public class FTPLibrary
    {
        public ResultViewModel Download()
        {
            try
            {
                var HostName = ConfigurationManager.AppSettings.Get("FTPHostName");
                var UserName = ConfigurationManager.AppSettings.Get("FTPUserName");
                var Password = ConfigurationManager.AppSettings.Get("FTPPassword");
                var PortNumber = ConfigurationManager.AppSettings.Get("FTPPortNumber");
                var SshHostKeyFingerprint = ConfigurationManager.AppSettings.Get("FTPKey");
                var FTPRootPath = ConfigurationManager.AppSettings.Get("FTPRootStorePath");
                var FTPRemotePath = ConfigurationManager.AppSettings.Get("FTPRemotePath");
                var ProviderFileFTP = ConfigurationManager.AppSettings.Get("ProviderFileFTP");
                var SNAPRootPath = ConfigurationManager.AppSettings.Get("SNAPRootStorePath");
                var SNAPRemotePath = ConfigurationManager.AppSettings.Get("SNAPRemotePath");
                var ProviderINVSNAP = ConfigurationManager.AppSettings.Get("ProviderINVSNAP");
                var TestSend = ConfigurationManager.AppSettings.Get("TestSend");

                var service = new FTPWebApplicationBusiness.FTPService();

                service.HostName = HostName;
                service.UserName = UserName;
                service.Password = Password;
                service.PortNumber = string.IsNullOrEmpty(PortNumber) ? 22 : int.Parse(PortNumber);
                //service.SshHostKeyFingerprint = SshHostKeyFingerprint;
                service.FTPRemotePath = FTPRemotePath;
                service.SNAPRemotePath = SNAPRemotePath;

                //string PathfilesFTP = FTPRootPath;
                //string PathfilesSNAP = SNAPRootPath;
                var result = service.Download(FTPRootPath, ProviderFileFTP, SNAPRootPath, ProviderINVSNAP, TestSend);

                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string DownloadFTP(string billing_id)
        {
            try
            {
                var HostName = ConfigurationManager.AppSettings.Get("FTPHostName");
                var UserName = ConfigurationManager.AppSettings.Get("FTPUserName");
                var Password = ConfigurationManager.AppSettings.Get("FTPPassword");
                var PortNumber = ConfigurationManager.AppSettings.Get("FTPPortNumber");
                //var SshHostKeyFingerprint = ConfigurationManager.AppSettings.Get("FTPKey");
                var FTPRemotePath = ConfigurationManager.AppSettings.Get("FTPRemotePath");
                var ProviderFileFTP = ConfigurationManager.AppSettings.Get("ProviderFileFTP");
                var SNAPRemotePath = ConfigurationManager.AppSettings.Get("SNAPRemotePath");
                var ProviderINVSNAP = ConfigurationManager.AppSettings.Get("ProviderINVSNAP");
                var TestSend = ConfigurationManager.AppSettings.Get("TestSend");
                ////////////////////////////////// For Download ///////////////////////////////////
                var DownLoadToPath = ConfigurationManager.AppSettings.Get("DownLoadToPath");
                var DownLoadToPathSNAP = ConfigurationManager.AppSettings.Get("DownLoadToPathSNAP");
                ///////////////////////////////////////////////////////////////////////////////////
                var SuccessToPath = ConfigurationManager.AppSettings.Get("SuccessToPath");


                //var FTPRootPath = ConfigurationManager.AppSettings.Get("FTPRootStorePath");
                //var SNAPRootPath = ConfigurationManager.AppSettings.Get("SNAPRootStorePath");

                var service = new FTPWebApplicationBusiness.FTPService();

                service.HostName = HostName;
                service.UserName = UserName;
                service.Password = Password;
                service.PortNumber = string.IsNullOrEmpty(PortNumber) ? 22 : int.Parse(PortNumber);
                //service.SshHostKeyFingerprint = SshHostKeyFingerprint;
                service.FTPRemotePath = FTPRemotePath;
                service.SNAPRemotePath = SNAPRemotePath;
                service.SuccessToPath = SuccessToPath;

                //string PathfilesFTP = FTPRootPath;
                //string PathfilesSNAP = SNAPRootPath;
                var result = service.DownloadFromFTP(DownLoadToPath, ProviderFileFTP, DownLoadToPathSNAP, ProviderINVSNAP, TestSend, billing_id);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}