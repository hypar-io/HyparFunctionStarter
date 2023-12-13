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

            var house = new House(new Color("Aqua"), 3, Polygon.Rectangle(5, 5));
            output.Model.AddElement(house);

            return output;
        }
    }
}