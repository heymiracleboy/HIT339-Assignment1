using Ass1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ass1.ViewModel
{
    public class UserPageViewModel
    {
        //UserPage viewmodel containing both the item and sales model
        public IEnumerable<Item> Item { get; set; }
        public IEnumerable<Sales> Sale { get; set; }

    }



}
