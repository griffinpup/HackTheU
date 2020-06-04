using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratePredictions
{
  public class NaiveSuggestion : Predictor
  {
    public override Activity GenerateSuggestion()
    {
            Activity happiest_activity = data[0];
      double current_happiness = 0;
      foreach (Activity block in data)
      {
        double happiness = 0;
        if (double.TryParse(block.happiness, out happiness))
        {
          if (happiness > current_happiness)
          {
            current_happiness = happiness;
            happiest_activity = block;
          }
        }
      }
      return happiest_activity;
    }
  }
}
