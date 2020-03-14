using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Square_1NN.Square1.Layer
{
    abstract class LayerBase
    {
        protected string major;
        protected string minor;

        public string Major { get => major; set => major = value; }
        public string Minor { get => minor; set => minor = value; }
        protected LayerBase()
        {

        }
        public LayerBase(string mask)
        {
            string[] split = mask.Split();
            if (split.Length != 2)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                major = split[0];
                minor = split[1];
            }
        }
        public virtual void Display()
        {
            Console.WriteLine(major + '\t' + minor);
        }
        public abstract bool Rotatable();
        public virtual void Shift(bool forward)
        {
            (string, string) backup = (Major, Minor);
            int count = forward ? 1 : -1;
            Major = ShiftString(backup.Item1, backup.Item2, count);
            Minor = ShiftString(backup.Item2, backup.Item1, count);
        }
        public virtual void Shift(int step)
        {
            while (step > 0)
            {
                Shift(true);
                step--;
            }
            while (step < 0)
            {
                Shift(false);
                step++;
            }
        }
        public virtual void Reverse(bool forMajor)
        {

            if (forMajor) Reverse(ref major);
            else Reverse(ref minor);
        }
        public static void Reverse(ref string target)
        {
            char[] array = target.ToCharArray();
            Array.Reverse(array);
            target = new string(array);
        }
        public static string ShiftString(string a, string b, int count)
        {
            count %= a.Length;
            return (b + a + b).Substring(b.Length + count, a.Length);
        }
        public abstract LayerBase Clone();
    }
}
