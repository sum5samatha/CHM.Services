using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHM.Services.Models
{
    public partial class Role : IRole
    {
        string _id = Guid.NewGuid().ToString();

        public virtual string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
    }
}