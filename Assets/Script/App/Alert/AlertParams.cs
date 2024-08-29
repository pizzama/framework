using System;

namespace App.Alert
{
	public class AlertParams
	{
		private string _prefabName;
		private string _prefabPath;
        private string _content;
        private Action<object> _okAction;
        private Action<object> _cancelAction;

        public string Content
        {
            get
            {
                return this._content;
            }
            set
            {
                this._content = value;
            }
        }

        public Action<object> OkAction
        {
            get
            {
                return this._okAction;
            }
            set
            {
                this._okAction = value;
            }
        }

        public Action<object> CancelAction
        {
            get
            {
                return this._cancelAction;
            }
            set
            {
                this._cancelAction = value;
            }
        }

		public string PrefabName
		{
			get
			{
				return this._prefabName;
			}
		}
		public string PrefabPath
		{
			get
			{
				return this._prefabPath;
			}
		}
		
		public static AlertParams CreateNoticeTip()
		{
			return new AlertParams
			{
				_prefabName = "AlertTip",
				_prefabPath = "Assets/Prefabs/Alert/AlertTip.prefab"
			};
		}

	}
}