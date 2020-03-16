using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Square_1NN.Support;

namespace Square_1NN.Square1
{
    internal class CubeController : IInteractable
    {
        Cube cube;
        CubeView view;
        Vector2 position;
        private Vector2 center = Vector2.Zero, center2 = Vector2.Zero;
        SoundEffect moveSfx, rotateSfx;
        private static readonly Vector2 flippedDelta = new Vector2(0, 200);
        internal CubeController(Cube cube, CubeView view, IDisplayer displayer)
        {
            this.cube = cube;
            this.view = view;
            moveSfx = displayer.GetManager().Load<SoundEffect>("Sound/Move");
            rotateSfx = displayer.GetManager().Load<SoundEffect>("Sound/Rotate");
            CubeView.Init(displayer.GetManager());
            UpdateView();
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
            UpdateView();
        }
        private bool pressed = false;
        private bool preventRotation = false;
        private int flag = 0;
        private Vector2 lastMouse = Vector2.Zero;
        private Vector2 startMouse = Vector2.Zero;
        private Vector2 delta = Vector2.Zero;
        public void Press(int x, int y)
        {
            if (!pressed)
            {
                pressed = true;
                preventRotation = false;
                lastMouse = new Vector2(x, y);
                startMouse = new Vector2(x, y);
                delta = Vector2.Zero;
            }
        }
        public void Release()
        {
            if (pressed)
            {
                pressed = false;
                lastTime = null;
            }
        }

        #region Control State
        
