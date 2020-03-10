using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace IdentityServerWeb.Models
{
    // Add profile data for application roles by adding properties to the ApplicationRole class
    public class ApplicationRole : IdentityRole<int>
    {

        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        /// <summary>
        ///排序
        /// </summary>
        public int OrderSort { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; } = DateTime.Now;

    }
}
