using System.Collections.Generic;

namespace JibbR
{
    public class HelpDetails
    {
        public HelpDetails()
        {
            Usages = new List<string>();
        }

        public string Description { get; set; }
        public List<string> Usages { get; set; }
    }
}