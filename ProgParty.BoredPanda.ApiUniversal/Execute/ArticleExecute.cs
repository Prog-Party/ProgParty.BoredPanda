using ProgParty.BoredPanda.ApiUniversal.Result;
using ProgParty.BoredPanda.ApiUniversal.Scrape;
using System;
using System.Collections.Generic;

namespace ProgParty.BoredPanda.ApiUniversal.Execute
{
    public class ArticleExecute
    {
        public Parameter.ArticleParameter Parameters = new Parameter.ArticleParameter();

        public ArticleResult Result;

        public bool Execute()
        {
            try
            {
                Result = new ArticleScrape(Parameters).Execute();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
