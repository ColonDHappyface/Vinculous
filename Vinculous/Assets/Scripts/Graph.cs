using System;

namespace DaburuTools
{
    // Graph.cs: A structure that contains simple graph equations that can be used in many applications
    public struct Graph
    {
        /// <summary>
        /// Clamps the value of _x to be between 0.0f and 1.0f. It will ignore any integers (other than 1 and 0) and absolutes all negatives
        /// </summary>
        /// <param name="_x"> The value to clamp </param>
        /// <returns> The clamped value of _x </returns>
        public static float ClampToGraph(float _x)
        {
            if (_x > 1f) _x %= 1f;
            if (_x < 0f) _x = -_x;
            return _x;
        }

        /// <summary>
        /// f(x) = x^2. Smooth start, Steep stop
        /// </summary>
        /// <param name="_x"> The value of x, use numbers between 0.0f to 1.0f </param>
        /// <returns> The f(x) value of the current function </returns>
        public static float Exponential(float _x) 
        {
            _x = ClampToGraph(_x);
            return _x * _x; 
        }

        /// <summary>
        /// f(x) = x^_nPower. The bigger the power, the smoother the start, the steeper the stop
        /// </summary>
        /// <param name="_x"> The value of x, use numbers between 0.0f to 1.0f </param>
        /// <param name="_nPower"> The integer that is used to exponentiate </param>
        /// <returns> The f(x) value of the current function </returns>
        public static float ExponentialByInt(float _x, int _nPower)
        {
            if (_nPower == 0) return 0f;

            _x = ClampToGraph(_x);
            if (_nPower < 0) _nPower = -_nPower;

            for (int i = 1; i < _nPower; i++)
                _x *= _x;
            return _x;
        }

        public static float ExponentialByFloat(float _x, float _fPower)
        {
            if (_fPower == 0f) return 0f;

            _x = ClampToGraph(_x);
            if (_fPower < 0f) _fPower = -_fPower;

            //return Mix(ExponentialByInt, Ex)
            return 0f;
        }

        /// <summary>
        /// f(x) = 1 - ((x - 1)^2). Steep start, Smooth stop
        /// </summary>
        /// <param name="_x"> The value of x, use numbers between 0.0f to 1.0f </param>
        /// <returns> The f(x) value of the current function </returns>
        public static float InverseExponential(float _x, Delegate x)
        {
            _x = ClampToGraph(_x);
            return 1f - (Exponential(_x - 1f));
        }

        /// <summary>
        /// Returns an f(x) based on the influence between equation A and B. (Example: If _fBInfluence = 0.3f, it will use 70% of A and 30% of B at any point)
        /// </summary>
        /// <param name="_GraphEquationA"> The first equation </param>
        /// <param name="_GraphEquationB"> The second equation </param>
        /// <param name="_fBInfluence"> The percentage of influence B will apply. If B is 0.3f(30% influence), A will be 0.7f(70% influence)</param>
        /// <param name="_x"> The value of x, use numbers between 0.0f to 1.0f </param>
        /// <returns></returns>
        public static float Mix(Func<float, float> _GraphEquationA, Func<float, float> _GraphEquationB, float _fBInfluence, float _x)
        {
            _x = ClampToGraph(_x);
            _fBInfluence = ClampToGraph(_fBInfluence);

            _fBInfluence = _fBInfluence > 1f ? _fBInfluence % 1 : _fBInfluence;
            return _GraphEquationA(_x) * (1f - _fBInfluence) + _fBInfluence * _GraphEquationB(_x);
        }


    }
}
