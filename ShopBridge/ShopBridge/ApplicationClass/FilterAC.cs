﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.ApplicationClass
{
    public class FilterAC
    {
        /// <summary>
        /// Specific field
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// Value of specific field
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Operation for the field
        /// </summary>
        public string Operator { get; set; }
    }
}
