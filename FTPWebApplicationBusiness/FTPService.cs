using FTPWebApplicationDataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WinSCP;

namespace FTPWebApplicationBusiness
{
    public class FTPService
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int PortNumber { get; set; }
        public string FTPRemotePath { get; set; }
        public string SNAPRemotePath { get; set; }
        public string SuccessToPath { get; set; }
        //public string SshHostKeyFingerprint { get; set; }

        public ResultViewModel Download(string DownLoadToPath, string ProviderFileFTP, string DownLoadToPathSNAP, string ProviderINVSNAP, string TestSend)
        {
            var result = new ResultViewModel
            {
                 MsgName = "FTPService"
            };

            Session MySession = new Session();

            try
            {
                SessionOptions mySessionOptions = new SessionOptions();
                //mySessionOptions.Protocol = Protocol.Ftp;
                mySessionOptions.Protocol = Protocol.Sftp;
                mySessionOptions.HostName = HostName;
                mySessionOptions.UserName = UserName;
                mySessionOptions.Password = Password;
                mySessionOptions.PortNumber = PortNumber;
             //   mySessionOptions.SshHostKeyFingerprint = SshHostKeyFingerprint;

                string Subpath = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                Subpath = Subpath.Replace(" ", "");
                Subpath = Subpath.Replace(":", "");
                Subpath = Subpath.Replace("-", "");
                string Pathfiles = DownLoadToPath + "\\" + Subpath + "\\";

                bool exists = System.IO.Directory.Exists(Pathfiles);

                if (!exists)
                    System.IO.Directory.CreateDirectory(Pathfiles);

                //string RemoteFile = DownLoadToPath;
                MySession.Open(mySessionOptions);

                var oResult = MySession.GetFiles(FTPRemotePath, Pathfiles);

                if (oResult.IsSuccess)
                {
                    //var SuccessFile = new List<string>();
                    var SuccessFile = new List<FTPServiceViewModel>();
                    var _ViewModelList = new List<ProviderFileFTPViewModel>();

                    //Keep List
                    foreach (TransferEventArgs e in oResult.Transfers)
                    {
                        //var fi = e.FileName;
                        //SuccessFile.Add(fi);
                        var fi = new FTPServiceViewModel();
                        fi.Source = e.FileName;
                        fi.Destination = e.Destination;
                        SuccessFile.Add(fi);

                        var _ViewModel = new ProviderFileFTPViewModel();
                        _ViewModel.Destination = e.Destination;
                        _ViewModelList.Add(_ViewModel);
                    }

                    //Send Provider FileFTP
                    if (SuccessFile.Count > 0)
                    {
                        var JsonString = CommonUtility.SerializeObject(_ViewModelList);

                        var _ViewModel = new ProviderFileFTPViewModel();
                        _ViewModel.Destination = Pathfiles;
                        var response = CommonUtility.SendDataPostApi<string>(ProviderFileFTP, CommonUtility.SerializeObject(_ViewModel));
                    }

                    //Save Into DB
                    if (TestSend.Trim().ToUpper() == "FALSE")
                    {
                        using (var context = new FTPDbContext())
                        {
                            foreach (var item in SuccessFile)
                            {
                                context.FTPFilesPath.Add(new FTPWebApplicationDataAccess.Models.FTPFilesPath
                                {
                                    FileName = item.Source,
                                    RelativePath = Pathfiles + item.Source
                                });

                                result.RECORD.Add(new FTPResultRecordViewModel
                                {
                                    FileName = item.Source,
                                    DownloadUrl = item.Source
                                });
                            }

                            context.SaveChanges();
                        }
                    }

                    //Remove Remote File
                    foreach (var item in SuccessFile)
                    {
                        //MySession.RemoveFiles(FTPRemotePath + item);
                        MySession.RemoveFiles(item.Source);
                    }

                    result.ResultStatus = "FTP Success";
                    result.ResultDesc = "FTP downloaded success";
                }
                else
                {
                    result.ResultStatus = "FTP Error";
                    result.ResultDesc = "FTP downloaded Unsuccess";
                }

                //INVSNAP
                string PathfilesSNAP = DownLoadToPathSNAP;
                //string RemoteFileSNAP = DownLoadToPathSNAP;
                //MySession.Open(mySessionOptions);

                var oResultSNAP = MySession.GetFiles(SNAPRemotePath, PathfilesSNAP);

                if (oResultSNAP.IsSuccess)
                {
                    var SuccessFile = new List<FTPServiceViewModel>();
                    var _ViewModelList = new List<ProviderFileFTPViewModel>();

                    foreach (TransferEventArgs e in oResultSNAP.Transfers)
                    {
                        var fi = new FTPServiceViewModel();
                        fi.Source = e.FileName;
                        fi.Destination = e.Destination;
                        SuccessFile.Add(fi);

                        var _ViewModel = new ProviderFileFTPViewModel();
                        _ViewModel.Destination = e.Destination;
                        _ViewModelList.Add(_ViewModel);
                    }

                    //Send Provider FileFTP
                    if (SuccessFile.Count > 0)
                    {
                        var JsonString = CommonUtility.SerializeObject(_ViewModelList);

                        var _ViewModel = new ProviderFileFTPViewModel();
                        _ViewModel.Destination = PathfilesSNAP;
                        var response = CommonUtility.SendDataPostApi<string>(ProviderINVSNAP, CommonUtility.SerializeObject(_ViewModel));
                    }

                    //Save Into DB
                    if (TestSend.Trim().ToUpper() == "FALSE")
                    {
                        using (var context = new FTPDbContext())
                        {
                            foreach (var item in SuccessFile)
                            {
                                context.FTPFilesPath.Add(new FTPWebApplicationDataAccess.Models.FTPFilesPath
                                {
                                    FileName = item.Source,
                                    RelativePath = Pathfiles + item.Source
                                });

                                result.RECORD.Add(new FTPResultRecordViewModel
                                {
                                    FileName = item.Source,
                                    DownloadUrl = item.Source
                                });
                            }

                            context.SaveChanges();
                        }
                    }

                    //Remove Remote File
                    foreach (var item in SuccessFile)
                    {
                        MySession.RemoveFiles(item.Source);
                    }

                    if (!string.IsNullOrEmpty(result.ResultStatus))
                        result.ResultStatus =  result.ResultStatus + " / INVSNAP Success";
                    else
                        result.ResultStatus = "INVSNAP Success";
                    if (!string.IsNullOrEmpty(result.ResultDesc))
                        result.ResultDesc = result.ResultDesc + " / INVSNAP downloaded success";
                    else
                        result.ResultDesc = "INVSNAP downloaded success";
                }
                else
                {
                    if (!string.IsNullOrEmpty(result.ResultStatus))
                        result.ResultStatus = result.ResultStatus + " / INVSNAP Error";
                    else
                        result.ResultStatus = "INVSNAP Error";
                    if (!string.IsNullOrEmpty(result.ResultDesc))
                        result.ResultDesc = result.ResultDesc + " / INVSNAP downloaded Unsuccess";
                    else
                        result.ResultDesc = "INVSNAP downloaded Unsuccess";
                }
            }
            catch (InvalidOperationException ex)
            {
                result.ResultStatus = "Error";
                result.ResultDesc = ex.Message;
            }
            catch (SessionLocalException ex)
            {
                result.ResultStatus = "Error";
                result.ResultDesc = ex.Message;
            }
            catch (SessionRemoteException ex)
            {
                result.ResultStatus = "Error";
                result.ResultDesc = ex.Message;
            }
            catch (TimeoutException ex)
            {
                result.ResultDesc = ex.Message;
            }
            catch (Exception ex)
            {
                result.ResultStatus = "Error";
                result.ResultDesc = ex.Message;
            }
            finally
            {
                result.ResultDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                MySession.Dispose();
            }

            return result;
        }

