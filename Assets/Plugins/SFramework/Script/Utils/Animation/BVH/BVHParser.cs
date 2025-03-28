﻿/*
特别注意：BVH数据一定要有Tpose，一般要么是内置骨骼为Tpose，要么是内置为Apos而第一帧为Tpose

创建角色 随便找个avatar角色或者使用工程中提供的Assets/Models中的两个角色(unity-chan或Rin)
挂载脚本 将Scripts/BVHDriver.cs拖拽到创建的角色上
脚本设置
Bonemaps：设置关节对应关系 脚本的Bonemaps定义了unity自己的humanoid的关节与BVH数据关节的对应关系，最好按照骨骼的层级关系填写
FirstT：注意你的BVH文件第一帧是Tpos，或者说内置的skeleton就是T-pose，可以通过bvhacker查看（如CMU提供的BVH数据内置骨骼是A-pos，第一帧是Tpose，所以需要勾选；示例中的tmp.bvh的内置骨骼的Tpos，所以无需勾选）
TargetAvatar：创建的角色
filename：bvh的路径
强调
代码中需要注意bvh可视化的结果对不对，如果不对，说明全局旋转没计算对，这里按照ZYX的欧拉角顺序计算的全局旋转，如果是其它顺序，请重写一下BVHParser.cs里面的eul2quat函数。

还有可能由于模型缩放，unity角色的动作没问题但是位置有问题，那就需要调整scaleRatio了，请自行尝试。
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class BVHParser
    {
        public int frames = 0;
        public float frameTime = 1000f / 60f;
        public BVHBone root;
        private List<BVHBone> boneList;

        static private char[] charMap = null;
        private float[][] channels;
        private string bvhText;
        private int pos = 0;

        public class BVHBone
        {
            public string name;
            public List<BVHBone> children;
            public float offsetX, offsetY, offsetZ;
            public int[] channelOrder;
            public int channelNumber;
            public BVHChannel[] channels;

            private BVHParser bp;

            // 0 = Xpos, 1 = Ypos, 2 = Zpos, 3 = Xrot, 4 = Yrot, 5 = Zrot
            public struct BVHChannel
            {
                public bool enabled;
                public float[] values;
            }

            public BVHBone(BVHParser parser, bool rootBone)
            {
                bp = parser;
                bp.boneList.Add(this);
                channels = new BVHChannel[6];
                channelOrder = new int[6] { 0, 1, 2, 5, 3, 4 };
                children = new List<BVHBone>();

                bp.skip();
                if (rootBone)
                {
                    bp.assureExpect("ROOT");
                }
                else
                {
                    bp.assureExpect("JOINT");
                }
                bp.assure("joint name", bp.getString(out name));
                bp.skip();
                bp.assureExpect("{");
                bp.skip();
                bp.assureExpect("OFFSET");
                bp.skip();
                bp.assure("offset X", bp.getFloat(out offsetX));
                bp.skip();
                bp.assure("offset Y", bp.getFloat(out offsetY));
                bp.skip();
                bp.assure("offset Z", bp.getFloat(out offsetZ));
                bp.skip();
                bp.assureExpect("CHANNELS");

                bp.skip();
                bp.assure("channel number", bp.getInt(out channelNumber));
                bp.assure("valid channel number", channelNumber >= 1 && channelNumber <= 6);

                for (int i = 0; i < channelNumber; i++)
                {
                    bp.skip();
                    int channelId;
                    bp.assure("channel ID", bp.getChannel(out channelId));
                    channelOrder[i] = channelId;
                    channels[channelId].enabled = true;
                }

                char peek = ' ';
                do
                {
                    float ignored;
                    bp.skip();
                    bp.assure("child joint", bp.peek(out peek));
                    switch (peek)
                    {
                        case 'J':
                            BVHBone child = new BVHBone(bp, false);
                            children.Add(child);
                            break;
                        case 'E':
                            bp.assureExpect("End Site");
                            bp.skip();
                            bp.assureExpect("{");
                            bp.skip();
                            bp.assureExpect("OFFSET");
                            bp.skip();
                            bp.assure("end site offset X", bp.getFloat(out ignored));
                            bp.skip();
                            bp.assure("end site offset Y", bp.getFloat(out ignored));
                            bp.skip();
                            bp.assure("end site offset Z", bp.getFloat(out ignored));
                            bp.skip();
                            bp.assureExpect("}");
                            break;
                        case '}':
                            bp.assureExpect("}");
                            break;
                        default:
                            bp.assure("child joint", false);
                            break;
                    }
                } while (peek != '}');
            }
        }

        private bool peek(out char c)
        {
            c = ' ';
            if (pos >= bvhText.Length)
            {
                return false;
            }
            c = bvhText[pos];
            return true;
        }

        private bool expect(string text)
        {
            foreach (char c in text)
            {
                if (pos >= bvhText.Length || (c != bvhText[pos] && bvhText[pos] < 256 && c != charMap[bvhText[pos]]))
                {
                    return false;
                }
                pos++;
            }
            return true;
        }

        private bool getString(out string text)
        {
            text = "";
            while (pos < bvhText.Length && bvhText[pos] != '\n' && bvhText[pos] != '\r')
            {
                text += bvhText[pos++];
            }
            text = text.Trim();

            return (text.Length != 0);
        }

        private bool getChannel(out int channel)
        {
            channel = -1;
            if (pos + 1 >= bvhText.Length)
            {
                return false;
            }
            switch (bvhText[pos])
            {
                case 'x':
                case 'X':
                    channel = 0;
                    break;
                case 'y':
                case 'Y':
                    channel = 1;
                    break;
                case 'z':
                case 'Z':
                    channel = 2;
                    break;
                default:
                    return false;
            }
            pos++;
            switch (bvhText[pos])
            {
                case 'p':
                case 'P':
                    pos++;
                    return expect("osition");
                case 'r':
                case 'R':
                    pos++;
                    channel += 3;
                    return expect("otation");
                default:
                    return false;
            }
        }

        private bool getInt(out int v)
        {
            bool negate = false;
            bool digitFound = false;
            v = 0;

            // Read sign
            if (pos < bvhText.Length && bvhText[pos] == '-')
            {
                negate = true;
                pos++;
            }
            else if (pos < bvhText.Length && bvhText[pos] == '+')
            {
                pos++;
            }

            // Read digits
            while (pos < bvhText.Length && bvhText[pos] >= '0' && bvhText[pos] <= '9')
            {
                v = v * 10 + (int)(bvhText[pos++] - '0');
                digitFound = true;
            }

            // Finalize
            if (negate)
            {
                v *= -1;
            }
            if (!digitFound)
            {
                v = -1;
            }
            return digitFound;
        }

        // Accuracy looks okay
        private bool getFloat(out float v)
        {
            bool negate = false;
            bool digitFound = false;
            int i = 0;
            v = 0f;            
            // Read sign
            if (pos < bvhText.Length && bvhText[pos] == '-')
            {
                negate = true;
                pos++;
            }
            else if (pos < bvhText.Length && bvhText[pos] == '+')
            {
                pos++;
            }     
            // Read digits before decimal point
            while (pos < bvhText.Length && bvhText[pos] >= '0' && bvhText[pos] <= '9')
            {
                v = v * 10 + (float)(bvhText[pos++] - '0');
                digitFound = true;
            }

            // Read decimal point
            if (pos < bvhText.Length && (bvhText[pos] == '.' || bvhText[pos] == ','))
            {
                pos++;
                // Read digits after decimal
                float fac = 0.1f;
                while (pos < bvhText.Length && bvhText[pos] >= '0' && bvhText[pos] <= '9' && i < 128)
                {
                    v += fac * (float)(bvhText[pos++] - '0');
                    fac *= 0.1f;
                    digitFound = true;
                }
            }

            // Finalize
            if (negate)
            {
                v *= -1f;
            }

            if (pos < bvhText.Length && bvhText[pos] == 'e')
            {
                string scienceNum = "10";
                while (pos < bvhText.Length && bvhText[pos] != ' ' && bvhText[pos] != '\t' && bvhText[pos] != '\n' && bvhText[pos] != '\r')
                {
                    scienceNum = scienceNum + bvhText[pos];
                    pos++;
                }
                v = v * (float)Double.Parse(scienceNum);
            }
            if (!digitFound)
            {
                v = float.NaN;
            }
            return digitFound;
        }

        private void skip()
        {
            while (pos < bvhText.Length && (bvhText[pos] == ' ' || bvhText[pos] == '\t' || bvhText[pos] == '\n' || bvhText[pos] == '\r'))
            {
                pos++;
            }
        }

        private void skipInLine()
        {
            while (pos < bvhText.Length && (bvhText[pos] == ' ' || bvhText[pos] == '\t'))
            {
                pos++;
            }
        }

        private void newline()
        {
            bool foundNewline = false;
            skipInLine();
            while (pos < bvhText.Length && (bvhText[pos] == '\n' || bvhText[pos] == '\r'))
            {
                foundNewline = true;
                pos++;
            }
            assure("newline", foundNewline);
        }

        private void assure(string what, bool result)
        {
            if (!result)
            {
                string errorRegion = "";
                for (int i = Math.Max(0, pos - 15); i < Math.Min(bvhText.Length, pos + 15); i++)
                {
                    if (i == pos - 1)
                    {
                        errorRegion += ">>>";
                    }
                    errorRegion += bvhText[i];
                    if (i == pos + 1)
                    {
                        errorRegion += "<<<";
                    }
                }
                throw new ArgumentException("Failed to parse BVH data at position " + pos + ". Expected " + what + " around here: " + errorRegion);
            }
        }

        private void assureExpect(string text)
        {
            assure(text, expect(text));
        }

        /*private void tryCustomFloats(string[] floats) {
            float total = 0f;
            foreach (string f in floats) {
                pos = 0;
                bvhText = f;
                float v;
                getFloat(out v);
                total += v;
            }
            Debug.Log("Custom: " + total);
        }

        private void tryStandardFloats(string[] floats) {
            IFormatProvider fp = CultureInfo.InvariantCulture;
            float total = 0f;
            foreach (string f in floats) {
                float v = float.Parse(f, fp);
                total += v;
            }
            Debug.Log("Standard: " + total);
        }

        private void tryCustomInts(string[] ints) {
            int total = 0;
            foreach (string i in ints) {
                pos = 0;
                bvhText = i;
                int v;
                getInt(out v);
                total += v;
            }
            Debug.Log("Custom: " + total);
        }

        private void tryStandardInts(string[] ints) {
            IFormatProvider fp = CultureInfo.InvariantCulture;
            int total = 0;
            foreach (string i in ints) {
                int v = int.Parse(i, fp);
                total += v;
            }
            Debug.Log("Standard: " + total);
        }

        public void benchmark () {
            string[] floats = new string[105018];
            string[] ints = new string[105018];
            for (int i = 0; i < floats.Length; i++) {
                floats[i] = UnityEngine.Random.Range(-180f, 180f).ToString();
            }
            for (int i = 0; i < ints.Length; i++) {
                ints[i] = ((int)Mathf.Round(UnityEngine.Random.Range(-180f, 18000f))).ToString();
            }
            tryCustomFloats(floats);
            tryStandardFloats(floats);
            tryCustomInts(ints);
            tryStandardInts(ints);
        }*/

        private void parse(bool overrideFrameTime, float time)
        {
            // Prepare character table
            if (charMap == null)
            {
                charMap = new char[256];
                for (int i = 0; i < 256; i++)
                {
                    if (i >= 'a' && i <= 'z')
                    {
                        charMap[i] = (char)(i - 'a' + 'A');
                    }
                    else if (i == '\t' || i == '\n' || i == '\r')
                    {
                        charMap[i] = ' ';
                    }
                    else
                    {
                        charMap[i] = (char)i;
                    }
                }
            }

            // Parse skeleton
            skip();
            assureExpect("HIERARCHY");

            boneList = new List<BVHBone>();
            root = new BVHBone(this, true);

            // Parse meta data
            skip();
            assureExpect("MOTION");
            skip();
            assureExpect("FRAMES:");
            skip();
            assure("frame number", getInt(out frames));
            skip();
            assureExpect("FRAME TIME:");
            skip();
            assure("frame time", getFloat(out frameTime));

            if (overrideFrameTime)
            {
                frameTime = time;
            }

            // Prepare channels
            int totalChannels = 0;
            foreach (BVHBone bone in boneList)
            {
                totalChannels += bone.channelNumber;
            }
            int channel = 0;
            channels = new float[totalChannels][];
            foreach (BVHBone bone in boneList)
            {
                for (int i = 0; i < bone.channelNumber; i++)
                {
                    channels[channel] = new float[frames];
                    bone.channels[bone.channelOrder[i]].values = channels[channel++];
                }
            }

            // Parse frames
            for (int i = 0; i < frames; i++)
            {
                newline();
                for (channel = 0; channel < totalChannels; channel++)
                {                    
                    skipInLine();
                    assure("channel value", getFloat(out channels[channel][i]));
                }
            }
        }

        public BVHParser(string bvhText)
        {
            this.bvhText = bvhText;

            parse(false, 0f);
        }

        public BVHParser(string bvhText, float time)
        {
            this.bvhText = bvhText;

            parse(true, time);
        }

  
        private Quaternion eul2quat(float z, float y, float x)
        {
            z = z * Mathf.Deg2Rad;
            y = y * Mathf.Deg2Rad;
            x = x * Mathf.Deg2Rad;

            // 动捕数据是ZYX，但是unity是ZXY
            float[] c = new float[3];
            float[] s = new float[3];
            c[0] = Mathf.Cos(x / 2.0f); c[1] = Mathf.Cos(y / 2.0f); c[2] = Mathf.Cos(z / 2.0f);
            s[0] = Mathf.Sin(x / 2.0f); s[1] = Mathf.Sin(y / 2.0f); s[2] = Mathf.Sin(z / 2.0f);

            return new Quaternion(
                c[0] * c[1] * s[2] - s[0] * s[1] * c[2],
                c[0] * s[1] * c[2] + s[0] * c[1] * s[2],
                s[0] * c[1] * c[2] - c[0] * s[1] * s[2],
                c[0] * c[1] * c[2] + s[0] * s[1] * s[2]
                );
        }

        public Dictionary<string,string> getHierachy()
        {
            Dictionary<string, string> hierachy = new Dictionary<string, string>();
            foreach (BVHBone bb in boneList)
            {
                foreach (BVHBone bbc in bb.children)
                {
                    hierachy.Add(bbc.name, bb.name);
                }
            }
            return hierachy;
        }

        public Dictionary<string,Quaternion> getKeyFrame(int frameIdx)
        {
            Dictionary<string, string> hierachy = getHierachy();
            Dictionary<string, Quaternion> boneData = new Dictionary<string, Quaternion>();
            boneData.Add("pos", new Quaternion(
                boneList[0].channels[0].values[frameIdx],
                boneList[0].channels[1].values[frameIdx],
                boneList[0].channels[2].values[frameIdx],0));
            
            boneData.Add(boneList[0].name, eul2quat(
                    boneList[0].channels[3].values[frameIdx],
                    boneList[0].channels[4].values[frameIdx],
                    boneList[0].channels[5].values[frameIdx]));
            foreach (BVHBone bb in boneList)
            {
                if (bb.name != boneList[0].name)
                {
                    Quaternion localrot = eul2quat(bb.channels[3].values[frameIdx],
                        bb.channels[4].values[frameIdx],
                        bb.channels[5].values[frameIdx]);
                    boneData.Add(bb.name, boneData[hierachy[bb.name]] * localrot);
                }                
            }            
            return boneData;
        }

        public Dictionary<string,Vector3> getOffset(float ratio) {
            Dictionary<string, Vector3> offset = new Dictionary<string, Vector3>();
            foreach(BVHBone bb in boneList)
            {
                offset.Add(bb.name, new Vector3(bb.offsetX * ratio, bb.offsetY * ratio, bb.offsetZ * ratio));
            }
            return offset;
        }
    }
}
