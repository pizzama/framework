using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProtoGameData;
using SFramework;
using SFramework.Game;
using SFramework.Tools.Math;
using UnityEngine;

namespace App.Inventory
{
    public class InventoryModel : RootModel
    {
        private ProtoUserData _userData;

        protected override void opening()
        {
            // read userData
            if (_userData == null)
                _userData = ReadData<ProtoUserData>();
            GetData("").Forget();
        }

        public void AddPackageBySid(int sid, int num)
        {
            try
            {
                ProtoItem item = GetPackageBySid(sid);
                if (item == null)
                {
                    item = new ProtoItem();
                    item.Sid = (uint)MathTools.RandomInt(100000, 999999);
                    item.Cid = (uint)sid;
                    item.Num = (uint)num;
                }
                else
                {
                    int left = (int)item.Num;
                    left += num;
                    if (left < 0)
                    {
                        _userData.Items.Remove(item);
                    }
                    else
                    {
                        item.Num += (uint)left;
                    }
                }
            }
            catch (System.Exception err)
            {
                Debug.LogError(err.ToString());
            }
        }

        public void AddPackageByCid(int cid, int num)
        {
            try
            {
                List<ProtoItem> items = GetPackageByCid(cid);
                if (items.Count == 0)
                {
                    if (num < 0)
                        return;
                    ProtoItem item = new ProtoItem();
                    item.Sid = (uint)MathTools.RandomInt(100000, 999999);
                    item.Cid = (uint)cid;
                    item.Num = (uint)num;
                    _userData.Items.Add(item);
                }
                else
                {
                    if (items.Count > 1)
                    {
                        throw new DataErrorException("Found multi item by cid");
                    }
                    else
                    {
						ProtoItem item = items[0];
                        int left = (int)item.Num;
                        left += num;
                        if (left < 0)
                        {
                            _userData.Items.Remove(item);
                        }
                        else
                        {
                            item.Num += (uint)left;
                        }
                    }
                }
            }
            catch (System.Exception err)
            {
                Debug.LogError(err.ToString());
            }
        }

        public List<ProtoItem> GetPackageByCid(int cid)
        {
            List<ProtoItem> result = new List<ProtoItem>();
            var items = _userData.Items;
            for (int i = 0; i < items.Count; i++)
            {
                ProtoItem it = items[i];
                if (it.Cid == cid)
                {
                    result.Add(it);
                }
            }

            return result;
        }

        public ProtoItem GetPackageBySid(int sid)
        {
            var items = _userData.Items;
            for (int i = 0; i < items.Count; i++)
            {
                ProtoItem it = items[i];
                if (it.Sid == sid)
                {
                    return it;
                }
            }

            return null;
        }

        public void SaveUserData()
        {
            SaveData<ProtoUserData>(_userData);
        }
    }
}
