using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleBox {

	private float rate_normal = 1.0f;

	private static List<string> box_l_list = new List<string>() {
	
		"box_l_0",
		"box_l_1",
		"box_l_2",
		"box_l_3",
		"box_l_4",
		"box_l_5",
		"box_l_6",
	
	};

	private static List<string> box_h_list = new List<string>() {
	
		"box_h_0",
		"box_h_1",
	
	};

	private static List<string> box_d_list = new List<string>() {
	
		"box_d_0",
		"box_d_1",
		"box_d_2",
		"box_d_3",
		"box_d_4",
		"box_d_5",
	
	};

	private static List<string> box_b_list = new List<string>() {
	
		"box_b_0",
	
	};

	private static List<string> box_s_list = new List<string>() {
	
		"box_s_0",
	
	};

	private static List<string> Gnat_list = new List<string>() {
	
		"open_bug",
	　
	};

	private static List<string> box_k_list = new List<string>() {
		"box_k_0_lock",
	};


	private static List<string> box_b_lock_list = new List<string>() {
		"box_b_0_lock",
	};

	public static string GetBox(int row, int column) {

		return box_l_list[0];
	
		if ((column == 0 || column == 5) && Random.value > 0.95f)
			return box_s_list[Random.Range(0, box_s_list.Count)];


		return box_l_list[Random.Range(0, box_l_list.Count)];
	
	}

	public static string GetBlock(int row, int column) {
	
		return box_b_list[Random.Range(0, box_b_list.Count)];
	
	}

	public static string GetLock(int row, int column) {
		return box_k_list[Random.Range(0, box_k_list.Count)];
	}

	public static string GetBlockLock(int row, int colum) {
		return box_b_lock_list[Random.Range(0, box_b_lock_list.Count)];
	}

	public static string GetUnExplored(int row, int clumn) {

		return box_d_list[0];

		float r_value = Random.value;

		if(r_value < 0.93f)
			return box_d_list[Random.Range(0, 2)];

		if(r_value < 0.99)
			return box_d_list[Random.Range(2, 5)];

		return box_d_list[Random.Range(5, 6)];

	}

	public static void GetGnat(Vector3 pos) {
	
		if(Random.value > 0.9f)
			ArtSkill.Show(Gnat_list[Random.Range(0, Gnat_list.Count)], pos);
	
	}

	public static Color GetBoxColor() {
		float random_color = Random.Range(0.8f, 0.93f);
        return new Color(random_color,random_color,random_color,1);
	}

	public static Color GetBlockColor() {
		float random_color = 1;//Random.Range(0.3f, .4f);
        return new Color(random_color,random_color,random_color,1);
	}
	public static Color GetUnExploredColor() {

		float random_color = Random.Range(0.3f, 0.4f);

		//if(Random.value > 0.3f) random_color = Random.Range(0.87f, 0.92f);

        return new Color(random_color,random_color,random_color,1);
	}

    public static void OpenBoxTrigger(Vector3 pos) {
		GetGnat(pos);
	}

}
