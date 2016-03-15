using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace golf.Models
{
    public class NewsClass
    {
        public List<NewsArticle> newarticle = new List<NewsArticle>();
        public List<Images> images = new List<Images>();
        public List<NewsArticleImage> ArticleImages = new List<NewsArticleImage>();
        public Images bild { get; set; }
        public NewsArticle NA  { get; set; }
        public Person logedPerson { get; set; }
    }
}