namespace App.Alert
{
	public class AlertParams
	{
		private string _prefabName;
		private string _prefabPath;
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
		
		public static AlertParams CreateTip()
		{
			return new AlertParams
			{
				_prefabName = "AlertTip",
				_prefabPath = "Assets/Prefabs/Alert/AlertTip.prefab"
			};
		}

	}
}