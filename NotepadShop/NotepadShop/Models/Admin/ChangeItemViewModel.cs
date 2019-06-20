using System.Collections.Generic;

namespace NotepadShop.Models.Admin
{
    public class ChangeItemViewModel
    {
        public string Key { get; set; }
        public string Code { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
        public string RuName { get; set; }
        public string UkrName { get; set; }
        public string EngName { get; set; }
        public string MainIImageName { get; set; }
        public IEnumerable<string> AdditionalImages { get; set; }

        public IEnumerable<string> AllCategories { get; private set; }

        public ChangeItemViewModel(string key, string code, string price, string category, string ruName, string ukrName, 
            string engName, string mainIImageName, IEnumerable<string> allCategories, IEnumerable<string> images)
        {
            Key = key;
            Code = code;
            Price = price;
            Category = category;
            RuName = ruName;
            UkrName = ukrName;
            EngName = engName;
            MainIImageName = mainIImageName;
            AllCategories = allCategories;
            AdditionalImages = images;
        }


    }
}