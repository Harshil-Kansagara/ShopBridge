using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.ApplicationClass
{
    public class FilterModelAC
    {
        /// <summary>
        /// Page number
        /// </summary>
        public int? PageNo { get; set; }

        /// <summary>
        /// Page Record Count
        /// </summary>
        public int? PageRecordCount { get; set; }

        [JsonIgnore]
        public List<FilterAC> Filters { get; set; }

        /// <summary>
        /// Filter Object
        /// [{"field1","operator","value"},{"field2","operator","value"}]
        /// </summary>
        public string Filter
        {
            get
            {
                return null;
            }
            set
            {
                this.Filters = JsonConvert.DeserializeObject<List<FilterAC>>(value);
            }
        }
    }
}
