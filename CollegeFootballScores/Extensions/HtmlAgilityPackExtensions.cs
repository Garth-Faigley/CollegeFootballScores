using HtmlAgilityPack;

namespace CollegeFootballScores.Extensions
{
    static class HtmlAgilityPackExtensions
    {
        public static HtmlNodeCollection NullGard(this HtmlNodeCollection self)
        {
            if (self == null)
                return new HtmlNodeCollection(null);

            return self;
        }
    }
}


// usage example: 
// var links = htmlDocument.DocumentNode
//   .SelectNodes("//a[@href]").NullGuard()
//   .Select(node => node.GetAttributeValue("href", ""));