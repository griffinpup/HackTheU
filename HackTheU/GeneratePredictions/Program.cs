using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratePredictions
{
  class Program
  {
    static void Main(string[] args)
    {
            //Predictor predictor;
            //while (true)
            //{
            //  GetData();
            //}
            Data temp = new Data();
            temp.SaveTestData(@"C:\Users\jodir\Desktop\Data\MostRecent.txt", new HashSet<User>(), true);

            HashSet<User> users = temp.ReadData(@"C:\Users\jodir\Desktop\Data\MostRecent.txt");
            temp.SaveTestData(@"C:\Users\jodir\Desktop\Data\MostRecentScrub.txt", temp.TopTwenty(users), false);
        }

    public static void GetData()
    {
      //Placeholder method
    }
  }
}
