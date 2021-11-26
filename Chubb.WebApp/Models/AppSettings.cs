using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chubb.WebApp.Models
{
    public class AppSettings
    {
        public StorageConfiguration StorageConfiguration { get; set; }        
    }

    public class StorageConfiguration
    {
        public string UrlWebApi { get; set; }        
    }
}
