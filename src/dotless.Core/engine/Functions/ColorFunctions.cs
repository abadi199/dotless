/* Copyright 2009 dotless project, http://www.dotlesscss.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */

using System.Linq;
using dotless.Core.exceptions;

namespace dotless.Core.engine.Functions
{
    public class RedFunction : ColorFunctionBase
    {
        protected override INode Eval(Color color, INode[] args)
        {
            return new Number(color.R);
        }

        protected override string Name
        {
            get { return "red"; }
        }
    }

    public class GreenFunction : ColorFunctionBase
    {
        protected override INode Eval(Color color, INode[] args)
        {
            return new Number(color.G);
        }

        protected override string Name
        {
            get { return "green"; }
        }
    }

    public class BlueFunction : ColorFunctionBase
    {
        protected override INode Eval(Color color, INode[] args)
        {
            return new Number(color.B);
        }

        protected override string Name
        {
            get { return "blue"; }
        }
    }

    public class AlphaFunctionImpl : ColorFunctionBase
    {
        protected override INode Eval(Color color, INode[] args)
        {
            return new Number(color.A);
        }

        protected override INode EditColor(Color color, Number number)
        {
            return new Color(color.R, color.G, color.B, color.A + number.Value);
        }

        protected override string Name
        {
            get { return "alpha"; }
        }
    }

    // HACK: avoid Alpha from throwing an error in case of "filter: aplha(Opacity = x);"
    public class AlphaFunction : FunctionBase
    {
        public override INode Evaluate()
        {
            try
            {
                var function = new AlphaFunctionImpl();
                function.SetArguments(Arguments);
                return function.Evaluate();
            }
            catch(ParsingException)
            {
                var argumentString = string.Join(", ", Arguments.Select(x => x.ToCss()).ToArray());
                return new Literal(string.Format("ALPHA({0})", argumentString));
            }
        }
    }

    public class ComplementFunction : HslColorFunctionBase
    {
        protected override INode EvalHsl(HslColor color, INode[] args)
        {
            color.Hue += 0.5;
            return color.ToRgbColor();
        }

        protected override string Name
        {
            get { return "complement"; }
        }
    }

    public class GrayscaleFunction : ColorFunctionBase
    {
        protected override INode Eval(Color color, INode[] args)
        {
            var grey = (color.RGB.Max() + color.RGB.Min()) / 2;

            return new Color(grey, grey, grey);
        }

        protected override string Name
        {
            get { return "grayscale"; }
        }
    }
}