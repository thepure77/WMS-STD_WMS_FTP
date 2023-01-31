﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Configuration;

namespace FTPWebApplicationBusiness
{
    public class Loging
    {
        public String DataLog(string floder, string message, string data)
        {
            try
            {
                var path = Directory.GetCurrentDirectory();
                path += "\\" + floder.ToString() + "\\";

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                Task.Run(() =>
                {
                    System.IO.File.WriteAllText(path + DateTime.Now.ToString("yyyy-MM-dd-HHmm ") + message + ".txt", data);
                });
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Success";
        }

        public String DataLogLines(string floder, string message, string data)
        {
            try
            {
                var path = ConfigurationManager.AppSettings.Get("DownLoadToPath"); ;
                path += "\\" + floder.ToString() + "\\";

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                //Task.Run(() =>
                //{
                    System.IO.File.AppendAllText(path + DateTime.Now.ToString("yyyy-MM-dd ") + message + ".txt", data + Environment.NewLine);
                //});
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Success";
        }
    }

}
