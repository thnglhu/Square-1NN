using System.Text;

namespace Square_1NN.Square1.Layer
{
    class ColoredMiddleLayer : MiddleLayer
    {
    string major_side_color, minor_side_color;
        public string MajorSideColor { get => major_side_color; set => major_side_color = value; }
        public string MinorSideColor { get => minor_side_color; set => minor_side_color = value; }
        protected ColoredMiddleLayer() : base() { }
        public ColoredMiddleLayer(string mask) : base(Convert(mask))
        {
            string[] split = mask.Split();
            StringBuilder side = new StringBuilder();
            int last = 0;
            foreach (char chr in Major)
            {
                MajorSideColor += char.ToLower(split[0][last]);
                if (chr == '4' || chr == '6' || chr == '8') last++;
            }
            last = 0;
            foreach (char chr in Minor)
            {
                MinorSideColor += char.ToLower(split[1][last]);
                if (chr == '4' || chr == '6' || chr == '8') last++;
            }
        }
        private static string Convert(string mask)
        {
            string[] split = mask.Split();
            StringBuilder builder = new StringBuilder();
            if (char.IsLower(split[0][0])) builder.Append("413826 ");
            else builder.Append("261384 ");
            if (char.IsLower(split[1][0])) builder.Append("431826");
            else builder.Append("261384");
            return builder.ToString();
        }
        public override void Shift(bool _)
        {
            base.Shift(_);
            string temp = MajorSideColor;
            MajorSideColor = MinorSideColor;
            MinorSideColor = temp;
        }
        public override void Reverse(bool forMajor)
        {
            base.Reverse(forMajor);
            if (forMajor)
            {
                Reverse(ref major_side_color);
            }
            else
            {
                Reverse(ref minor_side_color);
            }
        }
        public override LayerBase Clone()
        {
            return new ColoredMiddleLayer()
            {
                major = Major,
                minor = Minor,
                major_side_color = MajorSideColor,
                minor_side_color = MinorSideColor
            };
        }
    }

}
