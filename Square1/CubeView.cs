﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Square_1NN.Support;
using Square_1NN.Square1.Layer;
using System.Collections.Generic;

namespace Square_1NN.Square1
{
    class CubeView
    {
        private static bool init = false;
        private static Texture2D[] smallTop;
        private static Texture2D[] smallBottom;
        private static Texture2D[] smallTopSide;
        private static Texture2D[] smallBottomSide;
        private static Texture2D[] largeTop;
        private static Texture2D[] largeBottom;
        private static Texture2D[] largeTopSide;
        private static Texture2D[] largeBottomSide;
        private static Texture2D[] middleSide;
        private static Texture2D[] middleTop;
        private static Dictionary<char, Color> normalMapping = new Dictionary<char, Color>()
        {
            ['w'] = Color.White,
            ['y'] = Color.Yellow,
            ['r'] = Color.Red,
            ['g'] = Color.LawnGreen,
            ['b'] = Color.DeepSkyBlue,
            ['o'] = Color.Orange,
        };
        private static Dictionary<char, Color> highlightMapping = new Dictionary<char, Color>()
        {
            ['w'] = Color.AntiqueWhite,
            ['y'] = new Color(255, 250, 149),
            ['r'] = new Color(255, 89, 89),
            ['g'] = new Color(185, 255, 106),
            ['b'] = new Color(98, 220, 255),
            ['o'] = new Color(255, 196, 106),
        };
        public static void Init(ContentManager manager)
        {
            if (!init)
            {
                init = true;
                smallTop = new Texture2D[12];
                smallBottom = new Texture2D[12];
                largeTop = new Texture2D[12];
                largeBottom = new Texture2D[12];
                smallTopSide = new Texture2D[12];
                largeTopSide = new Texture2D[24];
                smallBottomSide = new Texture2D[12];
                largeBottomSide = new Texture2D[24];
                Texture2D TryLoad(string path)
                {
                    try
                    {
                        return manager.Load<Texture2D>(path);
                    }
                    catch (ContentLoadException)
                    {
                        return null;
                    }
                }
                for (int index = 1; index <= 12; index++)
                {
                    smallTop[index - 1] = TryLoad($"Pieces/smallTop{index}");
                    smallBottom[index - 1] = TryLoad($"Pieces/smallBottom{index}");
                    largeTop[index - 1] = TryLoad($"Pieces/largeTop{index}");
                    largeBottom[index - 1] = TryLoad($"Pieces/largeBottom{index}");
                    smallTopSide[index - 1] = TryLoad($"Pieces/smallTopSide{index}");
                    largeTopSide[2 * (index - 1)] = TryLoad($"Pieces/largeTopSide{2 * index - 1}");
                    largeTopSide[2 * (index - 1) + 1] = TryLoad($"Pieces/largeTopSide{2 * index}");
                    smallBottomSide[index - 1] = TryLoad($"Pieces/smallBottomSide{index}");
                    largeBottomSide[2 * (index - 1)] = TryLoad($"Pieces/largeBottomSide{2 * index - 1}");
                    largeBottomSide[2 * (index - 1) + 1] = TryLoad($"Pieces/largeBottomSide{2 * index}");
                }
                middleSide = new Texture2D[12];
                for (int index = 1; index <= 12; index++)
                {
                    middleSide[index - 1] = TryLoad($"Pieces/middleSide{index}");
                }
                middleTop = new Texture2D[8];
                for (int index = 1; index <= 8; index++)
                {
                    middleTop[index - 1] = TryLoad($"Pieces/middleTop{index}");
                }
            }
        }
        LinkedList<(Texture2D, Color, Vector2)> topOrder = new LinkedList<(Texture2D, Color, Vector2)>();
        LinkedList<(Texture2D, Color, Vector2)> midOrder = new LinkedList<(Texture2D, Color, Vector2)>();
        LinkedList<(Texture2D, Color, Vector2)> botOrder = new LinkedList<(Texture2D, Color, Vector2)>();
        LinkedList<(Texture2D, Color, Vector2)> refTopOrder = new LinkedList<(Texture2D, Color, Vector2)>();
        LinkedList<(Texture2D, Color, Vector2)> refMidOrder = new LinkedList<(Texture2D, Color, Vector2)>();
        LinkedList<(Texture2D, Color, Vector2)> refBotOrder = new LinkedList<(Texture2D, Color, Vector2)>();
        internal void Update(Cube cube, Vector2 position, Vector2 position2, int flag)
        {
            Clear();
            UpdateTop(cube, position, position2, flag);
            UpdateMid(cube, position, position2, flag);
            UpdateBot(cube, position, position2, flag);
        }
        internal void Clear()
        {
            topOrder.Clear();
            midOrder.Clear();
            botOrder.Clear();
            refTopOrder.Clear();
            refMidOrder.Clear();
            refBotOrder.Clear();
        }
        public int Count { get; set; } = 1;
        internal void Display(IDisplayer displayer, Cube cube)
        {
            List<(Texture2D texture, Color color, Vector2 position)> list = new List<(Texture2D texture, Color color, Vector2 position)>();
            list.AddRange(botOrder);
            list.AddRange(midOrder);
            list.AddRange(topOrder);
            list.AddRange(refBotOrder);
            list.AddRange(refMidOrder);
            list.AddRange(refTopOrder);
            int count = 0;
            foreach ((Texture2D texture, Color color, Vector2 position) in list)
            {
                if (texture == null) continue;
                if (count++ >= Count) break;
                displayer.DrawTexture(texture, color, position);
            }
        }
        internal void UpdateTop(Cube cube, Vector2 position, Vector2 position2, int flag)
        {
            Dictionary<char, Color> mapping = flag == 0b001 ? highlightMapping : normalMapping;
            string info = cube.Top.Major + cube.Top.Minor;
            string sideColor = cube.Top.MajorSideColor + cube.Top.MinorSideColor;
            string roofColor = cube.Top.MajorColor + cube.Top.MinorColor;
            BuildTop(position, info, sideColor, roofColor, mapping, topOrder);

            cube.Top.Shift(4);
            cube.Top.Reverse(forMajor: true);
            cube.Top.Reverse(forMajor: false);
            info = cube.Top.Minor + cube.Top.Major;
            sideColor = cube.Top.MinorSideColor + cube.Top.MajorSideColor;
            BuildBot(position2, info, sideColor, mapping, refBotOrder);
            cube.Top.Reverse(forMajor: false);
            cube.Top.Reverse(forMajor: true);
            cube.Top.Shift(-4);
        }
        internal void UpdateMid(Cube cube, Vector2 position, Vector2 position2, int flag)
        {
            Dictionary<char, Color> mapping = flag == 0b010 ? highlightMapping : normalMapping;
            string info = cube.Mid.Major + cube.Mid.Minor;
            string sideColor = cube.Mid.MajorSideColor + cube.Mid.MinorSideColor;
            BuildMid(position, info, sideColor, mapping, midOrder);

            info = cube.Mid.Minor + cube.Mid.Major;
            sideColor = cube.Mid.MinorSideColor + cube.Mid.MajorSideColor;
            LayerBase.Reverse(ref info);
            LayerBase.Reverse(ref sideColor);
            BuildMid2(position2, info, sideColor, mapping, refMidOrder);
        }
        internal void UpdateBot(Cube cube, Vector2 position, Vector2 position2, int flag)
        {
            Dictionary<char, Color> mapping = flag == 0b100 ? highlightMapping : normalMapping;
            string info = cube.Bot.Major + cube.Bot.Minor;
            string sideColor = cube.Bot.MajorSideColor + cube.Bot.MinorSideColor;
            BuildBot(position, info, sideColor, mapping, botOrder);

            cube.Bot.Shift(4);
            cube.Bot.Reverse(forMajor: true);
            cube.Bot.Reverse(forMajor: false);
            info = cube.Bot.Minor + cube.Bot.Major;
            sideColor = cube.Bot.MinorSideColor + cube.Bot.MajorSideColor;
            string roofColor = cube.Bot.MinorColor + cube.Bot.MajorColor;
            BuildTop(position2, info, sideColor, roofColor, mapping, refTopOrder);
            cube.Bot.Reverse(forMajor: false);
            cube.Bot.Reverse(forMajor: true);
            cube.Bot.Shift(-4);
        }

