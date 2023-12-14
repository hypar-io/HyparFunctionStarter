using Elements;
using Elements.Geometry;
using System.Collections.Generic;

namespace DemoHouseJames2
{
    public static class DemoHouseJames2
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A DemoHouseJames2Outputs instance containing computed results and the model with any new elements.</returns>
        public static DemoHouseJames2Outputs Execute(Dictionary<string, Model> inputModels, DemoHouseJames2Inputs input)
        {
            // Your code here.
            var output = new DemoHouseJames2Outputs();

            var lots = new List<Site>();
            if (inputModels.TryGetValue("Lots", out var lotsModel))
            {
                lots.AddRange(lotsModel.AllElementsOfType<Site>());
            }

            // Gather inputs.
            var numberOfFloors = input.NumberOfFloors;
            var floorHeight = input.FloorHeight;

            var height = numberOfFloors * floorHeight;
            var color = new Color("aqua");

            foreach (var lot in lots)
            {
                var lotHouse = new House(lot, height, color);
                output.Model.AddElement(lotHouse);
            }

            var house = new House(new Color("aqua"), height, Polygon.Rectangle(5, 5));
            output.Model.AddElement(house);

            return output;
        }
    }
}