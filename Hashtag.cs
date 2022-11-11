using System;
using System.Collections.Generic;
using System.Text;

namespace NapierFilteringSystem
{
    public class Hashtag
    {
        public string hashtag
        {
            get;
            set;
        }
        public int count
        {
            get;
            set;
        }



        public Hashtag (string hashIn, int countIn)
        {
            hashtag = hashIn;
            count = countIn;
        }






    }
}
