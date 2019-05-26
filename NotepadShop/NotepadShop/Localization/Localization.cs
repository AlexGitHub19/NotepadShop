using System.Resources;

namespace NotepadShop.Localization
{
    public static class Localization
    {
        public static string Russian = "ru";
        public static string Ukrainian = "uk";
        public static string English = "en";

        private static ResourceManager ResourceManagerRu { get; set; } =
            new ResourceManager("NotepadShop.Localization.Resources.LocalizationRu", typeof(Localization).Assembly);

        private static ResourceManager ResourceManagerUk { get; set; } =
            new ResourceManager("NotepadShop.Localization.Resources.LocalizationUk", typeof(Localization).Assembly);

        private static ResourceManager ResourceManagerEn { get; set; } =
            new ResourceManager("NotepadShop.Localization.Resources.LocalizationEn", typeof(Localization).Assembly);

        public static ResourceManager GetResourceManager(string language)
        {
            ResourceManager result;
            switch (language)
            {
                case "uk":
                    result = ResourceManagerUk;
                    break;
                case "en":
                    result = ResourceManagerEn;
                    break;
                case "ru":
                default:
                    result = ResourceManagerRu;
                    break;
            }

            return result;
        }
    }
}