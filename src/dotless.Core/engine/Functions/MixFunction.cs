using System.Linq;

namespace dotless.Core.engine.Functions
{
    public class MixFunction : FunctionBase
    {
        public override INode Evaluate()
        {
            if (Arguments.Length < 2)
                throw new exceptions.ParsingException(string.Format("Expected at least 2 arguments in function 'mix'."));

            if (Arguments.Length > 3)
                throw new exceptions.ParsingException(string.Format("Expected at most 3 arguments in function 'mix'."));

            if (!Arguments.Take(2).All(arg => arg is Color))
                throw new exceptions.ParsingException(string.Format("First 2 arguments in function 'mix' mist be colors."));

            double weight = 50;
            if (Arguments.Length == 3)
            {
                var arg = Arguments[2];
                if (!(arg is Number))
                    throw new exceptions.ParsingException(string.Format("Expected number, found '{0}'.", arg));

                weight = (arg as Number).Value;
            }


            var colors = Arguments.Take(2).Cast<Color>().ToArray();

            var p = weight / 100.0;
            var w = p * 2 - 1;
            var a = colors[0].A - colors[1].A;

            var w1 = (((w * a == -1) ? w : (w + a) / (1 + w * a)) + 1) / 2.0;
            var w2 = 1 - w1;

            var rgb = colors[0].RGB.Select((x, i) => x * w1 + colors[1].RGB[i] * w2).ToArray();

            var alpha = colors[0].A * p + colors[1].A * (1 - p);

            var color = new Color(rgb[0], rgb[1], rgb[2], alpha);
            return color;
        }
    }
}