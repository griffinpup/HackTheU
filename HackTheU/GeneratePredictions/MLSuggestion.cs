using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning;
using Accord.Math;

namespace GeneratePredictions
{
  public class MLSuggestion : Predictor
  {
    public override ActivityDataBlock GenerateSuggestion()
    {
      ActivityDataBlock happiest_activity = data[0];
      double current_happiness = 0;
      foreach (ActivityDataBlock block in data)
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