        internal bool RotateMajor(bool sfxEnabled = false)
        {
            if (cube.Top.Rotatable() && cube.Mid.Rotatable() && cube.Bot.Rotatable())
            {
                cube.Top.Reverse(forMajor: true);
                cube.Mid.Reverse(forMajor: true);
                cube.Bot.Reverse(forMajor: true);
                (cube.Top.Major, cube.Bot.Major) = (cube.Bot.Major, cube.Top.Major);
                (cube.Top.MajorColor, cube.Bot.MajorColor) = (cube.Bot.MajorColor, cube.Top.MajorColor);
                (cube.Top.MajorSideColor, cube.Bot.MajorSideColor) = (cube.Bot.MajorSideColor, cube.Top.MajorSideColor);
                if (sfxEnabled) rotateSfx.Play();
                return true;
            }

            return false;
        }
        internal bool RotateMinor(bool sfxEnabled = false)
        {
            if (cube.Top.Rotatable() && cube.Mid.Rotatable() && cube.Bot.Rotatable())
            {
                cube.Top.Reverse(forMajor: false);
                cube.Mid.Reverse(forMajor: false);
                cube.Bot.Reverse(forMajor: false);
                (cube.Top.Minor, cube.Bot.Minor) = (cube.Bot.Minor, cube.Top.Minor);
                (cube.Top.MinorColor, cube.Bot.MinorColor) = (cube.Bot.MinorColor, cube.Top.MinorColor);
                (cube.Top.MinorSideColor, cube.Bot.MinorSideColor) = (cube.Bot.MinorSideColor, cube.Top.MinorSideColor);
                if (sfxEnabled) rotateSfx.Play();
                return true;
            }
            return false;
        }
        internal void ShiftTop(int value, bool sfxEnabled = false)
        {
            if (sfxEnabled) moveSfx.Play();
            cube.Top.Shift(value);
        }
        internal void ShiftBot(int value, bool sfxEnabled = false)
        {
            if (sfxEnabled) moveSfx.Play();
            cube.Bot.Shift(-value);
        }
        private void UpdateView()
        {
            view.Update(cube, center, center2, flag);
        }
        LinkedList<(int, int)> sequence = new LinkedList<(int, int)>();
        double? lastTime = null;
        double threshHold = 60;
        double total = 0;
        public void Update(int x, int y, GameTime gameTime)
        {
            total += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (total >= 50)
            {
                total -= 50;
                view.Count++;
            }
            return;
            if (sequence.Count > 0)
            {
                double currentTime = gameTime.TotalGameTime.TotalMilliseconds;
                if (lastTime == null) lastTime = currentTime;
                if (currentTime - lastTime > threshHold)
                {
                    if (threshHold > 10) threshHold--;
                    lastTime += threshHold;
                    (int, int) value = sequence.First.Value;
                    bool last = false;
                    if (value.Item1 < 0)
                    {
                        ShiftTop(-1, true);
                        value.Item1 += 1;
                    }
                    else if (value.Item1 > 0)
                    {
                        ShiftTop(1, true);
                        value.Item1 -= 1;
                    }
                    else if (value.Item2 < 0)
                    {
                        ShiftBot(-1, true);
                        value.Item2 += 1;
                    }
                    else if (value.Item2 > 0)
                    {
                        ShiftBot(1, true);
                        value.Item2 -= 1;
                    }
                    else last = true;
                    if (last)
                    {
                        RotateMajor(true);
                        sequence.RemoveFirst();
                    }
                    else sequence.First.Value = value;
                    UpdateView();
                }
                if (sequence.Count == 0) threshHold = 60;
            }
            else
            {
                if (!pressed)
                {
                    int newFlag = 0;
                    int baseX = (int)center.X - 70, baseY = (int)center.Y;
                    int refX = (int)center2.X - 70, refY = (int)center2.Y;
                    if ((
                        new IsoscelesTriangleCriteria(baseX, baseY + 38, 140, 35)
                        | new RectangleCriteria(baseX, baseY + 13, 140, 25)
                        ).MeetCriteria(x, y)) newFlag = 0b100;
                    if ((new IsoscelesTriangleCriteria(baseX, baseY + 12, 140, 35)
                        | new RectangleCriteria(baseX, baseY - 13, 140, 25)
                        ).MeetCriteria(x, y)) newFlag = 0b010;
                    if ((new IsoscelesTriangleCriteria(baseX, baseY - 14, 140, 35)
                        | new RectangleCriteria(baseX, baseY - 75, 140, 60)
                        ).MeetCriteria(x, y)) newFlag = 0b001;
                    if ((new IsoscelesTriangleCriteria(refX, refY + 38, 140, 35)
                        | new RectangleCriteria(refX, refY + 13, 140, 25)
                        ).MeetCriteria(x, y)) newFlag = 0b001;
                    if ((new IsoscelesTriangleCriteria(refX, refY + 12, 140, 35)
                        | new RectangleCriteria(refX, refY - 13, 140, 25)
                        ).MeetCriteria(x, y)) newFlag = 0b010;
                    if ((new IsoscelesTriangleCriteria(refX, refY - 14, 140, 35)
                        | new RectangleCriteria(refX, refY - 75, 140, 60)
                        ).MeetCriteria(x, y)) newFlag = 0b100;
                    if (flag != newFlag)
                    {
                        flag = newFlag;
                        UpdateView();
                    }
                }
                else
                {
                    if (flag != 0b000)
                    {
                        delta.X += x - lastMouse.X;
                        delta.Y += y - lastMouse.Y;
                        if (Math.Abs(delta.Y) > 20) delta.X = 0;
                        if (!preventRotation && Math.Abs(delta.Y) > 60)
                        {
                            if (y < position.Y)
                            {
                                Console.WriteLine(startMouse.X + " . " + (center.X + 25));
                                if (startMouse.X > center.X + 25) RotateMajor(true);
                                else RotateMinor(true);
                            }
                            else
                            {
                                Console.WriteLine(startMouse.X + " - " + (center2.X - 25));
                                if (startMouse.X > center2.X - 35) RotateMajor(true);
                                else RotateMinor(true);
                            }
                            delta.X = 0;
                            if (delta.Y < -60) delta.Y += 60;
                            if (delta.Y > 60) delta.Y -= 60;
                            UpdateView();
                            Release();
                            return;
                        }
                        if (Math.Abs(delta.X) > 20)
                        {
                            int dir = delta.X > 0 ? 1 : -1;
                            delta = Vector2.Zero;
                            switch (flag)
                            {
                                case 0b001: ShiftTop(y > position.Y ? dir : -dir, true); UpdateView(); break;
                                case 0b100: ShiftBot(y > position.Y ? -dir : dir, true); UpdateView(); break;
                            }
                            preventRotation = true;
                        }
                        lastMouse.X = x;
                        lastMouse.Y = y;
                    }
                }
            }
        }
        internal void Scramble(int number, bool forAnimation = false)
        {
            forAnimation = false;
            Cube backup = cube.Clone() as Cube;
            Random random = new Random();
            LinkedList<(int, int)> queue = new LinkedList<(int, int)>();
            for (int index = 0; index < number; index++)
            {
                List<(int, int)> available = cube.GetAvailableMoves();
                (int, int) randomPick = available[random.Next(available.Count)];
                queue.AddLast(randomPick);
                ShiftTop(randomPick.Item1);
                ShiftBot(randomPick.Item2);
                RotateMajor();
            }
            if (!forAnimation) UpdateView();
            else
            {
                flag = 0;
                cube = backup;
                sequence = queue;
            }
            view.Count = 0;
        }
        internal void Reset()
        {
            view.Count = 0;
            cube = Cube.Solved();
            UpdateView();
        }
        #endregion
    }
}
