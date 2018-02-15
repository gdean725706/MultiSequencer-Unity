using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.ProceduralAudio
{
    /// <summary>
    /// Read back wavetables with phasor
    /// Must call Tick() after each frame is read
    /// </summary>
    public class Phasor
    {
        public float _phase = 0f;
        public float _sr = 0f;
        public float _delta = 0f;
        public float _freq = 0f;
        public bool _firstTimeIn = false;

       /// <summary>
       /// Create a phasor with a set sample rate, frequency (Hz) and initial phase value.
       /// </summary>
        public Phasor(float sampleRate = 44100f, float frequency = 1f, float phase = 0f)
        {
            _phase = phase;
            _sr = sampleRate;
            _delta = frequency / sampleRate;
            _firstTimeIn = true;
        }

        /// <summary>
        /// Returns the current frequency in Hz that the phasor is playing back at
        /// </summary>
        /// <returns></returns>
        public float GetFrequency()
        {
            return _freq;
        }

        /// <summary>
        /// Sets the frequency in Hz for the phasor to play back at
        /// </summary>
        /// <param name="frequency"></param>
        public void SetFrequency(float frequency)
        {
            _freq = frequency;
            _delta = frequency / _sr;
        }

        /// <summary>
        /// Returns current phase value
        /// </summary>
        /// <returns></returns>
        public float GetPhase()
        {
            return _phase;
        }

        /// <summary>
        /// Manually set the phase position
        /// </summary>
        /// <param name="phase"></param>
        public void SetPhase(float phase)
        {
            _phase = phase;
        }

        /// <summary>
        /// Increment phasor by delta value.
        /// Must be called after each frame read.
        /// </summary>
        public void Tick()
        {
            _phase += _delta;
            if (_phase < 0f)
            {
                _phase += 1f;
            }
            else if (_phase >= 1f)
            {
                _phase -= 1f;
            }
        }
       
    };

    /// <summary>
    /// Class to create wavetables which can be read back with a phasor
    /// Specify size in constructor
    /// 512 is good
    /// </summary>
    public class Wavetable
    {
        private double[] m_table;

        /// <summary>
        /// Initialise a wavetable with a set size. 512 would be a good default size.
        /// Call any of the Create methods to fill the table with a chosen waveform.
        /// </summary>
        /// <param name="size"></param>
        public Wavetable(int size)
        {
            m_table = new double[size];
            CreateSquare();
        }
        
        public void CreateSquare()
        {
            for (int i = 0; i < m_table.Length; ++i)
            {
                if (i <= m_table.Length/2)
                {
                    m_table[i] = 1f;
                }
                else
                {
                    m_table[i] = -1f;
                }
            }
            m_table[m_table.Length - 1] = m_table[0];
        }

        public void CreateSine()
        {
            double f = 2.0 * Math.PI / (m_table.Length - 1);
            for (int i = 0; i < m_table.Length; ++i)
            {
                m_table[i] = Math.Sin(i*f);
            }
            m_table[m_table.Length - 1] = m_table[0];
        }

        public void CreateNoise()
        {
            var r = new System.Random();
            for (int i = 0; i < m_table.Length; ++i)
            {
                m_table[i] = ((r.Next(0, 1000000) * 0.000001) * 2.0f) - 1.0f;
            }
        }

        public double At(int index)
        {
            return m_table[index];
        }


        // Lookup float values in wavetable using linear interpolation
        public double LinearLookup(float index)
        {
            int aIndex, bIndex;
            double r, va, vb;
            // Splitting the float into integer and decimal parts
            r = index % 1; 
            aIndex = (int)index;
            bIndex = aIndex + 1;
            va = m_table[aIndex];
            vb = m_table[bIndex];
            return (1.0 - r) * va + r * vb;
        }

        public float GetSize()
        {
            return m_table.Length - 1;
        }
    }
}
