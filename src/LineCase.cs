using Elements;
using Elements.Geometry;
using Elements.Millwork;
using Microsoft.CodeAnalysis;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace LineCase
{
  public static class LineCase
  {
    /// <summary>
    /// The LineCase function.
    /// </summary>
    /// <param name="model">The input model.</param>
    /// <param name="input">The arguments to the execution.</param>
    /// <returns>A LineCaseOutputs instance containing computed results and the model with any new elements.</returns>
    public static LineCaseOutputs Execute(Dictionary<string, Model> inputModels, LineCaseInputs input)
    {

      List<string> warnings = new List<string>();

      var guides = input.Overrides.Guides.CreateElements(
        input.Overrides.Additions.Guides,
        input.Overrides.Removals.Guides,
        (add) => new Guide(add),
        (guide, identity) => guide.Match(identity),
        (guide, edit) => guide.Update(edit)
      );

      List<string> millworkIdStrings = input.Overrides.Millwork
          .Select(mw => mw.Identity.ID.ToString())
          .ToList();

      // make the millwork
      List<Millwork> millwork = new List<Millwork>();
      foreach (var guide in guides)
      {

        Line lastSegment = null;
        double lastThickness = 0.0;
        var segmentCounter = 0;
        foreach (var segment in guide.Polyline.Segments())
        {
          var lineCounter = 0;
          double curLength = 0.0;
          while (curLength < segment.Length())
          {
            var ID = $"{guide.AddId}_{segmentCounter.ToString()}_{lineCounter.ToString()}";
            var mwo = input.Overrides.Millwork.FirstOrDefault(mw => mw.Identity.ID == ID);

            if (mwo == null)
            {
              if (curLength + 1 < segment.Length())
              {
                var line = new Line(segment.PointAtLength(curLength), segment.PointAtLength(curLength + 1));
                var _mwo = new MillworkOverride("", new MillworkIdentity(ID), new MillworkValue("Shelving", 1, 1, 1, false, -1, true, -1, null, -1));
                var mw = new Shelving(line, _mwo);

                lastThickness = 1.0;
                millwork.Add(mw);
              }
              curLength += 1;
            }
            else
            {
              var extend = mwo.Value.Extend;
              mwo.Value.Width = extend == true ? segment.Length() - curLength : mwo.Value.Width ?? 1.0;

              lastThickness = mwo.Value.Depth ?? 1.0;

              if (curLength + mwo.Value.Width <= segment.Length())
              {
                var line = new Line(segment.PointAtLength(curLength), segment.PointAtLength(curLength + (double)mwo.Value.Width));

                switch (mwo?.Value.MillworkType)
                {
                  case "Bar":
                    millwork.Add(new Bar(line, mwo));
                    break;
                  case "Shelving":
                    millwork.Add(new Shelving(line, mwo));
                    break;
                  case "Counter":
                    millwork.Add(new Counter(line, mwo));
                    break;
                  case "Display":
                    millwork.Add(new Display(line, mwo));
                    break;
                  case "Cabinet":
                    millwork.Add(new Cabinet(line, mwo));
                    break;
                  default:
                    millwork.Add(new Shelving(line, mwo));
                    // millwork.Add(new Millwork(line, 1, 1, 1, ID));
                    break;
                }
              }

              curLength += (double)mwo.Value.Width;
            }
            lineCounter++;
          }
          segmentCounter++;

          lastSegment = segment;
        }
      }

      var output = new LineCaseOutputs();

      output.Model.AddElements(guides);
      output.Model.AddElements(millwork);
      // output.Model.AddElements(millwork.SelectMany(m => m.SubElements));
      output.Errors.AddRange(warnings);

      return output;
    }
  }
}