using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtTestSkill : MonoBehaviour {

	public List<Transform> tranlist = new List<Transform>(2);

	public string skill_name = "liudan";

	public List<string> skilllist = new List<string> {

		"none",
		"none",
		"nangua",
		"liudan",
		"guanchuanshexian",
		"quandanqifa",
		"sanshe",
		"qiyoudan",
		"xuanguangshexian",
		"jingshencuoluan",
		"bianxing",
		"nuyi",
		"cuimian",
		"chenmo",
		"jinghuashuiyue",
		"shengmingchouqu",
		"zhongzidan",
		"ganraodan",
		"shenwei",
		"shikongzhuanyi",
		"siwangbiaoji",
		"chongdong",
		"pianzhuantuceng",
		"shangtongjiyi",
		"jisutianzhuang",
		"paotai",
		"fuyoua1xing",
		"fuyoua2xing",
		"baojing",
		"zibao",
		"dingshizhadan",
		"elingchenmo",
		"xiulijiqiren",
		"bingzao",
		"shanghaimianyi",
	
	};

	public string numstring = "";
	
	// Update is called once per frame
	void Update () {

//		if (Input.GetKeyDown(KeyCode.F)) {
//
//			ArtSkill.DoSkill(skill_name, tranlist[0].position, tranlist[1].position, null);
//		
//		}

		if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
		
			int number = 0;

			if (int.TryParse(numstring, out number)) {
			
				if (number > 0 && number < skilllist.Count) {
					skill_name = skilllist[number];
					//ArtSkill.DoSkill(skill_name, tranlist[0].position, tranlist[1].position, null);

					StartCoroutine(ArtSkill.DoSkillIE(skill_name, tranlist[0], tranlist[1]));

				}
			
			}


			numstring = "";
			return;
		
		}


		if (Input.anyKeyDown) {
			
			numstring += Input.inputString;
			Debug.Log(numstring);
		
		}


		
	}


	public void OnDrawGizmos() {

		tranlist[0] = transform.GetChild(0);
		tranlist[1] = transform.GetChild(1);

	}

}
