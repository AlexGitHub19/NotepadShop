using System.Collections.Generic;

namespace NotepadShop.Models.ItemModels
{
    public class FullItem : Item
    {
        public IEnumerable<string> AdditionalImages { get; set; }
    }
}