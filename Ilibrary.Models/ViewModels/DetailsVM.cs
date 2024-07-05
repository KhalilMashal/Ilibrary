using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ilibrary.Models.ViewModels
{
    public class DetailsVM
    {
        public Comment comments { get; set; }
        public IEnumerable<Comment> allcomments { get; set; }
        public ShoppingCart shoppingcarts { get; set; }

    }
}
