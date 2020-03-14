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
        Vector2 position;
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
            this.position = position;
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
            view.Update(cube, center, center2, 0);
            return true;
        }
        internal bool RotateMinor()
        {
            if (cube.Top.Rotatable() && cube.Mid.Rotatable() && cube.Bot.Rotatable())
            {
                cube.Top.Reverse(forMajor: false);
                cube.Mid.Reverse(forMajor: false);
                cube.Bot.Reverse(forMajor: false);
                (cube.Top.Minor, cube.Bot.Minor) = (cube.Bot.Minor, cube.Top.Minor);
                (cube.Top.MinorColor, cube.Bot.MinorColor) = (cube.Bot.MinorColor, cube.Top.MinorColor);
                view.Update(cube, center, center2, 0);
                return true;
            }
            return false;
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
        public void Update(int x, int y, GameTime game_time)
        {
            if (!lockLayer)
            {
                int newFlag = 0;
                int baseX = (int)center.X - 70, baseY = (int)center.Y;
                int refX = (int)center2.X - 70, refY = (int)center2.Y;
                if (
                    new OrCriteria(
                        new OrCriteria(
                            new RectangleCriteria(baseX, baseY + 17, 139, 23),
                            new IsoscelesTriangleCriteria(baseX, baseY, 139, 33)),
                        new OrCriteria(
                            new RectangleCriteria(refX, refY - 67, 139, 55),
                            new IsoscelesTriangleCriteria(refX, refY - 12, 139, 33))
                    ).MeetCriteria(x, y)) newFlag = 0b001;
                if (
                    new OrCriteria(
                        new OrCriteria(
                            new RectangleCriteria(baseX, baseY - 9, 139, 23),
                            new IsoscelesTriangleCriteria(baseX, baseY + 14, 139, 33)),
                        new OrCriteria(
                            new RectangleCriteria(refX, refY - 9, 139, 23),
                            new IsoscelesTriangleCriteria(refX, refY + 14, 139, 33))
                    ).MeetCriteria(x, y)) newFlag = 0b010;
                if (
                    new OrCriteria(
                        new OrCriteria(
                            new RectangleCriteria(baseX, baseY - 67, 139, 55),
                            new IsoscelesTriangleCriteria(baseX, baseY - 12, 139, 33)),
                        new OrCriteria(
                            new RectangleCriteria(refX, refY + 17, 139, 23),
                            new IsoscelesTriangleCriteria(refX, refY, 139, 33))
                    ).MeetCriteria(x, y)) newFlag = 0b100;
                if (flag != newFlag)
                {
                    flag = newFlag;
                    switch (flag)
                    {
                        case 0b001: view.UpdateTop(cube, center, center2, flag); break;
                        case 0b010: view.UpdateMid(cube, center, center2, flag); break;
                        case 0b100: view.UpdateBot(cube, center, center2, flag); break;
                    }
                }
            }
            else
            {
                if (flag != 0b000)
                {
                    delta.X += x - lastMouse.X;
                    delta.Y += y - lastMouse.Y;
                    if (Math.Abs(delta.X) > 20) delta.X = 0;
                    if (!preventRotation && Math.Abs(delta.Y) > 60)
                    {
                        if (y > position.Y)
                        {
                            if (startMouse.X > center.X + 25) RotateMajor();
                            else RotateMinor();
                        }
                        else
                        {
                            if (startMouse.X > center2.X - 35) RotateMinor();
                            else RotateMajor();
                        }
                        delta.X = 0;
                        if (delta.Y < -60) delta.Y += 60;
                        if (delta.Y > 60) delta.Y -= 60;
                        Release();
                        return;
                    }
                    if (delta.X > 20)
                    {
                        delta = Vector2.Zero;
                        switch (flag)
                        {
                            case 0b001: ShiftTop(y > position.Y ? 1 : -1); break;
                            case 0b100: ShiftBot(y > position.Y ? -1 : 1); break;
                        }
                        preventRotation = true;
                    }

                }
            }
        }
        internal void Scramble(int number, bool forAnimation = false)
        {
            Cube backup = cube.Clone() as Cube;
            Random random = new Random();
            Queue<(int, int)> queue = new Queue<(int, int)>();
            for (int index = 0; index < number; index++)
            {
                List<(int, int)> available = cube.GetAvailableMoves();
                (int, int) randomPick = available[random.Next(available.Count)];
                queue.Enqueue(randomPick);
            }
        }
        internal void Reset()
        {

        }
        #endregion
    }
}
