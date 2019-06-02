using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotepadShop.Models.Admin
{
    public class AddItemViewModel
    {
        public string Key { get; private set; }
        public IEnumerable<string> Categories { get; private set; }

        public AddItemViewModel(string key, IEnumerable<string> categories)
        {
            Key = key;
            Categories = categories;
        }
    }
}