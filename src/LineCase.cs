using Elements;
using Elements.Geometry;
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

      var guides = input.Overrides.Guides.CreateElements(
        input.Overrides.Additions.Guides,
        input.Overrides.Removals.Guides,
        (add) => new Guide(add),
        (guide, identity) => guide.Match(identity),
        (guide, edit) => guide.Update(edit)
      );

      var output = new LineCaseOutputs();

      output.Model.AddElements(guides);

      return output;
    }
  }
}