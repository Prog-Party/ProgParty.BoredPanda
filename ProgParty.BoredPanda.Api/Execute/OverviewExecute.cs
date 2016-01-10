using ProgParty.BoredPanda.Api.Result;
using ProgParty.BoredPanda.Api.Scrape;
using System.Collections.Generic;

namespace ProgParty.BoredPanda.Api.Execute
{
    public class OverviewExecute
    {
        public Parameter.OverviewParameter Parameters = new Parameter.OverviewParameter();

        public List<OverviewResult> Result;

        public bool Execute()
        {
            try
            {
                Result = new OverviewScrape(Parameters).Execute();
                return true;
            }
            catch(System.Exception e)
            {
                return false;
            }
        }
    }
}
