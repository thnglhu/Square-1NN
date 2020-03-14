using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Square_1NN.Extension;

namespace Square_1NN.Square1
{
    internal class CubeController : IInteractable
    {
        Cube cube;
        CubeView view;
        private Vector2 center = Vector2.Zero, center2 = Vector2.Zero;
        private static readonly Vector2 flippedDelta = new Vector2(0, 200);
        internal CubeController(Cube cube, CubeView view, IDisplayer displayer)
        {
            this.cube = cube;
            this.view = view;
            CubeView.Init(displayer.Manager());
        }
        public void Display(IDisplayer displayer)
        {
            view.Display(displayer, cube);
        }

        public void Locate(Vector2 position)
        {
            center = position - flippedDelta / 2;
            center2 = position + flippedDelta / 2;
        }
        private bool lockLayer = false;
        private bool preventRotation = false;
        private int flag = 0;
        private Vector2 lastMouse = Vector2.Zero;
        private Vector2 startMouse = Vector2.Zero;
        private Vector2 delta = Vector2.Zero;
        public void Press(int x, int y)
        {
            if (!lockLayer)
            {
                lockLayer = true;
                preventRotation = false;
                lastMouse = new Vector2(x, y);
                startMouse = new Vector2(x, y);
                delta = Vector2.Zero;
            }
        }
        public void Release()
        {
            if (lockLayer)
            {
                lockLayer = false;
            }
        }
        public void Update(int x, int y, GameTime game_time)
        {
            Vector2 position = new Vector2(x, y);
            if (!lockLayer)
            {
                int newFlag = 0;
                int baseX = (int)center.X, baseY = (int)center.Y;
                if (
                    new OrCriteria(
                        new RectangleCriteria(baseX, baseY - 23, 139, 23),
                        new IsoscelesTriangleCriteria(baseX, baseY, 139, 33)
                    ).MeetCriteria(x, y)) newFlag = 1;
                if (
                    new OrCriteria(
                        new RectangleCriteria(baseX, baseY - 23, 139, 23),
                        new IsoscelesTriangleCriteria(baseX, baseY, 139, 33)
                    ).MeetCriteria(x, y)) newFlag = 1;

            }
        }
        #region Control State
        internal bool RotateMajor()
        {
            if (!cube.Top.Rotatable()
                || !cube.Mid.Rotatable()
                || !cube.Bot.Rotatable())
                return false;
            cube.Top.Reverse(forMajor: true);
            cube.Mid.Reverse(forMajor: true);
            cube.Bot.Reverse(forMajor: true);
            (cube.Top.Major, cube.Bot.Major) = (cube.Bot.Major, cube.Top.Major);
            (cube.Top.MajorColor, cube.Bot.MajorColor) = (cube.Bot.MajorColor, cube.Top.MajorColor);
            view.UpdateAll(cube, center, center2, 0);
            return true;
        }
        internal bool RotateMinor()
        {
            if (!cube.Top.Rotatable()
                || !cube.Mid.Rotatable()
                || !cube.Bot.Rotatable())
                return false;
            cube.Top.Reverse(forMajor: false);
            cube.Mid.Reverse(forMajor: false);
            cube.Bot.Reverse(forMajor: false);
            (cube.Top.Minor, cube.Bot.Minor) = (cube.Bot.Minor, cube.Top.Minor);
            (cube.Top.MinorColor, cube.Bot.MinorColor) = (cube.Bot.MinorColor, cube.Top.MinorColor);
            view.UpdateAll(cube, center, center2, 0);
            return true;
        }
        internal void ShiftTop(int value)
        {
            cube.Top.Shift(value);
            view.UpdateTop(cube, center, center2, 0);
        }
        internal void ShiftBot(int value)
        {
            cube.Bot.Shift(-value);
            view.UpdateBot(cube, center, center2, 0);
        }
        #endregion

    }
}
