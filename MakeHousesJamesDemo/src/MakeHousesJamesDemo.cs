using Elements;
using Elements.Geometry;
using System.Collections.Generic;

namespace MakeHousesJamesDemo
{
    public static class MakeHousesJamesDemo
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A MakeHousesJamesDemoOutputs instance containing computed results and the model with any new elements.</returns>
        public static MakeHousesJamesDemoOutputs Execute(Dictionary<string, Model> inputModels, MakeHousesJamesDemoInputs input)
        {
            // Your code here.
            var output = new MakeHousesJamesDemoOutputs();

            var lots = new List<Site>();

            if (inputModels.TryGetValue("Lots", out var lotsModel))
            {
                lots = lotsModel.AllElementsOfType<Site>().ToList();
            }

            // Gather inputs.
            var floorHeight = input.FloorHeight;

            var random = new System.Random();

            foreach (var lot in lots)
            {
                var height = random.Next((int)input.MinimumFloors, (int)input.MaximumFloors) * floorHeight;

                var targetArea = input.MaxAreaStrategy == MakeHousesJamesDemoInputsMaxAreaStrategy.Area ? input.MaxArea : input.MaxRatio * lot.Perimeter.Area();

                var house = new House(lot, height, random.NextColor(), targetArea);

                output.Model.AddElement(house);
            }

            return output;
        }
    }
}