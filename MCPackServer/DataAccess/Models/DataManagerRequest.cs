using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class DataManagerRequest
    {
        public DataManagerRequest()
        {
            Table = string.Empty;
            Take = 10;
            Skip = 0;
            Where = new();
            Select = new();
        }
        public List<KeyValuePair<string, object>> Where { get; set; }
        public List<string> Select { get; set; }
        public string Table { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
    }
}
