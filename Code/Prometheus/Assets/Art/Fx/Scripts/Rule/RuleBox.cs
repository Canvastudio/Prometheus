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
	
	};

	private static List<string> box_b_list = new List<string>() {
	
		"box_b_0",
	
	};

	private static List<string> box_s_list = new List<string>() {
	
		"box_s_0",
	
	};

	public static string GetBox(int row, int column) {
	
		if (column == 0 || column == 5 && Random.value > 0.95f)
			return box_s_list[Random.Range(0, box_s_list.Count - 1)];


		return box_l_list[Random.Range(0, box_l_list.Count - 1)];
	
	}

	public static string GetBlock(int row, int column) {
	
		return box_b_list[Random.Range(0, box_b_list.Count - 1)];
	
	}

	public static string GetUnExplored(int row, int clumn) {

		return box_d_list[Random.Range(0, box_d_list.Count - 1)];

	}

}
