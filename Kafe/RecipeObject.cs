using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafe
{
    class RecipeObject
    {
        private string name { get; set; }
        private int stock { get; set; }
        private string comment { get; set; }

        public RecipeObject(string name, int stock, string comment)
        {
            this.name = name;
            this.stock = stock;
            this.comment = comment;
        }
    }
}
