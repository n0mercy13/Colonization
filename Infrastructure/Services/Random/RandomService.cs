using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Codebase.Infrastructure
{
    public partial class RandomService
    {
        private readonly int _seed = 1;
        private readonly System.Random _random;

        public RandomService()
        {
            _random = new System.Random(_seed);
        }

        private float Range(float number1, float number2)
        {
            if(number2 >= number1)
                return number1 + (number2 - number1) * (float)_random.NextDouble();
            else
                return number2 + (number1 - number2) * (float)_random.NextDouble();
        }
    }

    public partial class RandomService : IRandomService
    {
        public Vector3 Range(Vector3 position1,  Vector3 position2) 
        { 
            float x = Range(position1.x, position2.x);
            float y = Range(position1.y, position2.y);
            float z = Range(position1.z, position2.z);

            return new Vector3(x, y, z);
        }
    }
}
