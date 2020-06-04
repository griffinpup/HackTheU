using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratePredictions
{
  public abstract class Predictor
  {
    public List<Activity> data;
    public abstract Activity GenerateSuggestion();
    public void LoadData(List<Activity> _data)
    {
      data = _data;
    }
  }
}