        private static void BuildBot(Vector2 position, string info, string sideColor, Dictionary<char, Color> color, LinkedList<(Texture2D, Color, Vector2)> list)
        {
            info = info.Substring(6) + info.Substring(0, 6);
            sideColor = sideColor.Substring(6) + sideColor.Substring(0, 6);
            for (int subindex = 0; subindex < 5; subindex++)
            {
                int code = info[subindex] - '0';
                int pos = (6 + subindex) % 12;
                if (code == 2)
                {
                    list.AddLast((smallBottomSide[pos], color[sideColor[subindex % sideColor.Length]], position));
                }
                else if (code == 1)
                {
                    list.AddLast((largeBottomSide[2 * pos], color[sideColor[subindex % sideColor.Length]], position));
                }
                else
                {
                    list.AddLast((largeBottomSide[(2 * pos + 23) % 24], color[sideColor[subindex % sideColor.Length]], position));
                }
                code = info[9 - subindex] - '0';
                pos = (15 - subindex) % 12;
                if (code == 2)
                {
                    list.AddLast((smallBottomSide[pos], color[sideColor[(9 + sideColor.Length - subindex) % sideColor.Length]], position));
                }
                else if (code == 1)
                {
                    list.AddLast((largeBottomSide[2 * pos], color[sideColor[(9 + sideColor.Length - subindex) % sideColor.Length]], position));
                }
                else
                {
                    list.AddLast((largeBottomSide[(2 * pos + 23) % 24], color[sideColor[(9 + sideColor.Length - subindex) % sideColor.Length]], position));
                }
            }
            int index = 6;
            foreach (char chr in info)
            {
                int code = chr - '0';
                if (code == 2)
                {
                    list.AddLast((smallBottom[index], color['w'], position));
                }
                else if (code == 1)
                {
                    list.AddLast((largeBottom[index], color['w'], position));
                }
                index = (index + 1) % 12;
            }
        }
        private static void BuildMid(Vector2 position, string info, string sideColor, Dictionary<char, Color> color, LinkedList<(Texture2D, Color, Vector2)> list)
        {
            if (info[0] == '4')
            {
                list.AddLast((middleSide[0], color[sideColor[0]], position));
                list.AddLast((middleTop[0], color['w'], position));
            }
            else
            {
                list.AddLast((middleSide[1], color[sideColor[0]], position));
                list.AddLast((middleTop[1], color['w'], position));
            }
            if (info[6] == '2')
            {
                list.AddLast((middleSide[2], color[sideColor[9]], position));
                list.AddLast((middleSide[3], color[sideColor[11]], position));
                list.AddLast((middleTop[2], color['w'], position));
            }
            else
            {
                list.AddLast((middleSide[4], color[sideColor[9]], position));
                list.AddLast((middleSide[5], color[sideColor[11]], position));
                list.AddLast((middleTop[3], color['w'], position));
            }
        }
        private static void BuildMid2(Vector2 position, string info, string sideColor, Dictionary<char, Color> color, LinkedList<(Texture2D, Color, Vector2)> list)
        {

            if (info[0] == '4')
            {
                list.AddLast((middleSide[8], color[sideColor[0]], position));
                list.AddLast((middleSide[9], color[sideColor[1]], position));
                list.AddLast((middleTop[6], color['w'], position));
            }
            else
            {
                list.AddLast((middleSide[10], color[sideColor[0]], position));
                list.AddLast((middleSide[11], color[sideColor[2]], position));
                list.AddLast((middleTop[7], color['w'], position));
            }
            if (info[6] == '2')
            {
                list.AddLast((middleSide[6], color[sideColor[11]], position));
                list.AddLast((middleTop[4], color['w'], position));
            }
            else
            {
                list.AddLast((middleSide[7], color[sideColor[11]], position));
                list.AddLast((middleTop[5], color['w'], position));
            }
        }
        private static void BuildTop(Vector2 position, string info, string sideColor, string roofColor, Dictionary<char, Color> color, LinkedList<(Texture2D, Color, Vector2)> list)
        {
            string backup = info;
            info = info.Substring(6) + info.Substring(0, 6);
            sideColor = sideColor.Substring(6) + sideColor.Substring(0, 6);
            for (int subindex = 0; subindex < 5; subindex++)
            {
                int code = info[subindex] - '0';
                int pos = (6 + subindex) % 12;
                if (code == 2)
                    list.AddLast((smallTopSide[pos], color[sideColor[subindex % sideColor.Length]], position));
                else if (code == 1)
                    list.AddLast((largeTopSide[2 * pos], color[sideColor[subindex % sideColor.Length]], position));
                else
                    list.AddLast((largeTopSide[(2 * pos + 23) % 24], color[sideColor[subindex % sideColor.Length]], position));
                code = info[9 - subindex] - '0';
                pos = (15 - subindex) % 12;
                if (code == 2)
                    list.AddLast((smallTopSide[pos], color[sideColor[(9 + sideColor.Length - subindex) % sideColor.Length]], position));
                else if (code == 1)
                    list.AddLast((largeTopSide[2 * pos], color[sideColor[(9 + sideColor.Length - subindex) % sideColor.Length]], position));
                else
                    list.AddLast((largeTopSide[(2 * pos + 23) % 24], color[sideColor[(9 + sideColor.Length - subindex) % sideColor.Length]], position));
            }
            int index = 0;
            foreach (char chr in backup)
            {
                int code = chr - '0';
                if (code == 2)
                    list.AddLast((smallTop[index], color[roofColor[index]], position));
                else if (code == 1)
                    list.AddLast((largeTop[index], color[roofColor[index]], position));
                index = (index + 1) % 12;
            }
        }
    }
}