        public string DownloadFromFTP(string DownLoadToPath, string ProviderFileFTP, string DownLoadToPathSNAP, string ProviderINVSNAP, string TestSend, string billing_id)
        {
            string status = null;
            var result = new ResultViewModel
            {
                MsgName = "FTPService"
            };

            Session MySession = new Session();
            var olog = new Loging();
            try
            {
                
                SessionOptions mySessionOptions = new SessionOptions();
                mySessionOptions.Protocol = Protocol.Ftp;
                //mySessionOptions.Protocol = Protocol.Sftp;
                mySessionOptions.HostName = HostName;
                mySessionOptions.UserName = UserName;
                mySessionOptions.Password = Password;
                mySessionOptions.PortNumber = PortNumber;
                //mySessionOptions.SshHostKeyFingerprint = SshHostKeyFingerprint;

                string Subpath = DateTime.Now.ToString("yyyy-MM-dd");
                Subpath = Subpath.Replace(" ", "");
                Subpath = Subpath.Replace(":", "");
                Subpath = Subpath.Replace("-", "");
                string Pathfiles = DownLoadToPath + "\\" + Subpath + "\\";
                olog.DataLogLines("FTP", "", HostName );
                olog.DataLogLines("FTP", "", UserName);
                olog.DataLogLines("FTP", "", Password);
                olog.DataLogLines("FTP", "", PortNumber.ToString());
                olog.DataLogLines("FTP", "", Pathfiles);
                olog.DataLogLines("FTP", "", FTPRemotePath);

                bool exists = System.IO.Directory.Exists(Pathfiles);

                if (!exists)
                    System.IO.Directory.CreateDirectory(Pathfiles);

                //string RemoteFile = DownLoadToPath;
                MySession.Open(mySessionOptions);
              

                var FTPRemotePathName = FTPRemotePath + billing_id + ".pdf";

                olog.DataLogLines("FTP", "", FTPRemotePathName);

                var oResult = MySession.GetFiles(FTPRemotePathName, Pathfiles);

               
                if (oResult.IsSuccess)
                {
                    
                    

                    olog.DataLogLines("FTP", "", "IsSuccess");
                    string targetPath = SuccessToPath;
                    olog.DataLogLines("FTP", "", "targetPath = " + targetPath);
                    string sourceFile = Path.Combine(FTPRemotePath, billing_id + ".pdf");
                    olog.DataLogLines("FTP", "", "sourceFile = " + sourceFile);

                    MySession.MoveFile(sourceFile, targetPath);

                    //string destFile = Path.Combine(targetPath, billing_id + ".pdf");
                    //olog.DataLogLines("FTP", "", "destFile = " + destFile);
                    //System.IO.File.Copy(sourceFile, destFile, true);
                    //olog.DataLogLines("FTP", "", "FTPRemotePath = " + FTPRemotePath);
                    //System.IO.File.Delete(FTPRemotePathName);
                    //olog.DataLogLines("FTP", "", "FTPRemotePathName = " + FTPRemotePathName);

                    //string targetPath = SuccessToPath;
                    //string sourceFile = Path.Combine(FTPRemotePath, billing_id);
                    //string destFile = Path.Combine(targetPath, billing_id);
                    //File.Copy(sourceFile, destFile, true);
                    //File.Delete(FTPRemotePathName);



                    ////var SuccessFile = new List<string>();
                    //var SuccessFile = new List<FTPServiceViewModel>();
                    //var _ViewModelList = new List<ProviderFileFTPViewModel>();

                    ////Keep List
                    //foreach (TransferEventArgs e in oResult.Transfers)
                    //{
                    //    //var fi = e.FileName;
                    //    //SuccessFile.Add(fi);
                    //    var fi = new FTPServiceViewModel();
                    //    fi.Source = e.FileName;
                    //    fi.Destination = e.Destination;
                    //    SuccessFile.Add(fi);

                    //    var _ViewModel = new ProviderFileFTPViewModel();
                    //    _ViewModel.Destination = e.Destination;
                    //    _ViewModelList.Add(_ViewModel);
                    //}

                    ////Send Provider FileFTP
                    //if (SuccessFile.Count > 0)
                    //{
                    //    var JsonString = CommonUtility.SerializeObject(_ViewModelList);

                    //    var _ViewModel = new ProviderFileFTPViewModel();
                    //    _ViewModel.Destination = Pathfiles;
                    //    var response = CommonUtility.SendDataPostApi<string>(ProviderFileFTP, CommonUtility.SerializeObject(_ViewModel));
                    //}

                    ////Save Into DB
                    //if (TestSend.Trim().ToUpper() == "FALSE")
                    //{
                    //    using (var context = new FTPDbContext())
                    //    {
                    //        foreach (var item in SuccessFile)
                    //        {
                    //            context.FTPFilesPath.Add(new FTPWebApplicationDataAccess.Models.FTPFilesPath
                    //            {
                    //                FileName = item.Source,
                    //                RelativePath = Pathfiles + item.Source
                    //            });

                    //            result.RECORD.Add(new FTPResultRecordViewModel
                    //            {
                    //                FileName = item.Source,
                    //                DownloadUrl = item.Source
                    //            });
                    //        }

                    //        context.SaveChanges();
                    //    }
                    //}

                    ////Remove Remote File
                    //foreach (var item in SuccessFile)
                    //{
                    //    //MySession.RemoveFiles(FTPRemotePath + item);
                    //    MySession.RemoveFiles(item.Source);
                    //}

                    status = Pathfiles+billing_id+".pdf";
                    olog.DataLogLines("FTP", "", "part = " + status);
                    result.ResultStatus = "FTP Success";
                    result.ResultDesc = "FTP downloaded success";
                }
                else
                {
                    result.ResultStatus = "FTP Error";
                    result.ResultDesc = "FTP downloaded Unsuccess " + FTPRemotePath;
                }

            }
            catch (InvalidOperationException ex)
            {
                result.ResultStatus = "Error";
                result.ResultDesc = ex.Message;
                olog.DataLog("FTP", "", ex.Message);
            }
            catch (SessionLocalException ex)
            {
                result.ResultStatus = "Error";
                result.ResultDesc = ex.Message;
                olog.DataLog("FTP", "", ex.Message);

            }
            catch (SessionRemoteException ex)
            {
                result.ResultStatus = "Error";
                result.ResultDesc = ex.Message;
                olog.DataLog("FTP", "", ex.Message);

            }
            catch (TimeoutException ex)
            {
                result.ResultDesc = ex.Message;
                olog.DataLog("FTP", "", ex.Message);

            }
            catch (Exception ex)
            {
                result.ResultStatus = "Error";
                result.ResultDesc = ex.Message;
                olog.DataLog("FTP", "", ex.Message);

            }
            finally
            {
                result.ResultDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                MySession.Dispose();
            }

            return status;
        }

    }
}
