using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMSCodeTest.Services
{
    public class ImageUploadServices
    {
            private IWebHostEnvironment _environment;
            public ImageUploadServices(IWebHostEnvironment environment)
            {
                this._environment = environment;
            }
            public string UploadProductImage(IFormFile files, string UserId)
            {
                if (files.Length > 0)
                {
                    try
                    {
                        if (!Directory.Exists((_environment.ContentRootPath + "\\wwwroot\\" + UserId + "\\")))
                        {
                            Directory.CreateDirectory(_environment.ContentRootPath + "\\wwwroot\\" + UserId + "\\");
                        }
                        using (FileStream filestream = System.IO.File.Create(_environment.ContentRootPath + "\\wwwroot\\" + UserId + "\\" + files.FileName))
                        {
                            files.CopyTo(filestream);
                            filestream.Flush();
                            string FilePath = files.FileName;
                            return FilePath;
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.ToString();
                    }
                }
                else
                {
                    return "Unsuccessful";
                }
            }
            public bool DeleteImage(string fileName, string BusinessName)
            {
                try
                {
                    if (fileName == "default.jpeg")
                    {
                        return true;
                    }
                    string? imageToDelete = Path.Combine(_environment.ContentRootPath + "\\wwwroot\\" + BusinessName + "\\" + fileName);
                    if ((System.IO.File.Exists(imageToDelete)))
                    {
                        System.IO.File.Delete(imageToDelete);
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
