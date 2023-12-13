using Elements;
using Elements.Geometry;
using System.Collections.Generic;

namespace Houses
{
    public static class Houses
    {
        /// <summary>
        /// Adds houses to lots.
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A HousesOutputs instance containing computed results and the model with any new elements.</returns>
        public static HousesOutputs Execute(Dictionary<string, Model> inputModels, HousesInputs input)
        {
            // Your code here.
            var output = new HousesOutputs();

            // Gather inputs.
            var minimumOfFloors = input.MinimumOfFloors;
            var maximumOfFloors = input.MaximumOfFloors;
            var floorHeight = input.FloorHeight;

            var random = new Random(11);

            // Loop through each lot.
            var lots = inputModels.TryGetValue("Lots", out var lotsModel) ? lotsModel.AllElementsOfType<Site>() : new List<Site>();
            foreach (var lot in lots)
            {
                var height = random.Next((int)minimumOfFloors, (int)maximumOfFloors) * floorHeight;
                // Create a house.
                var lotHouse = new House(lot,
                                         input.MaximumFootprintStrategy == HousesInputsMaximumFootprintStrategy.Area ? input.MaximumArea : input.MaximumRatio,
                                         input.MaximumFootprintStrategy,
                                         height);
                output.Model.AddElement(lotHouse);
            }

            return output;
        }

    }
}