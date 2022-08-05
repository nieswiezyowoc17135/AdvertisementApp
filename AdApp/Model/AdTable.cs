using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdApp.Model
{
    //model w pseudo bazie
    public class AdTable : TableEntity
    {
        public AdTable ()
        { }
        public string Title { get; set; }
        public string Ad { get; set; }
        public string Type { get; set; }

    }
}
