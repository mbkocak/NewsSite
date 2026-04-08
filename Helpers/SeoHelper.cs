using System.Text.RegularExpressions;
namespace News_Site.Helpers
{
    public class SeoHelper
    {
        public static string ToSeoUrl(string parVeri)
        {
            if (!string.IsNullOrEmpty(parVeri))
            {
                parVeri = parVeri.Trim();
                parVeri = parVeri.Replace("ã¢", "a")
                                 .Replace("ã‚", "a")
                                 .Replace("ãª", "e")
                                 .Replace("ãš", "e")
                                 .Replace("ã§", "c")
                                 .Replace("ã‡", "c")
                                 .Replace("äÿ", "g")
                                 .Replace("ä", "g")
                                 .Replace("ä°", "i")
                                 .Replace("ä±", "i")
                                 .Replace("ã¶", "o")
                                 .Replace("ã–", "o")
                                 .Replace("åÿ", "s")
                                 .Replace("å", "s")
                                 .Replace("ã¼", "u")
                                 .Replace("ãœ", "u")
                                 .Replace("â", "a")
                                 .Replace("Â", "a")
                                 .Replace("ê", "e")
                                 .Replace("Ê", "e")
                                 .Replace("ç", "c")
                                 .Replace("Ç", "c")
                                 .Replace("ğ", "g")
                                 .Replace("Ğ", "g")
                                 .Replace("İ", "i")
                                 .Replace("I", "i")
                                 .Replace("ı", "i")
                                 .Replace("î", "i")
                                 .Replace("Î", "i")
                                 .Replace("ö", "o")
                                 .Replace("Ö", "o")
                                 .Replace("ş", "s")
                                 .Replace("Ş", "s")
                                 .Replace("ü", "u")
                                 .Replace("Ü", "u")
                                 .Replace(" ", "-");

                parVeri = parVeri.ToLower();

                while (parVeri.Contains("--"))
                    parVeri = parVeri.Replace("--", "-");

                parVeri = Regex.Replace(parVeri, @"[^a-z0-9\-]", "");
            }

            return parVeri;
        }
    }
}
