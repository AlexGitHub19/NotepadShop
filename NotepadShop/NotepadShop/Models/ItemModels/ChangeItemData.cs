using System.Collections.Generic;

namespace NotepadShop.Models.ItemModels
{
    public class ChangeItemData
    {
        public string Code { get; set; }
        public string NewPrice { get; set; }
        public string NewCategory { get; set; }
        public string NewRuName { get; set; }
        public string NewUkName { get; set; }
        public string NewEnName { get; set; }
        public bool IsMainImageChanged { get; set; }
        public bool AreAdditionalImagesAdded { get; set; }
        public IEnumerable<string> AdditionalImagesToDeleteNames { get; set; }
    }
}