using UnityEngine;
using System.Collections;

public class PathManager {

	public static string ip = "";

	public static string net_base_path = "";
    public static string net_base_phone_path = "";

    public static string asset_path = "";
    public static string asset_config_path = "";

	public static string save_data_path = "";

	public static string local_data_path = "";

	public static string config_name = "config.txt";
	public static string res_path = "";

	public static string res_type = ".sd";
    public static string shared_path = "";
    public static string config_path = "";

    public static string cur_local_data_path = "";

	public static string UIPath = "Art/UI";

    public static string login_url = "http://121.40.181.118:8888/login";

	public static void Init() {

        net_base_path = @"http://10.128.2.51:8081/data/";

        asset_path = @"\\10.128.2.51\data\";

        net_base_phone_path = net_base_path + "Android/";

        shared_path = @"\\10.128.2.51\飞鱼共享\工作室\水星工作室\publish\";

		res_path = "file:///" + Application.streamingAssetsPath + "/";


#if UNITY_EDITOR

        res_path = "file:///" + Application.streamingAssetsPath + "/Android/";
        
		save_data_path = Application.persistentDataPath + "/";

		local_data_path = "file:///" + save_data_path;

#elif UNITY_ANDROID

        res_path = Application.streamingAssetsPath + "/Android/";

        save_data_path = Application.persistentDataPath + "/";

        local_data_path = "file:///" + save_data_path;


#elif UNITY_IOS

        res_path = "file:///" + Application.streamingAssetsPath + "/IOS/";

        net_base_phone_path = net_base_path + "IOS\\";

        save_data_path = Application.persistentDataPath + "/";

		local_data_path = "file:///" + save_data_path;


#endif

        config_path = net_base_path + config_name;

        asset_config_path = @"file:" + asset_path + config_name;

        Debug.Log ("res_path:" + res_path);
		Debug.Log ("net_base_path:" + net_base_path);
		Debug.Log ("save_data_path:" + save_data_path);
        Debug.Log ("local_data_path:" + local_data_path);
        Debug.Log ("net_base_phone_path:" + net_base_phone_path);
        Debug.Log ("config_path:" + config_path);

	
	}

	public static string GetAssetPath()
	{
		return Application.dataPath + "/";
	} 


}
